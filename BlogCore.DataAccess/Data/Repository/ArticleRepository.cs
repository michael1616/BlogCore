using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {

        private readonly ApplicationDbContext _db;

        public ArticleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Article article)
        {
            var objDB = _db.Articles.FirstOrDefault(a => a.IdArticle == article.IdArticle);

            objDB.Name = article.Name;
            objDB.Description = article.Description;
            objDB.ImageUrl = article.ImageUrl;
            objDB.CategoryId = article.CategoryId;

            _db.SaveChanges();
        }
    }
}
