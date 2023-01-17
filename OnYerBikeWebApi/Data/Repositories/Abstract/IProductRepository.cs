
using Data.Entities;

namespace DAL.Repositories.Abstract
{
	public interface IProductRepository
    {

        IEnumerable<Product> GetProductsByName(string name);

    }
}
