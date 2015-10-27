// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockShoppingCartRepository : IShoppingCartRepository
    {
        public Func<Task> ClearCartAsyncDelegate { get; set; }

        public Func<Task<ShoppingCart>> GetShoppingCartAsyncDelegate { get; set; }

        public Func<string, Task> AddProductToShoppingCartAsyncDelegate { get; set; }

        public Func<string, Task> RemoveProductFromShoppingCartAsyncDelegate { get; set; } 

        public Func<string, Task> RemoveShoppingCartItemDelegate { get; set; }

        public Task ClearCartAsync()
        {
            if (ClearCartAsyncDelegate != null)
            {
                return ClearCartAsyncDelegate();
            }
            return Task.Factory.StartNew(() => { });
        }

        public Task<ShoppingCart> GetShoppingCartAsync()
        {
            return this.GetShoppingCartAsyncDelegate();
        }

        public Task AddProductToShoppingCartAsync(string productId)
        {
            return AddProductToShoppingCartAsyncDelegate(productId);
        }

        public Task RemoveProductFromShoppingCartAsync(string productId)
        {
            return RemoveProductFromShoppingCartAsyncDelegate(productId);
        }

        public Task RemoveShoppingCartItemAsync(string itemId)
        {
            return RemoveShoppingCartItemDelegate(itemId);
        }

        public UserInfo CurrentUser { get; set; }
    }
}
