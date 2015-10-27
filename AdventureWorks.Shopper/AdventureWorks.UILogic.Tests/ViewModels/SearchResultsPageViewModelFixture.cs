// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class SearchResultsPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatingTo_Search_Results_Page_With_Search_Term()
        {
            var repository = new MockProductCatalogRepository();
            repository.GetFilteredProductsAsyncDelegate = (queryString, maxResults) =>
                {
                    Collection<Product> products;
                    if (queryString == "bike")
                        products = new Collection<Product>(new List<Product>
                        {
                            new Product(){Title = "bike1", ProductNumber = "1", ImageUri = new Uri("http://image")},
                            new Product(){Title = "bike2", ProductNumber = "2", ImageUri = new Uri("http://image")}
                        });
                    else
                    {
                        products = new Collection<Product>(new List<Product>
                        {
                            new Product(){Title = "bike1", ProductNumber = "1", ImageUri = new Uri("http://image")},
                            new Product(){Title = "bike2", ProductNumber = "2", ImageUri = new Uri("http://image")},
                            new Product(){Title = "product3", ProductNumber = "3", ImageUri = new Uri("http://image")}
                        });
                    }

                    return Task.FromResult(new SearchResult(3, products));
                };

            var target = new SearchResultsPageViewModel(repository, new MockResourceLoader(), new MockAlertMessageService());
            const string searchTerm = "bike";
            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = searchTerm, NavigationMode = NavigationMode.New }, null);
            Assert.AreEqual("bike", target.SearchTerm);
            Assert.IsNotNull(target.Results);
            Assert.AreEqual(2, target.Results.Count);
            var resultsThatDontMatch = target.Results.Any(p => !p.Title.Contains(searchTerm));
            Assert.IsFalse(resultsThatDontMatch);
        }

        [TestMethod]
        public void OnNavigatingTo_Search_Results_Page_Without_Search_Term()
        {
            var repository = new MockProductCatalogRepository();
            repository.GetFilteredProductsAsyncDelegate = (queryString, maxResults) =>
            {
                Collection<Product> products;
                if (queryString == "bike")
                    products = new Collection<Product>(new List<Product>
                        {
                            new Product(){Title = "bike1", ProductNumber = "1", ImageUri = new Uri("http://image")},
                            new Product(){Title = "bike2", ProductNumber = "2", ImageUri = new Uri("http://image")}
                        });
                else
                {
                    products = new Collection<Product>(new List<Product>
                        {
                            new Product(){Title = "bike1", ProductNumber = "1", ImageUri = new Uri("http://image")},
                            new Product(){Title = "bike2", ProductNumber = "2", ImageUri = new Uri("http://image")},
                            new Product(){Title = "product3", ProductNumber = "3", ImageUri = new Uri("http://image")}
                        });
                }

                return Task.FromResult(new SearchResult(3, products));
            };

            var target = new SearchResultsPageViewModel(repository, new MockResourceLoader(), new MockAlertMessageService());
            var searchTerm = string.Empty;
            target.OnNavigatedTo(new NavigatedToEventArgs { Parameter = searchTerm, NavigationMode = NavigationMode.New }, null);
            Assert.AreEqual(string.Empty, target.SearchTerm);
            Assert.IsNotNull(target.Results);
            Assert.AreEqual(3, target.Results.Count);
        }
    }
}
