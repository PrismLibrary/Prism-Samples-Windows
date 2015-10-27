// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ShoppingCartTabUserControlViewModelFixture
    {
        [TestMethod]
        public void NavigatedTo_CalculatesTotalNumberOfItemsInCart()
        {
            var shoppingCart = new ShoppingCart(new List<ShoppingCartItem>()
                                               {
                                                   new ShoppingCartItem {Quantity = 1},
                                                   new ShoppingCartItem {Quantity = 2}
                                               });
            var shoppingCartRepository = new MockShoppingCartRepository();
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type =>
            {
                if (type == typeof(ShoppingCartUpdatedEvent)) return new ShoppingCartUpdatedEvent();
                if (type == typeof(ShoppingCartItemUpdatedEvent)) return new ShoppingCartItemUpdatedEvent();
                return null;
            };
            var accountService = new MockAccountService();
            accountService.VerifyUserAuthenticationAsyncDelegate = () => Task.FromResult((UserInfo)null);
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(shoppingCart);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, eventAggregator, null, null, accountService);

            Assert.AreEqual(3, target.ItemCount);
        }

        [TestMethod]
        public void VMListensToShoppingCartUpdatedEvent_ThenCalculatesTotalNumberOfItemsInCart()
        {
            var shoppingCart = new ShoppingCart(new List<ShoppingCartItem>());
            var shoppingCartRepository = new MockShoppingCartRepository();

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(shoppingCart);
            var shoppingCartUpdatedEvent = new ShoppingCartUpdatedEvent();
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type =>
            {
                if (type == typeof(ShoppingCartUpdatedEvent)) return shoppingCartUpdatedEvent;
                if (type == typeof(ShoppingCartItemUpdatedEvent)) return new ShoppingCartItemUpdatedEvent();
                return null;
            };
            var accountService = new MockAccountService();
            accountService.VerifyUserAuthenticationAsyncDelegate = () => Task.FromResult((UserInfo)null);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, eventAggregator, null, null, accountService);

            shoppingCart = new ShoppingCart(new List<ShoppingCartItem>()
                                               {
                                                   new ShoppingCartItem {Quantity = 1},
                                                   new ShoppingCartItem {Quantity = 2}
                                               });

            Assert.AreEqual(0, target.ItemCount);

            shoppingCartUpdatedEvent.Publish(null);

            Assert.AreEqual(3, target.ItemCount);

        }

        [TestMethod]
        public void ShoppingCartUpdated_WithNullCart_SetsItemCountZero()
        {
            var shoppingCartRepository = new MockShoppingCartRepository();
            var shoppingCart = new ShoppingCart(new List<ShoppingCartItem> {new ShoppingCartItem {Quantity = 99}});
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(shoppingCart);
            var eventAggregator = new MockEventAggregator();
            var shoppingCartUpdatedEvent = new ShoppingCartUpdatedEvent();
            eventAggregator.GetEventDelegate = type =>
            {
                if (type == typeof(ShoppingCartUpdatedEvent)) return shoppingCartUpdatedEvent;
                if (type == typeof(ShoppingCartItemUpdatedEvent)) return new ShoppingCartItemUpdatedEvent();
                return null;
            };
            var accountService = new MockAccountService();
            accountService.VerifyUserAuthenticationAsyncDelegate = () => Task.FromResult((UserInfo)null);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, eventAggregator, null, null, accountService);

            shoppingCartUpdatedEvent.Publish(null);

            Assert.AreEqual(99, target.ItemCount);

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult<ShoppingCart>(null);
            shoppingCartUpdatedEvent.Publish(null);

            Assert.AreEqual(0, target.ItemCount);
        }

        [TestMethod]
        public void FailedCallToShoppingCartRepository_ShowsAlert()
        {
            var alertCalled = false;
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
                                                                      {
                                                                          throw new Exception();
                                                                      };
            var alertMessageService = new MockAlertMessageService();
            alertMessageService.ShowAsyncDelegate = (s, s1) =>
                                                        {
                                                            alertCalled = true;
                                                            Assert.AreEqual("ErrorServiceUnreachable", s1);
                                                            return Task.FromResult(string.Empty);
                                                        };
            var accountService = new MockAccountService();
            accountService.VerifyUserAuthenticationAsyncDelegate = () => Task.FromResult((UserInfo)null);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, null, alertMessageService, new MockResourceLoader(), accountService);

            Assert.IsTrue(alertCalled);
        }
    }
}
