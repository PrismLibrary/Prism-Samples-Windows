

using System.Collections.Generic;
using AdventureWorks.WebServices.Models;

namespace AdventureWorks.WebServices.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetTodaysDealsProducts();
        IEnumerable<Product> GetProductsForCategory(int subcategoryId);
        IEnumerable<Product> GetProducts();
        Product GetProduct(string productNumber);
    }
}