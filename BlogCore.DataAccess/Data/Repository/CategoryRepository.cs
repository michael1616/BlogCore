using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogCore.DataAccess.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetListCategories()
        {
            return _db.Categories.Select(a => new SelectListItem() 
            {
               Text = a.Name,
               Value = a.IdCategory.ToString()
            });
        }

        public void Update(Category category)
        {
            var objDB = _db.Categories.FirstOrDefault(a => a.IdCategory == category.IdCategory);

            objDB.Name = category.Name;
            objDB.Order = category.Order;

            _db.SaveChanges();
        }
    }
}
