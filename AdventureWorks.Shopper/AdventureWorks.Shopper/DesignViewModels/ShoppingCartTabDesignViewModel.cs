// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace AdventureWorks.Shopper.DesignViewModels
{
    public class ShoppingCartTabDesignViewModel
    {
        public ShoppingCartTabDesignViewModel()
        {
            FillWithDummyData();
        }

        public int ItemCount { get; set; }

        private void FillWithDummyData()
        {
            ItemCount = 5;
        }
    }
}
