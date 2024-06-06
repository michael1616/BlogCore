using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = CNT.Managment)]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private readonly ApplicationDbContext _db;

        public CategoryController(IWorkContainer workContainer, ApplicationDbContext db)
        {
            _workContainer = workContainer;
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid) 
            {
                return View(category);
            }

            _workContainer.Category.Add(category);
            _workContainer.Save();

            TempData["Type"] = "success";
            TempData["Message"] = "Inserted correctly";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _workContainer.Category.Update(category);
            _workContainer.Save();

            TempData["Type"] = "success";
            TempData["Message"] = "Updated correctly";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category category = _workContainer.Category.GetById(id);

            if (category == null) 
            {
                return NotFound();
            }

            return View(category);
        }


        #region Api calls


        [HttpGet]
        public IActionResult GetAllCategories() 
        {
            var list = _workContainer.Category.GetAll();

            return Json(new { data = list });
        }

        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            Category category = _workContainer.Category.GetById(id);

            if (category == null) 
            {
                return Json(new { success = false, message = "Something went wrong" });
            }

            _workContainer.Category.Remove(category);
            _workContainer.Save();

            return Json(new { success = true, message = "was successfully removed" });
        }

        #endregion
    }
}
