

using System.Collections.Generic;
using AdventureWorks.WebServices.Models;
using System.Linq;
using System;

namespace AdventureWorks.WebServices.Repositories
{
    public class ShippingMethodRepository : IRepository<ShippingMethod>
    {
        private static IEnumerable<ShippingMethod> _shippingMethod = PopulateShippingMethods();

        public IEnumerable<ShippingMethod> GetAll()
        {
            lock (_shippingMethod)
            {
                // Return new collection so callers can iterate independently on separate threads
                return _shippingMethod.ToArray();
            }
        }

        public ShippingMethod GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public ShippingMethod Create(ShippingMethod item)
        {
            throw new NotImplementedException();
        }

        public bool Update(ShippingMethod item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<ShippingMethod> PopulateShippingMethods()
        {
            return new List<ShippingMethod>()
            {
                new ShippingMethod() { Id = 1, Description = "Standard Shipping", EstimatedTime = "5-8 business days", Cost = 7.65 },
                new ShippingMethod() { Id = 2, Description = "Expedited Shipping", EstimatedTime = "3-5 business days", Cost = 14.25 },
                new ShippingMethod() { Id = 3, Description = "Two-day Shipping", EstimatedTime = "2 business days", Cost = 26.45 },
                new ShippingMethod() { Id = 4, Description = "One-day Shipping", EstimatedTime = "1 business day", Cost = 37.32 }
            };
        }


        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}