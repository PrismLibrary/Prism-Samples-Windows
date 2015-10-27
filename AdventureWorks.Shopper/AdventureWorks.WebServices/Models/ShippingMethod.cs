// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace AdventureWorks.WebServices.Models
{
    public class ShippingMethod
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string EstimatedTime { get; set; }

        public double Cost { get; set; }
    }
}
