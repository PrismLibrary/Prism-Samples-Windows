

using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace AdventureWorks.WebServices.Controllers
{
    public class ProductController : ApiController
    {
        // Another approach to bounding the search result set to a fixed number is to
        // implement ISupportIncrementalLoading
        private const int MaxSearchResults = 1000;

        private IProductRepository _productRepository;

        public ProductController()
            : this(new ProductRepository())
        { }

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET /api/Product
        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        // GET /api/Product/id
        public Product GetProduct(string id)
        {
            var item = _productRepository.GetProduct(id);

            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        // GET /api/Product?queryString={queryString}
        public SearchResult GetSearchResults(string queryString, int maxResults)
        {
            var fullsearchResult = _productRepository.GetProducts().Where(p => p.Title.ToUpperInvariant().Contains(queryString.ToUpperInvariant()));
            
            var searchResult = new SearchResult
                                   {
                                       TotalCount = fullsearchResult.Count(),
                                       Products = fullsearchResult.Take(maxResults > 0 ? maxResults : MaxSearchResults)
                                   };

            return searchResult;

        }

        // GET /api/Product?categoryId={categoryId}
        public IEnumerable<Product> GetProducts(int categoryId)
        {
            if (categoryId == 0)
            {
                return _productRepository.GetTodaysDealsProducts();
            }

            return _productRepository.GetProductsForCategory(categoryId);
        }
    }
}
