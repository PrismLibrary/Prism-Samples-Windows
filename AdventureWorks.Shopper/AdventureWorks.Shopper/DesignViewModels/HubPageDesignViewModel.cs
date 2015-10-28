using System;
using System.Collections.Generic;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.ViewModels;

namespace AdventureWorks.Shopper.DesignViewModels
{
    public class HubPageDesignViewModel
    {
        public HubPageDesignViewModel()
        {
            FillWithDummyData();
        }
        
        public IEnumerable<CategoryViewModel> RootCategories { get; set; }

        public bool LoadingData { get; set; }

        public string Title { get; set; }

        public void FillWithDummyData()
        {
            Title = "Accessories";
            RootCategories = new List<CategoryViewModel>()
                {
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 1", 
                        Products = new List<Product>()
                            {
                                new Product() { Title = "Product 1", Description = "Description of Product 1", ListPrice = 25.10, Currency = "$", DiscountPercentage = 10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png") },
                                new Product() { Title = "Product 2", Description = "Description of Product 2", ListPrice = 25.10, Currency = "$", DiscountPercentage = 10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png") },
                                new Product() { Title = "Product 3", Description = "Description of Product 3", ListPrice = 25.10, Currency = "$", DiscountPercentage = 10, ProductNumber = "3", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png") },
                            }
                    }, 
                    null),
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 2", 
                        Products = new List<Product>()
                            {
                                new Product() { Title = "Product 1",  Description = "Description of Product 1", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png") },
                                new Product() { Title = "Product 2",  Description = "Description of Product 2", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png") },
                            }
                    },
                    null),
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 3", 
                        Products = new List<Product>()
                            {
                                new Product() { Title = "Product 1",  Description = "Description of Product 1", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png") },
                            }
                    },
                    null)
                };
        }
    }
}
