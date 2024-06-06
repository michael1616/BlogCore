using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using BlogCore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = CNT.Managment)]
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly IWorkContainer _workContainer;

        private readonly IWebHostEnvironment _webHostEnviroment;

        public SliderController(IWorkContainer workContainer, IWebHostEnvironment webHostEnviroment)
        {
            _workContainer = workContainer;
            _webHostEnviroment = webHostEnviroment;
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
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View(slider);
            }

            var file = HttpContext.Request.Form.Files;

            if (file.Count <= 0)
            {
                ModelState.AddModelError("ImageUrl", "You have to upload the file");
                return View(slider);
            }

            string mainRoute = _webHostEnviroment.WebRootPath;

            string fileName = Guid.NewGuid().ToString();
            string route = Path.Combine(mainRoute, @"Images\Sliders");

            string ext = Path.GetExtension(file[0].FileName);

            using (FileStream stream = new FileStream(Path.Combine(route, $"{fileName}{ext}"), FileMode.Create))
            {
                file[0].CopyTo(stream);
            }

            slider.ImageUrl = $@"\Images\Sliders\{fileName}{ext}";

            _workContainer.Slider.Add(slider);
            _workContainer.Save();

            TempData["Type"] = "success";
            TempData["Message"] = "Inserted correctly";

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            Slider slider = _workContainer.Slider.GetById(id);

            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {

            if (!ModelState.IsValid)
            {
                return View(slider);
            }

            var file = HttpContext.Request.Form.Files;
            Slider sliderFromDb = _workContainer.Slider.GetById(slider.IdSlider);

            if (file.Count > 0)
            {
                string mainRoute = _webHostEnviroment.WebRootPath;

                if (!string.IsNullOrEmpty(sliderFromDb.ImageUrl)) 
                {
                    string oldRoute = Path.Combine(mainRoute, sliderFromDb.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldRoute))
                    {
                        System.IO.File.Delete(oldRoute);
                    }
                }

                string fileName = Guid.NewGuid().ToString();
                string route = Path.Combine(mainRoute, @"Images\Sliders");

                string ext = Path.GetExtension(file[0].FileName);

                using (FileStream stream = new FileStream(Path.Combine(route, $"{fileName}{ext}"), FileMode.Create))
                {
                    file[0].CopyTo(stream);
                }

                slider.ImageUrl = $@"\Images\Sliders\{fileName}{ext}";
            }
            else
            {
                slider.ImageUrl = sliderFromDb.ImageUrl;
            }

            _workContainer.Slider.Update(slider);
            _workContainer.Save();

            TempData["Type"] = "success";
            TempData["Message"] = "Updated correctly";

            return RedirectToAction(nameof(Index));
        }


        #region Call Apis

        [HttpGet]
        public IActionResult GetAllSlider() 
        {
            return Json(new { data = _workContainer.Slider.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Slider slider = _workContainer.Slider.GetById(id);

            if (slider == null)
            {
                return Json(new { success = false, message = "Something went wrong" });
            }

            _workContainer.Slider.Remove(slider);
            _workContainer.Save();

            return Json(new { success = true, message = "was successfully removed" });
        }

        #endregion

    }
}
