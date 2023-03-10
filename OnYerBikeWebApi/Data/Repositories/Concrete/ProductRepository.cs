using DAL.Context;
using DAL.Repositories.Abstract;
using Data.Entities;

namespace DAL.Repositories.Concrete
{
	public class ProductRepository : IProductRepository
    {

        private BikeShopDbContext _context;

        public ProductRepository()
        {
            _context = new BikeShopDbContext();          
        }

        public ProductRepository(BikeShopDbContext _context)
        {
            this._context = _context;          
        }

        public IEnumerable<Product> GetProductsByName(string name)
        {
            return _context.Products.Where(p => p.Name.Contains(name));
        }

    }
}
