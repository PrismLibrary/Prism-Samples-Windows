

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using Prism.Windows.Navigation;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class GroupDetailPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatedTo_Fill_Items_And_Title()
        {
            var repository = new MockProductCatalogRepository();

            repository.GetCategoryAsyncDelegate = (categoryId) =>
            {
                Category category = null;

                if (categoryId == 1)
                {
                    category = new Category { Id = categoryId,
                        Title = "CategoryTitle"
                    };
                }

                return Task.FromResult(category);
            };
            repository.GetProductsAsyncDelegate = i =>
            {
                ICollection<Product> products = null;
                if (i == 1)
                {
                    products = new Collection<Product>(new List<Product>
                                    {
                                        new Product(),
                                        new Product(),
                                        new Product()
                                    });
                }
                return Task.FromResult(products);
            };

           var viewModel = new GroupDetailPageViewModel(repository, null, null);
            viewModel.OnNavigatedTo(new NavigatedToEventArgs { Parameter = 1, NavigationMode = NavigationMode.New }, null);

            Assert.IsNotNull(viewModel.Items);
            Assert.AreEqual(3, ((ICollection<ProductViewModel>)viewModel.Items).Count);
            Assert.AreEqual("CategoryTitle", viewModel.Title);
        }

        [TestMethod]
        public void OnNavigatedTo_When_Service_Not_Available_Then_Pops_Alert()
        {
            var repository = new MockProductCatalogRepository();
            var alertService = new MockAlertMessageService();
            var resourceLoader = new MockResourceLoader();

            bool alertCalled = false;
 
            repository.GetCategoryAsyncDelegate = (categoryId) =>
            {
                throw new Exception();
            };

            alertService.ShowAsyncDelegate = (msg, title) =>
            {
                alertCalled = true;
                return Task.FromResult(string.Empty);
            };

            var viewModel = new GroupDetailPageViewModel(repository, alertService, resourceLoader);
            viewModel.OnNavigatedTo(new NavigatedToEventArgs { Parameter = "1", NavigationMode = NavigationMode.New }, null);

            Assert.IsTrue(alertCalled);
        }
    }
}
