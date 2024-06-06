using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class WorkContainer : IWorkContainer
    {

        private readonly ApplicationDbContext _db;

        public ICategoryRepository Category { get; set; }

        public IArticleRepository Article { get; set; }

        public ISliderRepository Slider { get; set; }

        public IUserRepository User { get; set; }

        public WorkContainer(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Article = new ArticleRepository(_db);
            Slider = new SliderRepository(_db);
            User = new UserRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose() 
        {
            _db.Dispose();
        }
    }
}
