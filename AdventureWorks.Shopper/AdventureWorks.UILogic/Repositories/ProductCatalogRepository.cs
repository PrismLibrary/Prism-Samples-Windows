using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.Repositories
{
    // Documentation on managing application data is at http://go.microsoft.com/fwlink/?LinkID=288818&clcid=0x409
    public class ProductCatalogRepository : IProductCatalogRepository
    {
        private readonly IProductCatalogService _productCatalogService;
        private readonly ICacheService _cacheService;
        private Dictionary<int, string> _rootCategoriesNames;

        public ProductCatalogRepository(IProductCatalogService productCatalogService, ICacheService cacheService)
        {
            _productCatalogService = productCatalogService;
            _cacheService = cacheService;
        }

        public async Task<ICollection<Category>> GetRootCategoriesAsync(int maxAmountOfProducts)
        {
            var rootCategories = await GetSubcategoriesAsync(0, maxAmountOfProducts);
            if (_rootCategoriesNames == null)
            {
                _rootCategoriesNames = new Dictionary<int, string>();
                foreach (var rootCategory in rootCategories)
                {
                    _rootCategoriesNames.Add(rootCategory.Id, rootCategory.Title);
                }
            }

            return rootCategories;
        }

        public async Task<ICollection<Category>> GetSubcategoriesAsync(int parentId, int maxAmountOfProducts)
        {
            string cacheFileName = String.Format("Categories-{0}-{1}", parentId, maxAmountOfProducts);

            try
            {
                // Case 1: Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ICollection<Category>>(cacheFileName);
            }
            catch (FileNotFoundException)
            {
            }

            // Retrieve the items from the service
            var categories = await _productCatalogService.GetCategoriesAsync(parentId, maxAmountOfProducts);

            // Save the items in the cache
            await _cacheService.SaveDataAsync(cacheFileName, categories);

            return categories;
        }

        public async Task<SearchResult> GetFilteredProductsAsync(string productsQueryString, int maxResults)
        {
            // Retrieve the items from the service
            var searchResult = await _productCatalogService.GetFilteredProductsAsync(productsQueryString, maxResults);
            return searchResult;
        }

        public async Task<ReadOnlyCollection<string>> GetSearchSuggestionsAsync(string searchTerm)
        {
            // Retrieve the search suggestions from the service
            var searchSuggestions = await _productCatalogService.GetSearchSuggestionsAsync(searchTerm);
            return searchSuggestions;
        }

        public async Task<ICollection<Product>> GetProductsAsync(int categoryId)
        {
            string cacheFileName = string.Format("SubProductsOfCategoryId{0}", categoryId);

            try
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ICollection<Product>>(cacheFileName);
            }
            catch (FileNotFoundException)
            {
            }

            // Retrieve the items from the service
            var products = await _productCatalogService.GetProductsAsync(categoryId);

            await _cacheService.SaveDataAsync(cacheFileName, products);

            return products;
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            string cacheFileName = string.Format("CategoryId{0}", categoryId);

            try
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<Category>(cacheFileName);
            }
            catch (FileNotFoundException)
            {
            }

            // Retrieve the items from the service
            var category = await _productCatalogService.GetCategoryAsync(categoryId);

            // Save the items in the cache
            await _cacheService.SaveDataAsync(cacheFileName, category);

            return category;
        }

        public async Task<Product> GetProductAsync(string productNumber)
        {
            string cacheFileName = string.Format("Product{0}", productNumber);

            try
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<Product>(cacheFileName);
            }
            catch (FileNotFoundException)
            {
            }

            // Retrieve the items from the service
            var product = await _productCatalogService.GetProductAsync(productNumber);

            // Save the items in the cache
            await _cacheService.SaveDataAsync(cacheFileName, product);

            return product;
        }

        public string GetCategoryName(int parentId)
        {
            if (_rootCategoriesNames == null)
            {
                return string.Empty;
            }

            return _rootCategoriesNames.ContainsKey(parentId) ? _rootCategoriesNames[parentId] : string.Empty;
        }
    }
}