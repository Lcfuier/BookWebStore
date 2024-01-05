using BussinessLogic.Interface;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Presentation.Models;
using System.Diagnostics;
using System.IO;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        public HomeController(ILogger<HomeController> logger, IBookService bookService, ICategoryService categoryService)
        {
            _logger = logger;
            _bookService = bookService;
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(Guid? CategoryId,string SearchString)
        {
            var books= await _bookService.GetAllBooksAsync();
            var categories= await _categoryService.GetAllCategoryAsync();
            if (!String.IsNullOrEmpty(SearchString))
            {
                books = books.Where(s => s.Title.ToLower().Contains(SearchString.ToLower()));
            }
            if(CategoryId!= null)
            {
                books=books.Where(s=>s.Categories.Any(a=>a.CategoryId==CategoryId));
            }
            BookDisplayModel vm = new BookDisplayModel
            {
                Books=books,
                categories= categories,
                SearchString=SearchString,
                CategoryId=CategoryId
            };
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}