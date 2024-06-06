using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using BlogCore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = CNT.Managment)]
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ArticleController(IWorkContainer workContainer, IWebHostEnvironment hostEnvironment)
        {
            _workContainer = workContainer;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticleViewModel article = new ArticleViewModel()
            {
                Article = new Models.Article(),
                ListCategories = _workContainer.Category.GetListCategories()
            };

            return View(article);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticleViewModel article = new ArticleViewModel()
            {
                Article = new Models.Article(),
                ListCategories = _workContainer.Category.GetListCategories()
            };

            if (id != null)
            {
                article.Article = _workContainer.Article.GetById(id.GetValueOrDefault());
            }

            return View(article);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticleViewModel articleViewModel)
        {

            if (!ModelState.IsValid)
            {
                articleViewModel.ListCategories = _workContainer.Category.GetListCategories();
                return View(articleViewModel);
            }

            var file = HttpContext.Request.Form.Files;

            if (file.Count <= 0)
            {
                ModelState.AddModelError("Article.ImageUrl", "You have to upload the file");
                articleViewModel.ListCategories = _workContainer.Category.GetListCategories();
                return View(articleViewModel);
            }

            string mainRoute = _hostEnvironment.WebRootPath;

            string fileName = Guid.NewGuid().ToString();
            string route = Path.Combine(mainRoute, @"Images\Articles");

            string ext = Path.GetExtension(file[0].FileName);

            using (FileStream stream = new FileStream(Path.Combine(route, $"{fileName}{ext}"), FileMode.Create))
            {
                file[0].CopyTo(stream);
            }

            articleViewModel.Article.ImageUrl = $@"\Images\Articles\{fileName}{ext}";
            articleViewModel.Article.CreationDate = DateTime.Now.ToString();

            _workContainer.Article.Add(articleViewModel.Article);
            _workContainer.Save();

            TempData["Type"] = "success";
            TempData["Message"] = "Inserted correctly";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticleViewModel articleViewModel)
        {

            if (!ModelState.IsValid)
            {
                articleViewModel.ListCategories = _workContainer.Category.GetListCategories();
                return View(articleViewModel);
            }

            var file = HttpContext.Request.Form.Files;
            Article articleFromDb = _workContainer.Article.GetById(articleViewModel.Article.IdArticle);

            articleViewModel.Article.CreationDate = articleFromDb.CreationDate;

            if (file.Count > 0)
            {
                string mainRoute = _hostEnvironment.WebRootPath;

                string oldRoute = Path.Combine(mainRoute, articleFromDb.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldRoute))
                {
                    System.IO.File.Delete(oldRoute);
                }

                string fileName = Guid.NewGuid().ToString();
                string route = Path.Combine(mainRoute, @"Images\Articles");

                string ext = Path.GetExtension(file[0].FileName);

                using (FileStream stream = new FileStream(Path.Combine(route, $"{fileName}{ext}"), FileMode.Create))
                {
                    file[0].CopyTo(stream);
                }

                articleViewModel.Article.ImageUrl = $@"\Images\Articles\{fileName}{ext}";
            }
            else
            {
                articleViewModel.Article.ImageUrl = articleFromDb.ImageUrl;
            }

            _workContainer.Article.Update(articleViewModel.Article);
            _workContainer.Save();

            TempData["Type"] = "success";
            TempData["Message"] = "Updated correctly";

            return RedirectToAction(nameof(Index));
        }

        #region Llamadas api

        [HttpGet]
        public IActionResult GetAllArticles()
        {
            return Json(new { data = _workContainer.Article.GetAll(includeProperties: "Category") });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Article articleFromDb = _workContainer.Article.GetById(id);

            if (articleFromDb == null)
            {
                return Json(new { success = false, message = "Something went wrong" });
            }

            string mainRoute = _hostEnvironment.WebRootPath;
            string oldRoute = Path.Combine(mainRoute, articleFromDb.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldRoute))
            {
                System.IO.File.Delete(oldRoute);
            }

            _workContainer.Article.Remove(articleFromDb);
            _workContainer.Save();

            return Json(new { success = true, message = "was successfully removed" });
        }


        #endregion
    }
}
