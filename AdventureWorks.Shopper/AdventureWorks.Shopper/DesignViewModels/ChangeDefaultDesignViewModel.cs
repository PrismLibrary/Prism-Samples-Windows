// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AdventureWorks.UILogic.ViewModels;
using Windows.ApplicationModel.Resources;

namespace AdventureWorks.Shopper.DesignViewModels
{
    public class ChangeDefaultsDesignViewModel
    {
        public ChangeDefaultsDesignViewModel()
        {
            FillWithDummyData();
        }

        public IEnumerable<CheckoutDataViewModel> ShippingAddresses { get; private set; }

        public IEnumerable<CheckoutDataViewModel> BillingAddresses { get; private set; }

        public IEnumerable<CheckoutDataViewModel> PaymentMethods { get; private set; }

        public CheckoutDataViewModel SelectedShippingAddress { get; set; }

        public CheckoutDataViewModel SelectedBillingAddress { get; set; }

        public CheckoutDataViewModel SelectedPaymentMethod { get; set; }

        private void FillWithDummyData()
        {
            var resourceLoader = new ResourceLoader();

            ShippingAddresses = new List<CheckoutDataViewModel>()
                {
                    new CheckoutDataViewModel()
                        {
                            EntityId = "1",
                            Title = resourceLoader.GetString("ShippingAddress"),
                            FirstLine = "12345 Main St NE",
                            SecondLine = "Seattle, WA 54321",
                            BottomLine = "Name Lastname",
                            LogoUri = new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute)
                        },
                    new CheckoutDataViewModel()
                        {
                            EntityId = "3",
                            Title = resourceLoader.GetString("ShippingAddress"),
                            FirstLine = "12345 Main St NE",
                            SecondLine = "Seattle, WA 54321",
                            BottomLine = "Name Lastname",
                            LogoUri = new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute)
                        },
                    new CheckoutDataViewModel()
                        {
                            EntityId = "3",
                            Title = resourceLoader.GetString("ShippingAddress"),
                            FirstLine = "12345 Main St NE",
                            SecondLine = "Seattle, WA 54321",
                            BottomLine = "Name Lastname",
                            LogoUri = new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute)
                        }
                };

            BillingAddresses = new List<CheckoutDataViewModel>()
                {
                    new CheckoutDataViewModel()
                        {
                            EntityId = "1",
                            Title = resourceLoader.GetString("BillingAddress"),
                            FirstLine = "12345 Main St NE",
                            SecondLine = "Seattle, WA 54321",
                            BottomLine = "Name Lastname",
                            LogoUri = new Uri("ms-appx:///Assets/billingAddressLogo.png", UriKind.Absolute)
                        },
                    new CheckoutDataViewModel()
                        {
                            EntityId = "2",
                            Title = resourceLoader.GetString("BillingAddress"),
                            FirstLine = "12345 Main St NE",
                            SecondLine = "Seattle, WA 54321",
                            BottomLine = "Name Lastname",
                            LogoUri = new Uri("ms-appx:///Assets/billingAddressLogo.png", UriKind.Absolute)
                        },
                    new CheckoutDataViewModel()
                        {
                            EntityId = "3",
                            Title = resourceLoader.GetString("BillingAddress"),
                            FirstLine = "12345 Main St NE",
                            SecondLine = "Seattle, WA 54321",
                            BottomLine = "Name Lastname",
                            LogoUri = new Uri("ms-appx:///Assets/billingAddressLogo.png", UriKind.Absolute)
                        },
                };

            PaymentMethods = new List<CheckoutDataViewModel>()
                {
                    new CheckoutDataViewModel()
                        {
                            EntityId = "1",
                            Title = resourceLoader.GetString("PaymentMethod"),
                            FirstLine = "Card ending in 1234",
                            SecondLine = "Card expiring in 10/2014",
                            BottomLine = "Name Lastname",
                            LogoUri = new Uri("ms-appx:///Assets/paymentMethodLogo.png", UriKind.Absolute)
                        },
                };

            SelectedShippingAddress = ShippingAddresses.First();
            SelectedBillingAddress = BillingAddresses.First();
            ////SelectedPaymentMethod = PaymentMethods.First();
        }
    }
}