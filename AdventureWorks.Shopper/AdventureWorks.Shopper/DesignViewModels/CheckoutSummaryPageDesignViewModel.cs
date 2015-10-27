// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.ViewModels;
using Windows.ApplicationModel.Resources;

namespace AdventureWorks.Shopper.DesignViewModels
{
    public class CheckoutSummaryPageDesignViewModel
    {
        public CheckoutSummaryPageDesignViewModel()
        {
            FillWithDummyData();
        }

        public string OrderSubtotal { get; set; }

        public string ShippingCost { get; set; }

        public string TaxCost { get; set; }

        public string GrandTotal { get; set; }

        public IEnumerable<CheckoutDataViewModel> CheckoutDataViewModels { get; set; }

        public IEnumerable<ShippingMethod> ShippingMethods { get; set; }

        public IEnumerable<ShoppingCartItemViewModel> ShoppingCartItemViewModels { get; set; }

        public IEnumerable<CheckoutDataViewModel> AllCheckoutDataViewModels { get; set; }

        public string SelectCheckoutDataLabel { get; set; }

        public bool IsBottomAppBarOpened { get; set; }

        public ShippingMethod SelectedShippingMethod { get; set; }

        public CheckoutDataViewModel SelectedCheckoutData { get; set; }

        public CheckoutDataViewModel SelectedAllCheckoutData { get; set; }

        private void FillWithDummyData()
        {
            OrderSubtotal = "$100.00";
            ShippingCost = "$20.00";
            TaxCost = "$5.00";
            GrandTotal = "$125.00";

            IsBottomAppBarOpened = true;

            SelectCheckoutDataLabel = "Select Shipping Address";

            var resourceLoader = new ResourceLoader();
            CheckoutDataViewModels = new List<CheckoutDataViewModel>()
                {
                    new CheckoutDataViewModel() { EntityId = "1", Title = resourceLoader.GetString("ShippingAddress"), FirstLine = "12345 Main St NE",  SecondLine = "Seattle, WA 54321", BottomLine = "Name Lastname", LogoUri = new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute), DataType = resourceLoader.GetString("ShippingAddress") },
                    new CheckoutDataViewModel() { EntityId = "1", Title = resourceLoader.GetString("BillingAddress"), FirstLine = "12345 Main St NE",  SecondLine = "Seattle, WA 54321", BottomLine = "Name Lastname", LogoUri = new Uri("ms-appx:///Assets/billingAddressLogo.png", UriKind.Absolute), DataType = resourceLoader.GetString("BillingAddress") },
                    new CheckoutDataViewModel() { EntityId = "1", Title = resourceLoader.GetString("PaymentMethod"), FirstLine = "Card ending in 1234",  SecondLine = "Card expiring in 10/2014", BottomLine = "Name Lastname", LogoUri = new Uri("ms-appx:///Assets/paymentMethodLogo.png", UriKind.Absolute), DataType = resourceLoader.GetString("PaymentMethod") }
                };
            SelectedCheckoutData = CheckoutDataViewModels.First();

            ShippingMethods = new List<ShippingMethod>()
                {
                    new ShippingMethod() { Id = 1, Description = "Shipping Method 1", Cost = 1.50, EstimatedTime = "1 year" },
                    new ShippingMethod() { Id = 2, Description = "Shipping Method 2", Cost = 15.10, EstimatedTime = "1 month" },
                    new ShippingMethod() { Id = 3, Description = "Shipping Method 3", Cost = 151.0, EstimatedTime = "1 day" },
                };
            SelectedShippingMethod = ShippingMethods.First();

            ShoppingCartItemViewModels = new List<ShoppingCartItemViewModel>()
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
                            Quantity = 1,
                            Currency = "USD"
                        },
                        null),
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() { Title = "Product 3",  Description = "Description of Product 3", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "3", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png") },
                            Quantity = 1,
                            Currency = "USD"
                        },
                        null),
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() { Title = "Product 4",  Description = "Description of Product 4", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "4", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png") },
                            Quantity = 1,
                            Currency = "USD"
                        },
                        null),
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() { Title = "Product 5",  Description = "Description of Product 5", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "5", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png") },
                            Quantity = 1,
                            Currency = "USD"
                        },
                        null),
                };

            AllCheckoutDataViewModels = new List<CheckoutDataViewModel>()
                {
                    new CheckoutDataViewModel() { EntityId = "1", DataType = resourceLoader.GetString("ShippingAddress") },
                    new CheckoutDataViewModel() { EntityId = "1", DataType = resourceLoader.GetString("BillingAddress") },
                    new CheckoutDataViewModel() { EntityId = "1", DataType = resourceLoader.GetString("PaymentMethod") }
                };
            SelectedAllCheckoutData = AllCheckoutDataViewModels.First();
        }
    }
}
