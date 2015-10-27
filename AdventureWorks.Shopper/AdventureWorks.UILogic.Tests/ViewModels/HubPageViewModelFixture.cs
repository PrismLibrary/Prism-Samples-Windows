// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using Prism.Windows.Navigation;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class HubPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatedTo_Fill_RootCategories()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            repository.GetRootCategoriesAsyncDelegate = (maxAmmountOfProducts) =>
            {
                var categories = new Collection<Category>(new List<Category>{
                    new Category(),
                    new Category(),
                    new Category()
                });

                return Task.FromResult((ICollection<Category>)categories);
            };

            var viewModel = new HubPageViewModel(repository, navigationService, null, null);
            viewModel.OnNavigatedTo(new NavigatedToEventArgs { Parameter = null, NavigationMode = NavigationMode.New }, null);

            Assert.IsNotNull(viewModel.RootCategories);
            Assert.AreEqual(((ICollection<CategoryViewModel>)viewModel.RootCategories).Count, 3);
        }
        [TestMethod]
        public void FailedCallToProductCatalogRepository_ShowsAlert()
        {
            var alertCalled = false;
            var productCatalogRepository = new MockProductCatalogRepository();
            var navService = new MockNavigationService();
            productCatalogRepository.GetRootCategoriesAsyncDelegate = (maxAmmountOfProducts) =>
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
            var target = new HubPageViewModel(productCatalogRepository, navService,
                                                                 alertMessageService, new MockResourceLoader());
            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = null, NavigationMode = NavigationMode.New }, null);
            
            Assert.IsTrue(alertCalled);
        }
    }
}
