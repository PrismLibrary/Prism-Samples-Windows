

using System.Linq;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace AdventureWorks.WebServices.Tests.Repositories
{
    [TestClass]
    public class ShoppingCartRepositoryFixture
    {
        [TestInitialize]
        public void Initialize()
        {
            ShoppingCartRepository.Reset();
        }

        [TestMethod]
        public void AddProductToCart_AddsNewShoppingCartItem()
        {
            var target = new ShoppingCartRepository();
            var preAddcart = target.GetById("TestUser");
            Assert.AreEqual(0, preAddcart.ShoppingCartItems.Count);

            target.AddProductToCart("TestUser", new Product() { ProductNumber ="BB-7421"});

            var postAddcart = target.GetById("TestUser");
            Assert.IsNotNull(postAddcart);
            Assert.AreEqual(1, postAddcart.ShoppingCartItems.Count);
            Assert.AreEqual("BB-7421", postAddcart.ShoppingCartItems.First().Product.ProductNumber);
        }

        [TestMethod]
        public void AddProductToCart_AddsNewShoppingCartItemToExistingCart()
        {
            var target = new ShoppingCartRepository();
            target.AddProductToCart("TestUser", new Product() { ProductNumber ="BB-7421"});
            target.AddProductToCart("TestUser", new Product() { ProductNumber = "BB-8107" });
            var cart = target.GetById("TestUser");
            Assert.IsNotNull(cart);
            Assert.AreEqual(2, cart.ShoppingCartItems.Count);
            Assert.IsNotNull(cart.ShoppingCartItems.First(item => item.Product.ProductNumber == "BB-7421"));
            Assert.IsNotNull(cart.ShoppingCartItems.First(item => item.Product.ProductNumber == "BB-8107"));
        }

        [TestMethod]
        public void AddProductToCart_AddsNewShoppingCartItemToExistingCart_WithSameProduct()
        {
            var target = new ShoppingCartRepository();
            
            target.AddProductToCart("TestUser", new Product { ProductNumber = "123" });
            target.AddProductToCart("TestUser", new Product { ProductNumber = "123" });

            var cart = target.GetById("TestUser");
            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.ShoppingCartItems.Count);

            var items = cart.ShoppingCartItems.Where(item => item.Product.ProductNumber == "123");
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(2, items.First().Quantity);
        }

        [TestMethod]
        public void DeleteCart_DeletesCart_AndReturnsTrue()
        {
            var target = new ShoppingCartRepository();
            target.AddProductToCart("TestUser", new Product { ProductNumber = "BB-7421" });

            var cart = target.GetById("TestUser");
            Assert.AreEqual(1, cart.ShoppingCartItems.Count);

            var success = target.Delete("TestUser");

            Assert.IsTrue(success);

            var emptyCart = target.GetById("TestUser");
            Assert.AreEqual(0, emptyCart.ShoppingCartItems.Count);
        }

        [TestMethod]
        public void DeleteCart_ReturnsFalse_WhenCartDoesNotExist()
        {
            var target = new ShoppingCartRepository();

            var success = target.Delete("TestUser");
            Assert.IsFalse(success);
        }
    }
}
