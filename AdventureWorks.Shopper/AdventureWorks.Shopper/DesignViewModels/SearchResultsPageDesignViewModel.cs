// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.ViewModels;
using System;
using System.Collections.Generic;

namespace AdventureWorks.Shopper.DesignViewModels
{
    public class SearchResultsPageDesignViewModel
    {
        public SearchResultsPageDesignViewModel()
        {
            this.QueryText = '\u201c' + "bike" + '\u201d';
            this.SearchTerm = "bike";
            this.NoResults = false;
            this.TotalCount = 99;
            FillWithDummyData();
        }

        public string QueryText { get; set; }

        public string SearchTerm { get; set; }

        public bool NoResults { get; set; }

        public int TotalCount { get; set; }

        public List<ProductViewModel> Results { get; private set; }

        public void FillWithDummyData()
        {
            Results = new List<ProductViewModel>()
                {
                     new ProductViewModel(new Product() { Title = "Bike 1",  Description = "Description of Product 1", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" }),
                     new ProductViewModel(new Product() { Title = "Bike 2",  Description = "Description of Product 2", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" }),
                     new ProductViewModel(new Product() { Title = "Bike 3",  Description = "Description of Product 3", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "3", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" }),
                     new ProductViewModel(new Product() { Title = "Bike Lock",  Description = "Description of Product 1", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" }),
                     new ProductViewModel(new Product() { Title = "Red Mountain Bike with light blue inclusions in the frame.",  Description = "Description of Product 2", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" }),
                     new ProductViewModel(new Product() { Title = "Blue Bike Cover",  Description = "Description of Product 1", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" })
                };
        }
    }
}
