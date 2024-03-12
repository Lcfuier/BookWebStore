using AutoMapper;
using BussinessLogic.Interface;
using BussinessLogic.Service;
using Entity.Constants;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Librarian.Controllers
{
    [Area("Librarian")]
    [Authorize(Roles = Roles.Librarian + "," + Roles.Admin)]
    public class PublisherController : Controller
    {
        private readonly IPublisherService _service;
        private readonly IMapper _mapper;

        public PublisherController(IPublisherService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? id)
        {
            PublisherDTO? publisherDTO = new();
            if (id is null)
            {
                ViewBag.Action = "Add";
                return View(publisherDTO);
            }

            ViewBag.Action = "Update";
            publisherDTO = _mapper.Map<PublisherDTO>(
                await _service.GetPublisherByIdAsync(id.GetValueOrDefault()));
            if (publisherDTO is null)
            {
                return NotFound();
            }
            return View(publisherDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(PublisherDTO publisherDTO)
        {
            var ExistPublisher = await _service.GetPublisherByIdAsyncNoTracking(publisherDTO.PublisherId);
            var publisher = _mapper.Map<Publisher>(publisherDTO);
            if (ExistPublisher is null)
            {
                await _service.AddPublisherAsync(publisher);
                TempData["success"] = "Thêm thành công !";
            }
            else
            {
                await _service.UpdatePublisherAsync(publisher);
                TempData["success"] = "Cập nhật thành công !";
            }
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAllPublishers()
        {
            return Json(new { data = await _service.GetAllPublishersAsync() });
        }

        [HttpGet]
        public async Task<IActionResult> SearchPublishers(string term)
        {
            IEnumerable<Publisher> publishers = await _service.GetPublishersByTermAsync(term);

            return Json(publishers.Select(p => new { id = p.PublisherId, label = p.Name }));
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePublisher(Guid id)
        {
            Publisher? publisher = await _service.GetPublisherByIdAsync(id);
            if (publisher is null)
            {
                return Json(new { success = false, message = "Không tìm thấy nhà xuất bản bạn muốn xóa." });
            }

            await _service.RemovePublisherAsync(publisher);
            return Json(new { success = true, message = "Đã xóa thành công." });
        }

        #endregion
    }
}
