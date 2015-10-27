// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.ViewModels;

namespace AdventureWorks.Shopper.DesignViewModels
{
    public class CheckoutHubPageDesignViewModel
    {
        public CheckoutHubPageDesignViewModel()
        {
            FillWithDummyData();
        }

        public IShippingAddressUserControlViewModel ShippingAddressViewModel { get; set; }

        public IBillingAddressUserControlViewModel BillingAddressViewModel { get; set; }

        public IPaymentMethodUserControlViewModel PaymentMethodViewModel { get; set; }

        public bool UseSameAddressAsShipping { get; set; }

        public bool IsShippingAddressInvalid { get; set; }

        public bool IsBillingAddressInvalid { get; set; }

        public bool IsPaymentMethodInvalid { get; set; }

        private void FillWithDummyData()
        {
            ShippingAddressViewModel = new ShippingAddressUserControlViewModel(null, null, null, null)
            {
                Address = new Address()
                {
                    FirstName = "Name",
                    MiddleInitial = "M",
                    LastName = "Lastname",
                    StreetAddress = "12345 Main St NE",
                    City = "Seattle",
                    State = "Washington",
                    ZipCode = "54321",
                    Phone = "1234 5678 9876" 
                }
            };

            BillingAddressViewModel = new BillingAddressUserControlViewModel(null, null, null, null);
            UseSameAddressAsShipping = true;
            BillingAddressViewModel.IsEnabled = !UseSameAddressAsShipping;

            PaymentMethodViewModel = new PaymentMethodUserControlViewModel(null)
            {
                PaymentMethod = new PaymentMethod()
                {
                    CardholderName = "Name Lastname",
                    CardNumber = "1234",
                    CardVerificationCode = "54321",
                    ExpirationMonth = "10",
                    ExpirationYear = "2014",
                    Phone = "1234 5678 9876" 
                },
            };

            // Validation
            IsShippingAddressInvalid = ShippingAddressViewModel.Address.Errors.GetAllErrors().Count > 0;
            IsBillingAddressInvalid = BillingAddressViewModel.Address.Errors.GetAllErrors().Count > 0;
            IsPaymentMethodInvalid = PaymentMethodViewModel.PaymentMethod.Errors.GetAllErrors().Count > 0;
        }
    }
}
