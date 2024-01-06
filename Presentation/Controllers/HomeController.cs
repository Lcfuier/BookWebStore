using BussinessLogic.Interface;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;
        public HomeController(ILogger<HomeController> logger, IBookService bookService, ICategoryService categoryService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _bookService = bookService;
            _categoryService = categoryService;
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public async Task<IActionResult> Index(Guid? CategoryId,string SearchString)
        {
            var stopWatch=new Stopwatch();
            stopWatch.Start();
            //
            var books= await _bookService.GetAllBooksAsync();
            var cacheEntryOption = new MemoryCacheEntryOptions()
                                    .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(100))
                                    .SetPriority(CacheItemPriority.Normal);
            _memoryCache.Set("BookCache",books,cacheEntryOption);
            stopWatch.Stop();
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
            _logger.Log(LogLevel.Information, "Time " + stopWatch.ElapsedMilliseconds);
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