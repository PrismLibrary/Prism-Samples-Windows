

using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class OrderConfirmationPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatedTo_ClearsNavigationHistory()
        {
            bool clearHistoryCalled = false;
            var navigationService = new MockNavigationService();
            navigationService.ClearHistoryDelegate = () =>
            {
                clearHistoryCalled = true;
            };
            var resourcesService = new MockResourceLoader()
            {
                GetStringDelegate = (key) => key
            };
            var target = new OrderConfirmationPageViewModel(resourcesService, navigationService);
            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = null, NavigationMode = NavigationMode.Forward }, null);

            Assert.IsTrue(clearHistoryCalled);
        }
    }
}
