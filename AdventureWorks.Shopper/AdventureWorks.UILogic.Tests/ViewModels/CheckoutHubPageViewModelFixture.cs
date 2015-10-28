

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class CheckoutHubPageViewModelFixture
    {
        [TestMethod]
        public async Task ExecuteGoNextCommand_Validates3ViewModels()
        {
            bool shippingValidationExecuted = false;
            bool billingValidationExecuted = false;
            bool paymentValidationExecuted = false;
            var shippingAddressPageViewModel = new MockShippingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => { shippingValidationExecuted = true; return false; }
                };
            var billingAddressPageViewModel = new MockBillingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => { billingValidationExecuted = true; return false; }
                };
            var paymentMethodPageViewModel = new MockPaymentMethodPageViewModel()
                {
                    ValidateFormDelegate = () => { paymentValidationExecuted = true; return false; }
                };

            var target = new CheckoutHubPageViewModel(new MockNavigationService(), null, null, new MockShoppingCartRepository(),
                                                        shippingAddressPageViewModel, billingAddressPageViewModel, paymentMethodPageViewModel, null, null);
            await target.GoNextCommand.Execute();

            Assert.IsTrue(shippingValidationExecuted);
            Assert.IsTrue(billingValidationExecuted);
            Assert.IsTrue(paymentValidationExecuted);
        }

        [TestMethod]
        public async Task ExecuteGoNextCommand_ProcessesFormsAndNavigates_IfViewModelsAreValid()
        {
            bool shippingInfoProcessed = false;
            bool billingInfoProcessed = false;
            bool paymentInfoProcessed = false;
            bool navigated = false;
            var shippingAddressPageViewModel = new MockShippingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormAsyncDelegate = () =>
                                                   {
                                                       shippingInfoProcessed = true;
                                                       return Task.Delay(0);
                                                   }
                };
            var billingAddressPageViewModel = new MockBillingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormAsyncDelegate = () =>
                                                   {
                                                       billingInfoProcessed = true;
                                                       return Task.Delay(0);
                                                   }
                };
            var paymentMethodPageViewModel = new MockPaymentMethodPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormAsyncDelegate = async () => 
                        {
                            paymentInfoProcessed = true;
                            await Task.Delay(0);
                        }
                };
            var accountService = new MockAccountService()
                {
                    VerifyUserAuthenticationAsyncDelegate = () => Task.FromResult(new UserInfo()),
                    SignedInUser = new UserInfo() { UserName = "test" }
                };
            var orderRepository = new MockOrderRepository()
                {
                    CreateBasicOrderAsyncDelegate = (a, b, c, d, e) => Task.FromResult(new Order() { Id = 1 })
                };
            var shoppingCartRepository = new MockShoppingCartRepository()
                {
                    GetShoppingCartAsyncDelegate = () => Task.FromResult(new ShoppingCart(null))
                };
            var navigationService = new MockNavigationService()
                {
                    NavigateDelegate = (a, b) => navigated = true
                };

            var target = new CheckoutHubPageViewModel(navigationService, accountService, orderRepository, shoppingCartRepository,
                                            shippingAddressPageViewModel, billingAddressPageViewModel, paymentMethodPageViewModel, null, null);
            await target.GoNextCommand.Execute();

            Assert.IsTrue(shippingInfoProcessed);
            Assert.IsTrue(billingInfoProcessed);
            Assert.IsTrue(paymentInfoProcessed);
            Assert.IsTrue(navigated);
        }

        [TestMethod]
        public async Task ExecuteGoNextCommand_DoNothing_IfViewModelsAreInvalid()
        {
            bool formProcessStarted = false;
            var accountService = new MockAccountService()
            {
                VerifyUserAuthenticationAsyncDelegate = () =>
                    {
                        // The process starts with a call to retrieve the logged user
                        formProcessStarted = true;
                        return Task.FromResult(new UserInfo());
                    }
            };
            var shippingAddressPageViewModel = new MockShippingAddressPageViewModel();
            var billingAddressPageViewModel = new MockBillingAddressPageViewModel();
            var paymentMethodPageViewModel = new MockPaymentMethodPageViewModel();
            var target = new CheckoutHubPageViewModel(new MockNavigationService(), accountService, null, null,
                                                       shippingAddressPageViewModel, billingAddressPageViewModel, paymentMethodPageViewModel, null, null);

            // ShippingAddress invalid only
            shippingAddressPageViewModel.ValidateFormDelegate = () => false;
            billingAddressPageViewModel.ValidateFormDelegate = () => true;
            paymentMethodPageViewModel.ValidateFormDelegate = () => true;
            await target.GoNextCommand.Execute();

            Assert.IsFalse(formProcessStarted);

            // BillingAddress invalid only
            shippingAddressPageViewModel.ValidateFormDelegate = () => true;
            billingAddressPageViewModel.ValidateFormDelegate = () => false;
            paymentMethodPageViewModel.ValidateFormDelegate = () => true;

            Assert.IsFalse(formProcessStarted);

            // PaymentMethod invalid only
            shippingAddressPageViewModel.ValidateFormDelegate = () => true;
            billingAddressPageViewModel.ValidateFormDelegate = () => true;
            paymentMethodPageViewModel.ValidateFormDelegate = () => false;

            Assert.IsFalse(formProcessStarted);
        }

        [TestMethod]
        public async Task SettingUseShippingAddressToTrue_CopiesValuesFromShippingAddressToBilling()
        {
            var mockAddress = new Address()
                {
                    FirstName = "TestFirstName",
                    MiddleInitial = "TestMiddleInitial",
                    LastName = "TestLastName",
                    StreetAddress = "TestStreetAddress",
                    OptionalAddress = "TestOptionalAddress",
                    City = "TestCity",
                    State = "TestState",
                    ZipCode = "123456",
                    Phone = "123456"
                };
            var compareAddressesFunc = new Func<Address, Address, bool>((Address a1, Address a2) =>
                {
                    return a1.FirstName == a2.FirstName && a1.MiddleInitial == a2.MiddleInitial && a1.LastName == a2.LastName
                           && a1.StreetAddress == a2.StreetAddress && a1.OptionalAddress == a2.OptionalAddress && a1.City == a2.City
                           && a1.State == a2.State && a1.ZipCode == a2.ZipCode && a1.Phone == a2.Phone;
                });

            var shippingAddressPageViewModel = new MockShippingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormAsyncDelegate = () => Task.Delay(0),
                    Address = mockAddress
                };
            var billingAddressPageViewModel = new MockBillingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true
                };
            billingAddressPageViewModel.ProcessFormAsyncDelegate = () =>
                {
                    // The Address have to be updated before the form is processed
                    Assert.IsTrue(compareAddressesFunc(shippingAddressPageViewModel.Address, billingAddressPageViewModel.Address));
                    return Task.Delay(0);
                };
            var paymentMethodPageViewModel = new MockPaymentMethodPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormAsyncDelegate = async () => await Task.Delay(0),
                };
            var accountService = new MockAccountService()
                {
                    VerifyUserAuthenticationAsyncDelegate = () => Task.FromResult(new UserInfo()),
                    SignedInUser = new UserInfo()
                };
            var orderRepository = new MockOrderRepository()
                {
                    CreateBasicOrderAsyncDelegate = (userId, shoppingCart, shippingAddress, billingAddress, paymentMethod) =>
                        {
                            // The Address information stored in the order must be the same
                            Assert.IsTrue(compareAddressesFunc(shippingAddress, billingAddress));
                            return Task.FromResult<Order>(new Order());
                        }
                };
            var shoppingCartRepository = new MockShoppingCartRepository()
                {
                    GetShoppingCartAsyncDelegate = () => Task.FromResult(new ShoppingCart(null))
                };
            var navigationService = new MockNavigationService()
                {
                    NavigateDelegate = (a, b) => true
                };

            var target = new CheckoutHubPageViewModel(navigationService, accountService, orderRepository, shoppingCartRepository,
                                            shippingAddressPageViewModel, billingAddressPageViewModel, paymentMethodPageViewModel, null, null);
            target.UseSameAddressAsShipping = true;

            await target.GoNextCommand.Execute();
        }

        [TestMethod]
        public void ProcessFormAsync_WithServerValidationError_ShowsMessage()
        {
            var shippingAddressPageViewModel = new MockShippingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormAsyncDelegate = () => Task.Delay(0),
                    Address = new Address()
                };
            var billingAddressPageViewModel = new MockBillingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormAsyncDelegate = () => Task.Delay(0),
                    Address = new Address()
                };
            var paymentMethodPageViewModel = new MockPaymentMethodPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormAsyncDelegate = async () => await Task.Delay(0),
                    PaymentMethod = new PaymentMethod()
                };
            var accountService = new MockAccountService()
                {
                    VerifyUserAuthenticationAsyncDelegate = () => Task.FromResult(new UserInfo()),
                    SignedInUser = new UserInfo()
                };
            var shoppingCartRepository = new MockShoppingCartRepository()
                {
                    GetShoppingCartAsyncDelegate =
                        () => Task.FromResult(new ShoppingCart(null))
                };
            var orderRepository = new MockOrderRepository()
                {
                    CreateBasicOrderAsyncDelegate = (s, cart, arg3, arg4, arg5) =>
                         {
                             var result = new ModelValidationResult();
                             result.ModelState.Add("order.ShippingAddress.ZipCode", new List<string>{"Validation Message"});
                             throw new ModelValidationException(result);
                         }
                };
            var target = new CheckoutHubPageViewModel(new MockNavigationService(), accountService, orderRepository, shoppingCartRepository, shippingAddressPageViewModel,
                                                      billingAddressPageViewModel, paymentMethodPageViewModel, null, null);

            target.GoNextCommand.Execute();

            Assert.IsTrue(target.IsShippingAddressInvalid);
        }
    }
}
