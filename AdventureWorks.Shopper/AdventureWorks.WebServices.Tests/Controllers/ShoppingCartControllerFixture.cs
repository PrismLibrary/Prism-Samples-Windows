

using System.Collections.ObjectModel;
using System.Web.Http;
using AdventureWorks.WebServices.Controllers;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventureWorks.WebServices.Tests.Controllers
{
    [TestClass]
    public class ShoppingCartControllerFixture
    {
        [TestMethod]
        public void Get_GetsShoppingCartForUser()
        {
            var shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>()) { FullPrice = 200, TotalDiscount = 100 };
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetByIdDelegate = (userId) =>
                                         {
                                             return shoppingCart;
                                         };

            var target = new ShoppingCartController(shoppingCartRepository, new MockProductRepository());
            var result = target.Get("JohnDoe");
            Assert.AreEqual(result, shoppingCart);
        }

        [TestMethod]
        public void DeleteShoppingCart_ShoppingCartItem_For_An_User()
        {
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.DeleteDelegate = (userId) =>
            {
                Assert.AreEqual("JohnDoe", userId);
                return true;
            };

            var target = new ShoppingCartController(shoppingCartRepository, new MockProductRepository());
            target.DeleteShoppingCart("JohnDoe");
        }

        [TestMethod]
        public void DeleteShoppingCart_Throws_ForUnknownUser()
        {
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.DeleteDelegate = s => false;

            HttpResponseException caughtException = null;
            var target = new ShoppingCartController(shoppingCartRepository, new MockProductRepository());
            try
            {
                target.DeleteShoppingCart("UnknownUser");
            }
            catch (HttpResponseException ex)
            {
                caughtException = ex;
            }
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, caughtException.Response.StatusCode);
        }

        [TestMethod]
        public void RemoveShoppingCartItem_Throws_ForUnknownItem()
        {
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetByIdDelegate = (userId) =>
                                                         {
                                                             return new ShoppingCart(new Collection<ShoppingCartItem>());
                                                         };

            shoppingCartRepository.RemoveItemFromCartDelegate = (shoppingCart, itemId) =>
            {
                return false;
            };

            var target = new ShoppingCartController(shoppingCartRepository, new MockProductRepository());
            try
            {
                target.RemoveShoppingCartItem("JohnDoe", "UnknownProductid");
            }
            catch (HttpResponseException ex)
            {
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, ex.Response.StatusCode);
            }
        }

        [TestMethod]
        public void MergeShoppingCarts_Merges()
        {
            var addProductToCartCalled = false;
            var deleteCartCalled = false;
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetByIdDelegate = userId =>
                                                         {
                                                             switch (userId)
                                                             {
                                                                 case "newId":
                                                                     return
                                                                         new ShoppingCart(
                                                                             new Collection<ShoppingCartItem>
                                                                                 {new ShoppingCartItem {Id = "item1", Product = new Product()}});
                                                                 case "oldId":
                                                                     return
                                                                         new ShoppingCart(
                                                                             new Collection<ShoppingCartItem>
                                                                                 {new ShoppingCartItem {Id = "item2", Quantity = 1, Product = new Product{ProductNumber = "product1"}}});
                                                                 default:
                                                                     return null;
                                                             }
                                                         };
            shoppingCartRepository.AddProductToCartDelegate = (shoppingCartId, product) =>
                                                                  {
                                                                      Assert.AreEqual("newId", shoppingCartId);
                                                                      Assert.AreEqual("product1", product.ProductNumber);
                                                                      addProductToCartCalled = true;
                                                                  };
            shoppingCartRepository.DeleteDelegate = shoppingCartId =>
                                                        {
                                                            Assert.AreEqual("oldId", shoppingCartId);
                                                            deleteCartCalled = true;
                                                            return true;
                                                        };
            var target = new ShoppingCartController(shoppingCartRepository, new MockProductRepository());

            Assert.IsTrue(target.MergeShoppingCarts("newId", "oldId"));
            Assert.IsTrue(addProductToCartCalled);
            Assert.IsTrue(deleteCartCalled);
        }
        [TestMethod]
        public void MergeShoppingCarts_ReturnsFalse_WhenNothingToMerge()
        {
            var deleteCartCalled = false;
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetByIdDelegate = userId =>
                                                         {
                                                             switch (userId)
                                                             {
                                                                 case "oldId":
                                                                     return
                                                                         new ShoppingCart(
                                                                             new Collection<ShoppingCartItem>
                                                                                 {new ShoppingCartItem {Id = "item1", Product = new Product()}});
                                                                 case "newId":
                                                                     return
                                                                         new ShoppingCart(
                                                                             new Collection<ShoppingCartItem>());
                                                                 default:
                                                                     return null;
                                                             }
                                                         };
            shoppingCartRepository.DeleteDelegate = shoppingCartId =>
            {
                Assert.AreEqual("oldId", shoppingCartId);
                deleteCartCalled = true;
                return true;
            };
            var target = new ShoppingCartController(shoppingCartRepository, new MockProductRepository());

            Assert.IsFalse(target.MergeShoppingCarts("newId", "oldId"));
            Assert.IsTrue(deleteCartCalled);

        }
    }
}
