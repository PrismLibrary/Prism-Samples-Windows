

using AdventureWorks.WebServices.Models;

namespace AdventureWorks.WebServices.Repositories
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetById(string shoppingCartId);
        bool Delete(string userId);
        void AddProductToCart(string shoppingCartId, Product product);
        bool RemoveProductFromCart(string shoppingCartId, string productId);
        bool RemoveItemFromCart(ShoppingCart shoppingCart, string itemId);
    }
}