using AutoMapper;
using BussinessLogic.Interface;
using BussinessLogic.Service;
using DataAccess.Interface;
using Entity.Constants;
using Entity.DTOs;
using Entity.Models;
using Entity.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Presentation.Areas.Librarian.Controllers
{
    [Area("Librarian")]
    [Authorize(Roles =Roles.Librarian)]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAuthorService _authorService;
        private readonly IPublisherService _publisherService;
        public BookController(IBookService bookService, IUnitOfWork unitOfWork, ICategoryService categoryService, IWebHostEnvironment hostEnvironment,IAuthorService authorService, IPublisherService publisherService)
        {
            _bookService = bookService;
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
            _hostEnvironment = hostEnvironment;
            _authorService = authorService;
            _publisherService = publisherService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? Id)
        {  
            UpdateBookDTO vm = new()
            {
                BookDto = new BookDTO(),
                Categories= await _categoryService.GetAllCategoryAsync(),
                author= await _authorService.GetAllAuthorsAsync(),
                publisher = await _publisherService.GetAllPublishersAsync()
            };
            if (Id is null)
            {
                ViewBag.Action = "Add";
                return View(vm);
            }
            ViewBag.Action = "Update";
            BookDTO? book = await _bookService.GetBookDtoByIdAsync(Id);

            if(book is null)
            {
                return NotFound();
            }
            vm.BookDto = book;


            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(BookDTO bookDto)
        {
            string WebRootPath=_hostEnvironment.WebRootPath;
            IFormFileCollection files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString(); 
                string pathUploads = Path.Combine(WebRootPath, @"images\book");
                string fileExtension = Path.GetExtension(files[0].FileName);
                if (bookDto.ImageUrl is not null)
                {
                    // this is an update
                    string imagePath = Path.Combine(WebRootPath, bookDto.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                using (FileStream fs = new(
                    Path.Combine(pathUploads, fileName + fileExtension), FileMode.Create))
                {
                    await files[0].CopyToAsync(fs);
                }

                bookDto.ImageUrl =  fileName + fileExtension;
            }
            if (bookDto.BookId==Guid.Empty)
            {
                await _bookService.AddBookAsync(bookDto);
                TempData["success"] = "Thêm thành công !";
            }
            else
            {
                await _bookService.UpdateBookAsync(bookDto);
                TempData["success"] = "Cập nhật thành công !";
            }
            return RedirectToAction("Index");
        }
            #region API CALLS

            [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            IEnumerable<Book> books = await _bookService.GetAllBooksAsync();

            return Json(new
            {
                data = books.Select(b => new
                {
                    b.BookId,
                    b.Title,
                    b.Price,
                    b.NumberOfPage,
                    Author = b.Author.FullName,
                    Categories = b.Categories.Select(c => c.Name)
                })
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            Book? book = await _bookService.GetBookByIdAsync(id);
            if (book is null)
            {
                return Json(new { success = false, message = "Lỗi khi xóa!" });
            }

            if (book.ImageURL is not null)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                string imagePath = Path.Combine(webRootPath, book.ImageURL.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await _bookService.RemoveBookAsync(book);
            return Json(new { success = true, message = "Xóa thành công!" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            Book? book = await _bookService.GetBookByIdAsync(id);
            if (book is null)
            {
                return Json(new { success = false, message = "không tìm thấy sách!" });
            }

            if (book.ImageURL is null)
            {
                return Json(new { success = false, message = "Không có ảnh!" });
            }

            string webRootPath = _hostEnvironment.WebRootPath;
            string imagePath = Path.Combine(webRootPath, book.ImageURL.TrimStart('\\'));
            if (!System.IO.File.Exists(imagePath))
            {
                return Json(new { success = false, message = "Ảnh url Không còn tồn tại!" });
            }

            System.IO.File.Delete(imagePath);
            book.ImageURL = null;

            await _bookService.UpdateBookAsync(book);

            return Json(new { success = true, message = "Xóa thành công!" });
        }

        #endregion
    }
}
