using DAL.Context;
using DAL.Models;
using DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
