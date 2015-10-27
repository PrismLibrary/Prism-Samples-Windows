// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AdventureWorks.UILogic.Tests.Repositories
{
    [TestClass]
    public class ShoppingCartRepositoryFixture
    {
        [TestMethod]
        public void CartItemUpdatedEventRaised_WhenProductAdded()
        {
            var shoppingCartItemUpdatedRaised = false;
            var shoppingCartService = new MockShoppingCartService();
            shoppingCartService.AddProductToShoppingCartAsyncDelegate =
                (s, s1) => Task.FromResult(string.Empty);
            var shoppingCartItemUpdatedEvent = new ShoppingCartItemUpdatedEvent();
            shoppingCartItemUpdatedEvent.Subscribe((_) =>
                                                   {
                                                       shoppingCartItemUpdatedRaised = true;
                                                   });
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => shoppingCartItemUpdatedEvent;
            var target = new ShoppingCartRepository(shoppingCartService, new MockAccountService(), eventAggregator, new MockSessionStateService());

            target.AddProductToShoppingCartAsync("TestProductId");

            Assert.IsTrue(shoppingCartItemUpdatedRaised);
        }

        [TestMethod]
        public void CartItemUpdatedEventRaised_WhenProductRemoved()
        {
            var shoppingCartItemUpdatedRaised = false;
            var shoppingCartService = new MockShoppingCartService();
            shoppingCartService.RemoveProductFromShoppingCartAsyncDelegate =
                (s, s1) => Task.FromResult(string.Empty);
            var shoppingCartItemUpdatedEvent = new ShoppingCartItemUpdatedEvent();
            shoppingCartItemUpdatedEvent.Subscribe((_) =>
            {
                shoppingCartItemUpdatedRaised = true;
            });
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => shoppingCartItemUpdatedEvent;
            var target = new ShoppingCartRepository(shoppingCartService, new MockAccountService(), eventAggregator, new MockSessionStateService());

            target.RemoveProductFromShoppingCartAsync("TestProductId");

            Assert.IsTrue(shoppingCartItemUpdatedRaised);
        }

        [TestMethod]
        public async Task CartItemUpdatedEventRaised_WhenItemRemoved()
        {
            var shoppingCartItemUpdatedRaised = false;
            var shoppingCartService = new MockShoppingCartService()
                {
                    RemoveShoppingCartItemDelegate = (s, s1) =>
                        {
                            Assert.AreEqual("TestShoppingCartItemId", s1);
                            return Task.FromResult(string.Empty);
                        }
                };
            var shoppingCartItemUpdatedEvent = new ShoppingCartItemUpdatedEvent();
            shoppingCartItemUpdatedEvent.Subscribe((a) => shoppingCartItemUpdatedRaised = true);

            var eventAggregator = new MockEventAggregator()
                {
                    GetEventDelegate = (a) => shoppingCartItemUpdatedEvent
                };

            var target = new ShoppingCartRepository(shoppingCartService, new MockAccountService(), eventAggregator, new MockSessionStateService());
            await target.RemoveShoppingCartItemAsync("TestShoppingCartItemId");

            Assert.IsTrue(shoppingCartItemUpdatedRaised);
        }

        [TestMethod]
        public void CartUpdatedEventRaised_WhenUserChanged()
        {
            var shoppingCartUpdatedRaised = false;
            var accountService = new MockAccountService();
            var shoppingCartUpdatedEvent = new MockShoppingCartUpdatedEvent()
                {
                    PublishDelegate = () => shoppingCartUpdatedRaised = true
                };
            var eventAggregator = new MockEventAggregator()
                {
                    GetEventDelegate = (a) => shoppingCartUpdatedEvent
                };
            var shoppingCartService = new MockShoppingCartService()
                {
                    MergeShoppingCartsAsyncDelegate = (s, s1) => Task.FromResult(false)
                };

            var target = new ShoppingCartRepository(shoppingCartService, accountService, eventAggregator, new MockSessionStateService());
            accountService.RaiseUserChanged(new UserInfo { UserName = "TestUserName" }, null);

            Assert.IsTrue(shoppingCartUpdatedRaised);
        }

        [TestMethod]
        public void ShoppingCartMerged_WhenAnonymousUserLogsIn()
        {
            bool mergeShoppingCartsCalled = false;
            bool alertMessageServiceCalled = false;
            var anonymousCartItems = new List<ShoppingCartItem>
                                         {
                                             new ShoppingCartItem
                                                 {Quantity = 1, Product = new Product {ProductNumber = "123"}}
                                         };
            var testUserCartItems = new List<ShoppingCartItem>
                                         {
                                             new ShoppingCartItem
                                                 {Quantity = 2, Product = new Product {ProductNumber = "123"}}
                                         };

            var shoppingCartService = new MockShoppingCartService()
                {
                    GetShoppingCartAsyncDelegate = s =>
                        {
                            switch (s)
                            {
                                case "AnonymousId": 
                                    return Task.FromResult(new ShoppingCart(anonymousCartItems));
                                default:
                                    return Task.FromResult(new ShoppingCart(testUserCartItems));
                            }
                        },
                    MergeShoppingCartsAsyncDelegate = (s, s1) =>
                        {
                            mergeShoppingCartsCalled = true;
                            Assert.AreEqual("AnonymousId", s);
                            Assert.AreEqual("TestUserName", s1);
                            return Task.FromResult(true);
                        }
                };
            var accountService = new MockAccountService();
            var shoppingCartUpdatedEvent = new MockShoppingCartUpdatedEvent
                {
                    PublishDelegate = () => { }
                };
            
            var eventAggregator = new MockEventAggregator()
                {
                    GetEventDelegate = (a) => shoppingCartUpdatedEvent
                };
            var sessionStateService = new MockSessionStateService();
            sessionStateService.SessionState[ShoppingCartRepository.ShoppingCartIdKey] = "AnonymousId";

            var target = new ShoppingCartRepository(shoppingCartService, accountService, eventAggregator, sessionStateService);
            accountService.RaiseUserChanged(new UserInfo { UserName = "TestUserName" }, null);

            Assert.IsTrue(mergeShoppingCartsCalled);
        }

        [TestMethod]
        public async Task GetShoppingCartAsync_CachesCart()
        {

            var shoppingCart = new ShoppingCart(new Collection<ShoppingCartItem>());
            var shoppingCartService = new MockShoppingCartService()
                {
                    GetShoppingCartAsyncDelegate = s => Task.FromResult(shoppingCart)
                };

            var target = new ShoppingCartRepository(shoppingCartService, null, null, new MockSessionStateService());
            var firstCartReturned = await target.GetShoppingCartAsync();
            shoppingCartService.GetShoppingCartAsyncDelegate = s =>
            {
                Assert.Fail("Should not have called proxy second time.");
                return Task.FromResult((ShoppingCart)null);
            };

            var secondCartReturned = await target.GetShoppingCartAsync();
            Assert.AreSame(shoppingCart, firstCartReturned);
            Assert.AreSame(shoppingCart, secondCartReturned);
        }

        [TestMethod]
        public async Task Add_InvalidatesCachedCart()
        {
            var shoppingCartService = new MockShoppingCartService
                {
                    AddProductToShoppingCartAsyncDelegate = (s, s1) => Task.FromResult(new ShoppingCartItem()),
                    GetShoppingCartAsyncDelegate = s => Task.FromResult(new ShoppingCart(new Collection<ShoppingCartItem>()) {Id = "first"})
                };
            var eventAggregator = new MockEventAggregator
                {
                    GetEventDelegate = type => new ShoppingCartItemUpdatedEvent()
                };

            var target = new ShoppingCartRepository(shoppingCartService, null, eventAggregator, new MockSessionStateService());
            var firstCartReturned = await target.GetShoppingCartAsync();

            await target.AddProductToShoppingCartAsync("TestProductId");

            shoppingCartService.GetShoppingCartAsyncDelegate = s => Task.FromResult(new ShoppingCart(new Collection<ShoppingCartItem>()) { Id = "second" });
            var secondCartReturned = await target.GetShoppingCartAsync();

            Assert.IsNotNull(firstCartReturned);
            Assert.IsNotNull(secondCartReturned);
            Assert.AreNotSame(firstCartReturned, secondCartReturned);
        }

        [TestMethod]
        public async Task Remove_InvalidatesCachedCart()
        {
            var shoppingCartService = new MockShoppingCartService
                {
                    RemoveShoppingCartItemDelegate = (s, s1) => Task.FromResult(string.Empty),
                    GetShoppingCartAsyncDelegate = s => Task.FromResult(new ShoppingCart(new Collection<ShoppingCartItem>()) {Id = "first"})
                };
            var eventAggregator = new MockEventAggregator
                {
                    GetEventDelegate = type => new ShoppingCartItemUpdatedEvent()
                };
            var target = new ShoppingCartRepository(shoppingCartService, null, eventAggregator, new MockSessionStateService());
            var firstCartReturned = await target.GetShoppingCartAsync();

            await target.RemoveShoppingCartItemAsync("TestShoppingCartItemId");

            shoppingCartService.GetShoppingCartAsyncDelegate = s => Task.FromResult(new ShoppingCart(new Collection<ShoppingCartItem>()) { Id = "second" });
            var secondCartReturned = await target.GetShoppingCartAsync();

            Assert.IsNotNull(firstCartReturned);
            Assert.IsNotNull(secondCartReturned);
            Assert.AreNotSame(firstCartReturned, secondCartReturned);
        }
        //[TestMethod]
        //public void GetShoppingCartAsync_Offline_Gets_Valid_Cart()
        //{
        //    MockShoppingCartService shoppingCartService = new MockShoppingCartService
        //    {
        //        GetShoppingCartAsyncDelegate = (user) => { throw new Exception(); }
        //    };
        //    IShoppingCartRepository repository = new ShoppingCartRepository(shoppingCartService);
        //    var cart = repository.GetShoppingCartAsync().Result;
        //    Assert.IsNotNull(cart);
        //}

        //[TestMethod]
        //public void GetShoppingCartAsync_Online_Gets_Cart_From_Server()
        //{
        //}

        //[TestMethod]
        //public void AddProductToShoppingCartAsync_Offline_Adds_To_Client_Side_Cart()
        //{
        //}

        //[TestMethod]
        //public void AddProductToShoppingCartAsync_Online_Not_Logged_In_Adds_To_Server_Side_Temp_Cart()
        //{
        //}

        //[TestMethod]
        //public void AddProductToShoppingCartAsync_Online_Logged_In_Adds_To_Server_Side_User_Cart()
        //{
        //}

        //[TestMethod]
        //public void RemoveShoppingCartItemAsync_Offline_Adds_To_Client_Side_Cart()
        //{
        //}

        //[TestMethod]
        //public void RemoveShoppingCartItemAsync_Online_Not_Logged_In_Adds_To_Server_Side_Temp_Cart()
        //{
        //}

        //[TestMethod]
        //public void RemoveShoppingCartItemAsync_Online_Logged_In_Adds_To_Server_Side_User_Cart()
        //{
        //}
    }
}
