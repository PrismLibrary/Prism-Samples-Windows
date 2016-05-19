

using System.Collections.Generic;

namespace AdventureWorks.WebServices.Models
{
    public class ShoppingCart
    {
        public ShoppingCart(ICollection<ShoppingCartItem> shoppingCartItems)
        {
            ShoppingCartItems = shoppingCartItems;
        }

        public string ShoppingCartId { get; set; }

        public ICollection<ShoppingCartItem> ShoppingCartItems {get; private set;}

        public double FullPrice { get; set; }

        public double TotalDiscount { get; set; }

        public double TotalPrice { get; set; }

        public string Currency { get; set; }

        public double TaxRate { get; set; }
    }
}
