using BlogCore.Models;

namespace BlogCore.DataAccess.Data.Repository.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        void lockUser(string idUser);
        void unlockUser(string idUser);
    }
}
