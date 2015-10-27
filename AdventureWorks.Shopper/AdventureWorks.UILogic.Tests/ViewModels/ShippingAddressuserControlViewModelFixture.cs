// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;
using Prism.Windows.Navigation;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ShippingAddressUserControlViewModelFixture
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
            checkoutDataRepository.GetDefaultShippingAddressAsyncDelegate = () => Task.FromResult(defaultAddress);
            var locationService = new MockLocationService();
            var resourceLoader = new MockResourceLoader();
            var target = new ShippingAddressUserControlViewModel(checkoutDataRepository, locationService, resourceLoader, null);

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
            checkoutDataRepository.GetAllShippingAddressesAsyncDelegate =
                () => Task.FromResult<ICollection<Address>>(new ReadOnlyCollection<Address>(existingAddresses));

            var target = new ShippingAddressUserControlViewModel(checkoutDataRepository, null, null, null);
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
            checkoutDataRepository.GetAllShippingAddressesAsyncDelegate =
                () => Task.FromResult<ICollection<Address>>(new Collection<Address>(existingAddresses));

            checkoutDataRepository.SaveShippingAddressAsyncDelegate = address =>
                                                                          {
                                                                              saveAddressCalled = true;
                                                                              Assert.AreEqual("teststreetaddress",
                                                                                              address.StreetAddress);
                                                                              return Task.Delay(0);
                                                                          };
            var target = new ShippingAddressUserControlViewModel(checkoutDataRepository, null, null, null);
            target.Address = newAddress;

            await target.ProcessFormAsync();

            Assert.IsTrue(saveAddressCalled);
        }
    }
}
