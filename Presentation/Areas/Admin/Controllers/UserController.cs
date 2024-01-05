using AutoMapper;
using BussinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Interface;
using Entity.Constants;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class UserController : Controller
    {
        private readonly BookWebStoreDbContext _db;
        public UserController(BookWebStoreDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region API CALLS

        public async Task<IActionResult> GetAllUsers()
        {
            List<Entity.Models.Customer> users = await _db.Customer.ToListAsync();

            // this will be the mapping between the users and the roles
            List<IdentityUserRole<string>> userRoles = await _db.UserRoles.ToListAsync();

            List<IdentityRole> roles = await _db.Roles.ToListAsync();

            // maping users
            List<CustomerDTO> customerDtos = new();

            foreach (var user in users)
            {
                string roleId = userRoles.FirstOrDefault(ur => ur.UserId == user.Id)?.RoleId ?? string.Empty;
                // add role for the user in users
                user.Role = roles.FirstOrDefault(r => r.Id == roleId)?.Name ?? string.Empty;

                CustomerDTO customerDto = new()
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    Role = user.Role,
                    LockoutEnd = user.LockoutEnd?.UtcDateTime ?? null,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                customerDtos.Add(customerDto);
            }

            return Json(new { data = customerDtos });
        }

        [HttpGet]
        public async Task<IActionResult> LockUnlockUser(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                return Json(new { success = false, message = "Không tìm thấy dữ liệu." });
            }

            DateTime currentDate = DateTime.Today;
            DateTime lockoutEndDate = user.LockoutEnd?.UtcDateTime ?? new DateTime(1000, 1, 1);

            if (lockoutEndDate > currentDate)
            {
                // unlock it
                user.LockoutEnd = currentDate.AddYears(-100);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Đã mở khóa." });
            }
            else
            {
                // lock it
                user.LockoutEnd = currentDate.AddYears(100);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Đã khóa." });
            }
        }

        #endregion


    }
}
