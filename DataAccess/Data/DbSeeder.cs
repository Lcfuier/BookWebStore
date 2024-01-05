using Entity.Constants;
using Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaulData(IServiceProvider serviceProvider)
        {
            var userMgr = serviceProvider.GetService<UserManager<Customer>>();
            var roleMgr = serviceProvider.GetService<RoleManager<IdentityRole>>();
            //adding role
            if (!await roleMgr.RoleExistsAsync(Roles.User))
            {
                await roleMgr.CreateAsync(new IdentityRole(Roles.User));
            }
            if (!await roleMgr.RoleExistsAsync(Roles.Librarian))
            {
                await roleMgr.CreateAsync(new IdentityRole(Roles.Librarian));
            }
            if (!await roleMgr.RoleExistsAsync(Roles.Admin))
            {
                await roleMgr.CreateAsync(new IdentityRole(Roles.Admin));
            }
            var admin = new Customer
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };
            var isAdminExist = await userMgr.FindByEmailAsync(admin.Email);
            if (isAdminExist is null)
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin);
            }
            else
            {

            }
        }
    }
}
