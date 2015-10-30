using System.Collections.ObjectModel;

namespace AdventureWorks.UILogic.Models
{
    public class SearchResult
    {
        public SearchResult(int totalCount, Collection<Product> products)
        {
            TotalCount = totalCount;
            Products = products;
        }

        public int TotalCount { get; private set; }

        public Collection<Product> Products { get; private set; }
    }
}
