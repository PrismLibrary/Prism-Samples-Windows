// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.ViewModels;

namespace AdventureWorks.Shopper.DesignViewModels
{
    public class ShoppingCartPageDesignViewModel
    {
        public ShoppingCartPageDesignViewModel()
        {
            FillWithDummyData();
        }

        public string FullPrice { get; set; }

        public string TotalDiscount { get; set; }

        public string TotalPrice { get; set; }

        public ObservableCollection<ShoppingCartItemViewModel> ShoppingCartItemViewModels { get; private set; }

        public ShoppingCartItemViewModel SelectedItem { get; set; }

        private void FillWithDummyData()
        {
            FullPrice = "$100.50";
            TotalDiscount = "$10.50";
            TotalPrice = "$90.00";

            ShoppingCartItemViewModels = new ObservableCollection<ShoppingCartItemViewModel>()
                {
                    new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() { Title = "Product 1",  Description = "Description of Product 1", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png") },
                            Quantity = 1,
                            Currency = "USD"
                        }, 
                        null),
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() { Title = "Product 2",  Description = "Description of Product 2", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png") },
                            Quantity = 20,
                            Currency = "USD"
                        }, 
                        null), 
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() { Title = "Product 3",  Description = "Description of Product 3", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "3", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png") },
                            Quantity = 30,
                            Currency = "USD"
                        }, 
                        null), 
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() { Title = "Product 4",  Description = "Description of Product 4", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "4", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png") },
                            Quantity = 14,
                            Currency = "USD"
                        }, 
                        null), 
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() { Title = "Product 5",  Description = "Description of Product 5", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "5", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png") },
                            Quantity = 25,
                            Currency = "USD"
                        }, 
                        null), 
                };

            SelectedItem = ShoppingCartItemViewModels[0];
        }
    }
}
