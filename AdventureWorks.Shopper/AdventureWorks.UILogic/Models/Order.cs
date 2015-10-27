// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace AdventureWorks.UILogic.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public Address ShippingAddress { get; set; }

        public Address BillingAddress { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ShippingMethod ShippingMethod { get; set; }
    }
}
