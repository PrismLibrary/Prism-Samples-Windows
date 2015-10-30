

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;
using System.Reflection.Metadata;
using Prism.Windows.Navigation;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class PaymentMethodUserControlViewModelFixture
    {
        [TestMethod]
        public void OnNavigateTo_LoadsDefault_IfTryLoadDefaultTrue()
        {
            var defaultPaymentMethod = new PaymentMethod
            {
                CardholderName = "CardHolderName",
                CardNumber = "32323232",
                CardVerificationCode = "222",
                Phone = "22224232",
                ExpirationMonth = "12",
                ExpirationYear = "2016"
            };
            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetDefaultPaymentMethodAsyncDelegate = () => Task.FromResult(defaultPaymentMethod);
            var target = new PaymentMethodUserControlViewModel(checkoutDataRepository);

            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = null, NavigationMode = NavigationMode.New }, new Dictionary<string, object>());
            Assert.IsNull(target.PaymentMethod.CardholderName);

            target.SetLoadDefault(true);
            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = null, NavigationMode = NavigationMode.New }, new Dictionary<string, object>());
            Assert.AreEqual("CardHolderName", target.PaymentMethod.CardholderName);
        }

        [TestMethod]
        public async Task ProcessFormAsync_UsesExistingIfMatchingFound()
        {
            var newPaymentMethod = new PaymentMethod
            {
                CardNumber = "1234",
                CardholderName = "testcardholdername"
            };

            var existingPaymentMethods = new List<PaymentMethod>
                                        {
                                            new PaymentMethod
                                                {
                                                    Id = "testId",
                                                    CardNumber = "1234",
                                                    CardholderName = "testcardholdername"
                                                }
                                        };

            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetAllPaymentMethodsAsyncDelegate =
                () => Task.FromResult<ICollection<PaymentMethod>>(new ReadOnlyCollection<PaymentMethod>(existingPaymentMethods));

            var target = new PaymentMethodUserControlViewModel(checkoutDataRepository);
            target.PaymentMethod = newPaymentMethod;

            await target.ProcessFormAsync();

            Assert.AreEqual("testId", target.PaymentMethod.Id);
        }

        [TestMethod]
        public async Task ProcessFormAsync_SavesPaymentMethodIfNoMatchingFound()
        {
            var savePaymentMethodCalled = false;
            var newPaymentMethod = new PaymentMethod
            {
                CardNumber = "1234",
                CardholderName = "testcardholdername"
            };

            var existingPaymentMethods = new List<PaymentMethod>();
            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetAllPaymentMethodsAsyncDelegate =
                () => Task.FromResult<ICollection<PaymentMethod>>(new Collection<PaymentMethod>(existingPaymentMethods));

            checkoutDataRepository.SavePaymentMethodAsyncDelegate = paymentMethod =>
            {
                savePaymentMethodCalled = true;
                Assert.AreEqual("testcardholdername",
                                paymentMethod.CardholderName);
                return Task.Delay(0);
            };
            var target = new PaymentMethodUserControlViewModel(checkoutDataRepository);
            target.PaymentMethod = newPaymentMethod;

            await target.ProcessFormAsync();

            Assert.IsTrue(savePaymentMethodCalled);
        }
    }
}
