

using System;

namespace AdventureWorks.WebServices.Models
{
    public class Product
    {
        public string ProductNumber { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Uri ImageUri { get; set; }

        public int SubcategoryId { get; set; }

        public double ListPrice { get; set; }

        public double DiscountPercentage { get; set; }

        public double Weight { get; set; }

        public string Color { get; set; }

        public string Currency { get; set; }
    }
}