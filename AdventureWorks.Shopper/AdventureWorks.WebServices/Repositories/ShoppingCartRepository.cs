

using System;
using System.Collections.Generic;
using System.Linq;
using AdventureWorks.WebServices.Models;

namespace AdventureWorks.WebServices.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        // key: user | value: shopping cart items
        private static Dictionary<string, ShoppingCart> _shoppingCarts = GetDefaultShoppingCarts();
        // Can't lock on _shoppingCarts because it gets overwritten in Reset method
        private static object _lock = new object();

        private static Dictionary<string, ShoppingCart> GetDefaultShoppingCarts()
        {
            var imageServerPath = System.Configuration.ConfigurationManager.AppSettings["ImageServerPath"];
            if (imageServerPath == null)
            {
                imageServerPath = "http://localhost/";
            }
            var shoppingCarts = new Dictionary<string, ShoppingCart>();
            var shoppingCartItems = new List<ShoppingCartItem>();
            shoppingCartItems.Add(new ShoppingCartItem { Currency = "USD", Quantity = 1, Product = new Product { Currency = "USD", Title = "Mountain-400-W Silver, 42", ProductNumber = "BK-M38S-42", SubcategoryId = 1, Description = "This bike delivers a high-level of performance on a budget. It is responsive and maneuverable, and offers peace-of-mind when you decide to go off-road.", ListPrice = 769.4900, DiscountPercentage = 25, Weight = 27.13, Color = "Red", ImageUri = new Uri(imageServerPath + "hotrodbike_red_large.jpg") } });
            shoppingCartItems.Add(new ShoppingCartItem { Currency = "USD", Quantity = 1, Product = new Product { Currency = "USD", Title = "Touring Pedal", ProductNumber = "PD-T852", SubcategoryId = 13, Description = "A stable pedal for all-day riding.", ListPrice = 80.9900, Weight = 0, Color = "Silver/Black", ImageUri = new Uri(imageServerPath + "clipless_pedals_large.jpg") } });
            shoppingCartItems.Add(new ShoppingCartItem { Currency = "USD", Quantity = 1, Product = new Product { Currency = "USD", Title = "LL Touring Frame - Yellow, 62", ProductNumber = "FR-T67Y-62", SubcategoryId = 16, Description = "Lightweight butted aluminum frame provides a more upright riding position for a trip around town.  Our ground-breaking design provides optimum comfort.", ListPrice = 333.4200, Weight = 3.20, Color = "Yellow", ImageUri = new Uri(imageServerPath + "touring_frame_yellow_large.jpg") } });
            shoppingCarts.Add("JohnDoe", new ShoppingCart(shoppingCartItems) { Currency = "USD", FullPrice = 1183.90, TotalPrice = 1183.90 });
            return shoppingCarts;
        }

        public ShoppingCart GetById(string shoppingCartId)
        {
            lock (_lock)
            {
                if ( _shoppingCarts.ContainsKey(shoppingCartId))
                {
                    return _shoppingCarts[shoppingCartId];
                }
                var shoppingCart = CreateNewShoppingCart(shoppingCartId);
                _shoppingCarts[shoppingCartId] = shoppingCart;
                return shoppingCart;
            }
        }

        public void AddProductToCart(string shoppingCartId, Product product)
        {
            lock (_lock)
            {
                ShoppingCart shoppingCart = GetById(shoppingCartId);

                ShoppingCartItem item = shoppingCart.ShoppingCartItems.FirstOrDefault(c => c.Product.ProductNumber == product.ProductNumber);

                if (item == null)
                {
                    item = new ShoppingCartItem
                    {
                        Id = product.ProductNumber,
                        Product = product,
                        Quantity = 1,
                        Currency = shoppingCart.Currency
                    };

                    shoppingCart.ShoppingCartItems.Add(item);
                }
                else
                {
                    item.Quantity++;
                }

                UpdatePrices(shoppingCart);
            }
        }

        private static ShoppingCart CreateNewShoppingCart(string shoppingCartId)
        {
            ShoppingCart shoppingCart;
            shoppingCart = new ShoppingCart(new List<ShoppingCartItem>())
                               {
                                   ShoppingCartId = shoppingCartId,
                                   Currency = "USD",
                                   TaxRate = .09
                               };
            return shoppingCart;
        }

        public bool RemoveProductFromCart(string shoppingCartId, string productId)
        {
            lock (_lock)
            {
                ShoppingCart shoppingCart = GetById(shoppingCartId);
                if (shoppingCart == null) return false;

                var shoppingCartItem =
                    shoppingCart.ShoppingCartItems.FirstOrDefault((item) => item.Product.ProductNumber == productId);

                if (shoppingCartItem == null) return false;

                shoppingCartItem.Quantity--;

                return true;
            }
        }

        public bool RemoveItemFromCart(ShoppingCart shoppingCart, string itemId)
        {
            lock (_lock)
            {
                if (shoppingCart == null)
                {
                    throw new ArgumentNullException("shoppingCart");
                }

                ShoppingCartItem item = shoppingCart.ShoppingCartItems.FirstOrDefault(i => i.Id == itemId);
                bool itemRemoved = shoppingCart.ShoppingCartItems.Remove(item);

                if (itemRemoved)
                {
                    UpdatePrices(shoppingCart);
                }

                return itemRemoved;
            }
        }

        public bool Delete(string userId)
        {
            lock (_lock)
            {
                if (_shoppingCarts.ContainsKey(userId))
                {
                    _shoppingCarts.Remove(userId);
                    return true;
                }
                return false;
            }
        }

        private static void UpdatePrices(ShoppingCart shoppingCart)
        {
            double fullPrice = 0, discount = 0;
            foreach (var shoppingCartItem in shoppingCart.ShoppingCartItems)
            {
                fullPrice += shoppingCartItem.Product.ListPrice * shoppingCartItem.Quantity;
                discount += fullPrice * shoppingCartItem.Product.DiscountPercentage/100;
                shoppingCart.FullPrice = fullPrice;
                shoppingCart.TotalDiscount = discount;
                shoppingCart.TotalPrice = fullPrice - discount;
            }
        }

        public static void Reset()
        {
            lock (_lock)
            {
                _shoppingCarts = GetDefaultShoppingCarts();
            }
        }
    }
}