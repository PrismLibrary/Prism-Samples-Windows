using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Repositories
{
    using System.Collections.Generic;

    public interface IProductCatalogRepository
    {
        Task<ICollection<Category>> GetRootCategoriesAsync(int maxAmountOfProducts);

        Task<ICollection<Category>> GetSubcategoriesAsync(int parentId, int maxAmountOfProducts);

        Task<SearchResult> GetFilteredProductsAsync(string productsQueryString, int maxResults);

        Task<ICollection<Product>> GetProductsAsync(int categoryId);

        Task<ReadOnlyCollection<string>> GetSearchSuggestionsAsync(string searchTerm);

        Task<Category> GetCategoryAsync(int categoryId);

        Task<Product> GetProductAsync(string productNumber);

        string GetCategoryName(int parentId);
    }
}