

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.UI.Xaml.Navigation;
using Prism.Windows.Navigation;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class BillingAddressUserControlViewModelFixture
    {
        [TestMethod]
        public void OnNavigateTo_LoadsDefault_IfTryLoadDefaultTrue()
        {
            var defaultAddress = new Address
            {
                FirstName = "FirstName",
                State = "WA"
            };
            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetDefaultBillingAddressAsyncDelegate = () => Task.FromResult(defaultAddress);
            var locationService = new MockLocationService();
            var resourceLoader = new MockResourceLoader();
            var target = new BillingAddressUserControlViewModel(checkoutDataRepository, locationService, resourceLoader, null);

            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = null, NavigationMode = NavigationMode.New }, new Dictionary<string, object>());
            Assert.IsNull(target.Address.FirstName);

            target.SetLoadDefault(true);
            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = null, NavigationMode = NavigationMode.New }, new Dictionary<string, object>());
            Assert.AreEqual("FirstName", target.Address.FirstName);
        }

        [TestMethod]
        public async Task ProcessFormAsync_UsesExistingAddressIfMatchingFound()
        {
            var newAddress = new Address
            {
                FirstName = "testfirst",
                StreetAddress = "teststreetaddress"
            };

            var existingAddresses = new List<Address>
                                        {
                                            new Address
                                                {
                                                    Id = "testId",
                                                    FirstName = "testfirst",
                                                    StreetAddress = "teststreetaddress"
                                                }
                                        };

            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetAllBillingAddressesAsyncDelegate =
                () => Task.FromResult<ICollection<Address>>(new ReadOnlyCollection<Address>(existingAddresses));

            var target = new BillingAddressUserControlViewModel(checkoutDataRepository, null, null, null);
            target.Address = newAddress;

            await target.ProcessFormAsync();

            Assert.AreEqual("testId", target.Address.Id);
        }

        [TestMethod]
        public async Task ProcessFormAsync_SavesAddressIfNoMatchingFound()
        {
            var saveAddressCalled = false;
            var newAddress = new Address
            {
                FirstName = "testfirst",
                StreetAddress = "teststreetaddress"
            };

            var existingAddresses = new List<Address>();
            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetAllBillingAddressesAsyncDelegate =
                () => Task.FromResult<ICollection<Address>>(new Collection<Address>(existingAddresses));

            checkoutDataRepository.SaveBillingAddressAsyncDelegate = address =>
            {
                saveAddressCalled = true;
                Assert.AreEqual("teststreetaddress",
                                address.StreetAddress);
                return Task.Delay(0);
            };
            var target = new BillingAddressUserControlViewModel(checkoutDataRepository, null, null, null);
            target.Address = newAddress;

            await target.ProcessFormAsync();

            Assert.IsTrue(saveAddressCalled);
        }
    }
}
