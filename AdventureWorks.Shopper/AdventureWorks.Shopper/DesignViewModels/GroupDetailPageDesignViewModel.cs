using System;
using System.Collections.Generic;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.ViewModels;

namespace AdventureWorks.Shopper.DesignViewModels
{
    public class GroupDetailPageDesignViewModel
    {
        public GroupDetailPageDesignViewModel()
        {
            FillWithDummyData();
        }

        public string Title { get; set; }

        public IEnumerable<object> Items { get; set; }

        private void FillWithDummyData()
        {
            Title = "Mountain Bikes";
            Items = new List<ProductViewModel>()
                {
                    new ProductViewModel(new Product() { Title = "Product 1",  Description = "Description of Product 1", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png"), Currency = "USD" }),
                    new ProductViewModel(new Product() { Title = "Product 2",  Description = "Description of Product 2", ListPrice = 45.10, DiscountPercentage = 10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png"), Currency = "USD" }),
                    new ProductViewModel(new Product() { Title = "Product 3",  Description = "Description of Product 3", ListPrice = 55.10, DiscountPercentage = 10, ProductNumber = "3", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png"), Currency = "USD" }),
                    new ProductViewModel(new Product() { Title = "Product 4",  Description = "Description of Product 4", ListPrice = 65.10, DiscountPercentage = 10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png"), Currency = "USD" }),
                    new ProductViewModel(new Product() { Title = "Product 5",  Description = "Description of Product 5", ListPrice = 25.99, DiscountPercentage = 10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png"), Currency = "USD" }),
                    new ProductViewModel(new Product() { Title = "Product 6",  Description = "Description of Product 6", ListPrice = 35.10, DiscountPercentage = 10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png"), Currency = "USD" })
                };
        }
    }
}
