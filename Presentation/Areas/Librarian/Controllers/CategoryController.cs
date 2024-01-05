using AutoMapper;
using BussinessLogic.Interface;
using Entity.Constants;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Librarian.Controllers
{
    [Area("Librarian")]
    [Authorize(Roles =Roles.Librarian)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? id)
        {
            CategoryDTO? categoryDTO = new();

            // for add
            if (id is null)
            {
                ViewBag.Action = "Add";
                return View(categoryDTO);
            }

            // for update
            ViewBag.Action = "Update";
            categoryDTO = _mapper.Map<CategoryDTO>(await _service.GetCategoryByIdAsync(id.GetValueOrDefault()));
            if (categoryDTO is null)
            {
                return NotFound();
            }
            return View(categoryDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CategoryDTO categoryDTO)
        {
            var ExistCategory = await _service.GetCategoryByIdAsyncNoTracking(categoryDTO.CategoryId);
            var category = _mapper.Map<Category>(categoryDTO);
            if (ExistCategory is null)
            {
                await _service.AddCategoryAsync(category);
                TempData["success"] = "Thêm thành công !";
            }
            else
            {
                await _service.UpdateCategoryAsync(category);
                TempData["success"] = "Cập nhật thành công !";
            }
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return Json(new { data = await _service.GetAllCategoryAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            Category? category = await _service.GetCategoryByIdAsync(id);
            if (category is null)
            {
                return Json(new { success = false, message = "Không tìm thấy thể loại bạn muốn xóa." });
            }

            await _service.RemoveCategoryAsync(category);
            return Json(new { success = true, message = "Đã Xóa thành công." });
        }

        #endregion
    }
}
