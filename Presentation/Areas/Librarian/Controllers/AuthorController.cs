using AutoMapper;
using BussinessLogic.Interface;
using Entity.Constants;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Presentation.Areas.Librarian.Controllers
{
    [Area("Librarian")]
    [Authorize(Roles = Roles.Librarian)]
    public class AuthorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAuthorService _authorService;
        public AuthorController(IMapper mapper, IAuthorService authorService)
        {
            _mapper = mapper;
            _authorService = authorService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? Id)
        {
            AuthorDTO authorDTO = new();
            if (Id is null)
            {
                ViewBag.Action = "Add";
                return View(authorDTO);
            }
            ViewBag.Action = "Update";
            authorDTO = _mapper.Map<AuthorDTO>(await _authorService.GetAuthorByIdAsync(Id));
            if (authorDTO is null)
            {
                return NotFound();
            }
            return View(authorDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(AuthorDTO authorDTO)
        {
            var ExistAuthor = await _authorService.GetAuthorByIdAsyncNoTracking(authorDTO.AuthorId);
            var author = _mapper.Map<Author>(authorDTO);
            if (ExistAuthor is null)
            {
               await _authorService.AddAuthorAsync(author);
                TempData["success"] = "Thêm thành công !";
            }
            else
            {
                await _authorService.UpdateAuthorAsync(author);
                TempData["success"] = "Cập nhật thành công !";
            }
            return RedirectToAction(nameof(Index));
        }
       
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            return Json(new { data = await _authorService.GetAllAuthorsAsync() });
        }

        [HttpGet]
        public async Task<IActionResult> SearchAuthors(string term)
        {
            IEnumerable<Author> authors = await _authorService.GetAuthorsByTermAsync(term);

            return Json(authors.Select(a => new { id = a.AuthorId, label = a.FullName }));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            Author? author = await _authorService.GetAuthorByIdAsync(id);
            if (author is null)
            {
                return Json(new { success = false, message = "Không tìm thấy tác giả bạn muốn xóa." });
            }

            await _authorService.RemoveAuthorAsync(author);
            return Json(new { success = true, message = "Đã xóa thành công." });
        }

        #endregion
    }
}
