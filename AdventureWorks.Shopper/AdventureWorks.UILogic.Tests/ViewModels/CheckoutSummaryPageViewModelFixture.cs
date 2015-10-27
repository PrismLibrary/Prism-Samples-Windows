// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;
using Prism.Windows.Navigation;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class CheckoutSummaryPageViewModelFixture
    {
        [TestMethod]
        public async Task SubmitValidOrder_NavigatesToOrderConfirmation()
        {
            bool navigateCalled = false;
            bool clearCartCalled = false;
            var navigationService = new MockNavigationService();
            navigationService.NavigateDelegate = (s, o) =>
                                                     {
                                                         Assert.AreEqual("OrderConfirmation", s);
                                                         navigateCalled = true;
                                                         return true;
                                                     };
            
            var accountService = new MockAccountService()
                {
                    VerifySavedCredentialsAsyncDelegate = () => Task.FromResult<UserInfo>(new UserInfo())
                };
            var orderService = new MockOrderService()
                {
                    // the order is valid, it can be processed
                    ProcessOrderAsyncDelegate = (o) => Task.FromResult(true)
                };
            var resourcesService = new MockResourceLoader()
                {
                    GetStringDelegate = (key) => key
                };
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.ClearCartAsyncDelegate = () =>
                                                                {
                                                                    clearCartCalled = true;
                                                                    return Task.Delay(0);
                                                                };
            var target = new CheckoutSummaryPageViewModel(navigationService, orderService, null, null, null, shoppingCartRepository, accountService, resourcesService, null, null);
            await target.SubmitCommand.Execute();

            Assert.IsTrue(navigateCalled);
            Assert.IsTrue(clearCartCalled);
        }

        [TestMethod]
        public async Task SubmitInvalidOrder_CallsErrorDialog()
        {
            bool successDialogCalled = false;
            bool errorDialogCalled = false;
            var navigationService = new MockNavigationService();
            var accountService = new MockAccountService()
                {
                    VerifySavedCredentialsAsyncDelegate = () => Task.FromResult<UserInfo>(new UserInfo())
                };
            var orderService = new MockOrderService()
                {
                    // the order is invalid, it cannot be processed
                    ProcessOrderAsyncDelegate = (o) =>
                        {
                            var modelValidationResult = new ModelValidationResult();
                            modelValidationResult.ModelState.Add("someKey", new List<string>() { "the value of someKey is invalid" });
                            throw new ModelValidationException(modelValidationResult);
                        }
                };
            var resourcesService = new MockResourceLoader()
                {
                    GetStringDelegate = (key) => key
                };
            var alertService = new MockAlertMessageService()
                {
                    ShowAsyncDelegate = (dialogTitle, dialogMessage) =>
        {
                        successDialogCalled = dialogTitle.ToLower().Contains("purchased");
                        errorDialogCalled = !successDialogCalled;
                        return Task.FromResult(successDialogCalled);
                    }
                };

            var target = new CheckoutSummaryPageViewModel(navigationService, orderService, null, null, null, null, accountService, resourcesService, alertService, null);
            await target.SubmitCommand.Execute();

            Assert.IsFalse(successDialogCalled);
            Assert.IsTrue(errorDialogCalled);
        }

        [TestMethod]
        public async Task Submit_WhenAnonymous_ShowsSignInControl()
        {
            bool showSignInCalled = false;
            var accountService = new MockAccountService()
                {
                    VerifySavedCredentialsAsyncDelegate = () => Task.FromResult<UserInfo>(null)
                };
            var signInUserControlViewModel = new MockSignInUserControlViewModel()
                                                 {
                                                     OpenDelegate = (a) => showSignInCalled = true
                                                 };
            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), null, null, null, null, null, accountService, null, null, signInUserControlViewModel);
            await target.SubmitCommand.Execute();

            Assert.IsTrue(showSignInCalled);
        }

        [TestMethod]
        public void SelectShippingMethod_Recalculates_Order()
        {
            var shippingMethods = new List<ShippingMethod>() { new ShippingMethod() { Id = 1, Cost = 0 } };
            var shoppingCartItems = new List<ShoppingCartItem>() { new ShoppingCartItem() { Quantity = 1, Currency = "USD", Product = new Product() } };
            var order = new Order()
            {
                ShoppingCart = new ShoppingCart(shoppingCartItems) { Currency = "USD", TotalPrice = 100 },
                ShippingAddress = new Address(),
                BillingAddress = new Address(),
                PaymentMethod = new PaymentMethod() { CardNumber = "1234" },
                ShippingMethod = shippingMethods.First()
            };

            var shippingMethodService = new MockShippingMethodService() 
            {
                GetShippingMethodsAsyncDelegate = () => Task.FromResult<IEnumerable<ShippingMethod>>(shippingMethods) 
            };
            var orderRepository = new MockOrderRepository() { CurrentOrder =  order };
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(order.ShoppingCart);
            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetShippingAddressAsyncDelegate = s => Task.FromResult(new Address());
            checkoutDataRepository.GetBillingAddressAsyncDelegate = s => Task.FromResult(new Address());
            checkoutDataRepository.GetPaymentMethodDelegate = s => Task.FromResult(new PaymentMethod{CardNumber = "1234"});

            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), new MockOrderService(), orderRepository, shippingMethodService,
                                                          checkoutDataRepository, shoppingCartRepository,
                                                          new MockAccountService(), new MockResourceLoader(), null, null);

            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = null, NavigationMode = NavigationMode.New }, null);

            Assert.AreEqual("$0.00", target.ShippingCost);
            Assert.AreEqual("$100.00", target.OrderSubtotal);
            Assert.AreEqual("$100.00", target.GrandTotal);

            target.SelectedShippingMethod = new ShippingMethod() { Cost = 10 };

            Assert.AreEqual("$10.00", target.ShippingCost);
            Assert.AreEqual("$100.00", target.OrderSubtotal);
            Assert.AreEqual("$110.00", target.GrandTotal);

        }

        [TestMethod]
        public void SelectCheckoutData_Opens_AppBar()
        {
            var shippingMethods = new List<ShippingMethod>() { new ShippingMethod() { Id = 1, Cost = 0 } };
            var shoppingCartItems = new List<ShoppingCartItem>() { new ShoppingCartItem() { Quantity = 1, Currency = "USD", Product = new Product() } };
            var order = new Order()
            {
                ShoppingCart = new ShoppingCart(shoppingCartItems) { Currency = "USD", FullPrice = 100 },
                ShippingAddress = new Address(),
                BillingAddress = new Address(),
                PaymentMethod = new PaymentMethod() { CardNumber = "1234" },
                ShippingMethod = shippingMethods.First()
            };
            var shippingMethodService = new MockShippingMethodService()
            {
                GetShippingMethodsAsyncDelegate = () => Task.FromResult<IEnumerable<ShippingMethod>>(shippingMethods)
            };
            var orderRepository = new MockOrderRepository() { CurrentOrder = order};
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(order.ShoppingCart);
            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetShippingAddressAsyncDelegate = s => Task.FromResult(new Address());
            checkoutDataRepository.GetBillingAddressAsyncDelegate = s => Task.FromResult(new Address());
            checkoutDataRepository.GetPaymentMethodDelegate = s => Task.FromResult(new PaymentMethod { CardNumber = "1234" });
            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), new MockOrderService(), orderRepository, shippingMethodService,
                                                          checkoutDataRepository, shoppingCartRepository,
                                                          new MockAccountService(), new MockResourceLoader(), null, null);

            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = null, NavigationMode = NavigationMode.New }, null);
            Assert.IsFalse(target.IsBottomAppBarOpened);

            target.SelectedCheckoutData = target.CheckoutDataViewModels.First();
            Assert.IsTrue(target.IsBottomAppBarOpened);
        }

        [TestMethod]
        public void EditCheckoutData_NavigatesToProperPage()
        {
            string requestedPageName = string.Empty;
            var navigationService = new MockNavigationService() 
                { 
                    NavigateDelegate = (pageName, navParameter) =>
                                           {
                                               Assert.IsTrue(pageName == requestedPageName);
                                               return true;
                                           }
                };

            var target = new CheckoutSummaryPageViewModel(navigationService, null, null, null, null, null, null, null, null, null);

            requestedPageName = "ShippingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel() { DataType = Constants.ShippingAddress };
            target.EditCheckoutDataCommand.Execute().Wait();

            requestedPageName = "BillingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel { DataType = Constants.BillingAddress };
            target.EditCheckoutDataCommand.Execute().Wait();

            requestedPageName = "PaymentMethod";
            target.SelectedCheckoutData = new CheckoutDataViewModel { DataType = Constants.PaymentMethod };
            target.EditCheckoutDataCommand.Execute().Wait();
        }

        [TestMethod]
        public void AddCheckoutData_NavigatesToProperPage()
        {
            string requestedPageName = string.Empty;
            var navigationService= new MockNavigationService()
            {
                NavigateDelegate = (pageName, navParam) =>
                {
                    Assert.IsTrue(pageName == requestedPageName);
                    return true;
                }
            };

            var target = new CheckoutSummaryPageViewModel(navigationService, null, null, null, null, null, null, null, null, null);

            requestedPageName = "ShippingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel() { DataType = Constants.ShippingAddress };
            target.AddCheckoutDataCommand.Execute().Wait();

            requestedPageName = "BillingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel() { DataType = Constants.BillingAddress };
            target.AddCheckoutDataCommand.Execute().Wait();

            requestedPageName = "PaymentMethod";
            target.SelectedCheckoutData = new CheckoutDataViewModel() { DataType = Constants.PaymentMethod };
            target.AddCheckoutDataCommand.Execute().Wait();
        }

        [TestMethod]
        public void NavigatingToWhenNoShippingMethodSelected_RecalculatesOrder()
        {
            var shippingMethods = new List<ShippingMethod> { new ShippingMethod { Id = 1, Cost = 0 } };
            var shoppingCartItems = new List<ShoppingCartItem> { new ShoppingCartItem { Quantity = 1, Currency = "USD", Product = new Product() } };
            var order = new Order
                {
                ShoppingCart = new ShoppingCart(shoppingCartItems) { Currency = "USD", TotalPrice = 100 },
                ShippingAddress = new Address { Id = "1"},
                BillingAddress = new Address { Id = "1" },
                PaymentMethod = new PaymentMethod() { CardNumber = "1234" },
                ShippingMethod = null
            };
            var shippingMethodService = new MockShippingMethodService
            {
                GetShippingMethodsAsyncDelegate = () => Task.FromResult<IEnumerable<ShippingMethod>>(shippingMethods)
            };
            var orderRepository = new MockOrderRepository { CurrentOrder = order };
            var shoppingCartRepository = new MockShoppingCartRepository
                {
                    GetShoppingCartAsyncDelegate = () => Task.FromResult(order.ShoppingCart)
                };
            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetShippingAddressAsyncDelegate = s => Task.FromResult(new Address());
            checkoutDataRepository.GetBillingAddressAsyncDelegate = s => Task.FromResult(new Address());
            checkoutDataRepository.GetPaymentMethodDelegate = s => Task.FromResult(new PaymentMethod { CardNumber = "1234" });
            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), new MockOrderService(), orderRepository, shippingMethodService,
                                                          checkoutDataRepository, shoppingCartRepository,
                                                          new MockAccountService(), new MockResourceLoader(), null, null);

            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = null, NavigationMode = NavigationMode.New }, null);

            Assert.AreEqual("$0.00", target.ShippingCost);
            Assert.AreEqual("$100.00", target.OrderSubtotal);
            Assert.AreEqual("$100.00", target.GrandTotal);
        }
    }
}
