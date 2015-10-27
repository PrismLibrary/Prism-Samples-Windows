// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace AdventureWorks.WebServices.Models
{
    public class SearchResult
    {
        public int TotalCount { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}