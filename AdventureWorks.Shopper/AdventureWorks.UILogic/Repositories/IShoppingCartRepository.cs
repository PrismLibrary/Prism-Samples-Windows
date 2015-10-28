using AdventureWorks.UILogic.Models;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetShoppingCartAsync();

        Task AddProductToShoppingCartAsync(string productId);

        Task RemoveProductFromShoppingCartAsync(string productId);

        Task RemoveShoppingCartItemAsync(string itemId);

        Task ClearCartAsync();
    }
}
