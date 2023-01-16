using Data.Models;

namespace DAL.Repositories.Abstract
{
	public interface IProductRepository
    {

        IEnumerable<Product> GetProductsByName(string name);

    }
}
