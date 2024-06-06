using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = CNT.Managment)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IWorkContainer _workContainer;


        public UserController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var currentUser = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return View(_workContainer.User.GetAll(a => a.Id != currentUser.Value));
        }


        [HttpGet]
        public IActionResult LockUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _workContainer.User.lockUser(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult UnlockUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _workContainer.User.unlockUser(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
