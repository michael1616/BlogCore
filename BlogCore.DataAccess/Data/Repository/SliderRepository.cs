using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;

namespace BlogCore.DataAccess.Data.Repository
{
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;

        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Slider slider)
        {
            Slider sliderFromDb = _db.Sliders.FirstOrDefault(a => a.IdSlider == slider.IdSlider);

            sliderFromDb.Name = slider.Name;
            sliderFromDb.State = slider.State;
            sliderFromDb.ImageUrl = slider.ImageUrl;

            _db.SaveChanges();
        }
    }
}
