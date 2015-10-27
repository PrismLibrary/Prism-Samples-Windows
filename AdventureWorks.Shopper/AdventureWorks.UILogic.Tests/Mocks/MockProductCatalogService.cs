// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    using System.Collections.Generic;

    public class MockProductCatalogService : IProductCatalogService
    {
        public Func<int, int, Task<ICollection<Category>>> GetSubcategoriesAsyncDelegate { get; set; }
        public Func<string, int, Task<SearchResult>> GetFilteredProductsAsyncDelegate { get; set; }
        public Func<int, Task<ICollection<Product>>> GetProductsAsyncDelegate { get; set; }
        public Func<string, Task<ReadOnlyCollection<string>>> GetSearchSuggestionsAsyncDelegate { get; set; }
        public Func<int, Task<Category>> GetCategoryAsyncDelegate { get; set; }
        public Func<string, Task<Product>> GetProductAsyncDelegate { get; set; }

        public Task<ICollection<Category>> GetCategoriesAsync(int parentId, int maxAmountOfProducts)
        {
            return this.GetSubcategoriesAsyncDelegate(parentId, maxAmountOfProducts);
        }

        public Task<SearchResult> GetFilteredProductsAsync(string productsQueryString, int maxResults)
        {
            return this.GetFilteredProductsAsyncDelegate(productsQueryString, maxResults);
        }

        public Task<ICollection<Product>> GetProductsAsync(int categoryId)
        {
            return this.GetProductsAsyncDelegate(categoryId);
        }

        public Task<Category> GetCategoryAsync(int categoryId)
        {
            return this.GetCategoryAsyncDelegate(categoryId);
        }

        public Task<Product> GetProductAsync(string productNumber)
        {
            return this.GetProductAsyncDelegate(productNumber);
        }

        public Task<ReadOnlyCollection<string>> GetSearchSuggestionsAsync(string searchTerm)
        {
            return this.GetSearchSuggestionsAsyncDelegate(searchTerm);
        }
    }
}