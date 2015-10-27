// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetShoppingCartAsync(string shoppingCartId);

        Task AddProductToShoppingCartAsync(string shoppingCartId, string productIdToIncrement);

        Task RemoveProductFromShoppingCartAsync(string shoppingCartId, string productIdToDecrement);

        Task RemoveShoppingCartItemAsync(string shoppingCartId, string itemIdToRemove);

        Task DeleteShoppingCartAsync(string shoppingCartId);

        Task<bool> MergeShoppingCartsAsync(string anonymousShoppingCartId, string authenticatedShoppingCartId);
    }
}
