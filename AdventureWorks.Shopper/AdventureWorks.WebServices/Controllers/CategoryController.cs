

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;


namespace AdventureWorks.WebServices.Controllers
{
    public class CategoryController : ApiController
    {
        private IRepository<Category> _categoryRepository;
        private IProductRepository _productRepository;

        public CategoryController()
            : this(new CategoryRepository(), new ProductRepository())
        { }

        public CategoryController(IRepository<Category> categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        // GET /api/Category?parentId={parentId}&maxAmmountOfProducts={maxAmountOfProducts}
        public IEnumerable<Category> GetCategories(int parentId, int maxAmountOfProducts)
        {
            var categories = _categoryRepository.GetAll().Where(c => c.ParentId == parentId);

            var trimmedCategories = categories.Select(NewCategory).ToList();
            FillProducts(trimmedCategories);

            foreach (var trimmedCategory in trimmedCategories)
            {
                var products = trimmedCategory.Products.ToList();
                if (maxAmountOfProducts > 0)
                {
                    products = products.Take(maxAmountOfProducts).ToList();
                }
                trimmedCategory.Products = products;
            }

            return trimmedCategories;
        }

        // GET /api/Category/id
        public Category GetCategory(int id)
        {
            var item = _categoryRepository.GetItem(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        private void FillProducts(IEnumerable<Category> categories)
        {
            foreach (var category in categories)
            {

                if (category.Id != 0)
                {
                    var subcategories = _categoryRepository.GetAll().Where(c => c.ParentId == category.Id).ToList();
                    var productList = new List<Product>();
                    if (subcategories.Count > 0)
                    {
                        category.HasSubcategories = true;
                        foreach (var subcategory in subcategories)
                        {
                            productList.AddRange(_productRepository.GetProductsForCategory(subcategory.Id));
                        }
                    }
                    else
                    {
                        category.HasSubcategories = false;
                        productList.AddRange(_productRepository.GetProductsForCategory(category.Id));
                    }
                    category.TotalNumberOfItems = productList.Count;
                    category.Products = productList;
                }
                else
                {
                    //Today's Deals Category
                    category.Products = _productRepository.GetTodaysDealsProducts();
                    category.TotalNumberOfItems = _productRepository.GetTodaysDealsProducts().Count();
                }
            }
        }

        private static Category NewCategory(Category category)
        {
            if (category != null)
            {
                return new Category()
                    {
                        Id = category.Id,
                        Title = category.Title,
                        ParentId = category.ParentId,
                        HasSubcategories = category.HasSubcategories,
                        Subcategories = category.Subcategories,
                        ImageUri = category.ImageUri,
                        Products = category.Products,
                        TotalNumberOfItems = category.TotalNumberOfItems
                    };
            }
            return null;
        }
    }
}
