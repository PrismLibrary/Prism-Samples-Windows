

using System;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;
namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockShoppingCartService : IShoppingCartService
    {
        public Func<string, Task<ShoppingCart>> GetShoppingCartAsyncDelegate { get; set; }
        public Func<string, string, Task> AddProductToShoppingCartAsyncDelegate { get; set; }
        public Func<string, string, Task> RemoveProductFromShoppingCartAsyncDelegate { get; set; }
        public Func<string, string, Task> RemoveShoppingCartItemDelegate { get; set; }
        public Func<string, Task> DeleteShoppingCartAsyncDelegate { get; set; }
        public Func<string, string, Task<bool>> MergeShoppingCartsAsyncDelegate { get; set; }

        public Task<ShoppingCart> GetShoppingCartAsync(string shoppingCartId)
        {
            return GetShoppingCartAsyncDelegate(shoppingCartId);
        }

        public Task AddProductToShoppingCartAsync(string shoppingCartId, string productId)
        {
            return AddProductToShoppingCartAsyncDelegate(shoppingCartId, productId);
        }

        public Task RemoveProductFromShoppingCartAsync(string shoppingCartId, string productId)
        {
            return RemoveProductFromShoppingCartAsyncDelegate(shoppingCartId, productId);
        }

        public Task RemoveShoppingCartItemAsync(string shoppingCartId, string itemId)
        {
            return RemoveShoppingCartItemDelegate(shoppingCartId, itemId);
        }

        public Task DeleteShoppingCartAsync(string shoppingCartId)
        {
            return DeleteShoppingCartAsyncDelegate(shoppingCartId);
        }

        public Task<bool> MergeShoppingCartsAsync(string oldShoppingCartId, string newShoppingCartId)
        {
            return MergeShoppingCartsAsyncDelegate(oldShoppingCartId, newShoppingCartId);
        }
    }
}
