// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace AdventureWorks.WebServices.Models
{
    public class ShoppingCartItem
    {
        public string Id { get; set; }

        public Product Product{ get; set;}

        public int Quantity { get; set; }

        public string Currency { get; set; }
    }
}
