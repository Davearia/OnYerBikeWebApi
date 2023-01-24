using DAL.Context;
using Data.Entities;
using Data.Repositories.Abstract;

namespace Data.Repositories.Concrete
{
    public class UserRepository : IUserRepository
    {

        private BikeShopDbContext _context;

        public UserRepository()
        {
            _context = new BikeShopDbContext();
        }

        public UserRepository(BikeShopDbContext _context)
        {
            this._context = _context;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

    }
}
