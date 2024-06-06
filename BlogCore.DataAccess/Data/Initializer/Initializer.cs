using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Initializer
{
    public class Initializer : IInitializerDB
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public Initializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        void IInitializerDB.Initializer()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_db.Roles.Any(a => a.Name == CNT.Managment)) return;

            _roleManager.CreateAsync(new IdentityRole(CNT.Managment)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.User)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = "maicol.a16-@hotmail.com",
                Name = "Michael Alexander",
                Email = "maicol.a16-@hotmail.com",
                Adress = "Av 34-55a90",
                City = "Barcelona",
                Country = "España",
                EmailConfirmed = true
            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.Where(a => a.Email == "maicol.a16-@hotmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(user, CNT.Managment).GetAwaiter().GetResult();
        }
    }
}
