// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    using System.Collections.Generic;

    public interface IProductCatalogService
    {
        Task<ICollection<Category>> GetCategoriesAsync(int parentId, int maxAmountOfProducts);

        Task<SearchResult> GetFilteredProductsAsync(string productsQueryString, int maxResults);

        Task<ReadOnlyCollection<string>> GetSearchSuggestionsAsync(string searchTerm);

        Task<ICollection<Product>> GetProductsAsync(int categoryId);

        Task<Category> GetCategoryAsync(int categoryId);

        Task<Product> GetProductAsync(string productNumber);
    }
}