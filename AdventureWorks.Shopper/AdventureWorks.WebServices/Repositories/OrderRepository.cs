

using System.Collections.Generic;
using AdventureWorks.WebServices.Models;
using System.Linq;
using System;

namespace AdventureWorks.WebServices.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private static ICollection<Order> _orders = new List<Order>();
        private static object _lock = new object();

        public IEnumerable<Order> GetAll()
        {
            lock (_lock)
            {
                // Return new collection so callers can iterate independently on separate threads
                return _orders.ToArray();
            }
        }

        public Order GetItem(int id)
        {
            lock (_lock)
            {
                return _orders.FirstOrDefault(c => c.Id == id);
            }
        }

        public Order Create(Order item)
        {
            lock (_lock)
            {
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }

                item.Id = _orders.Any() ? _orders.Max(c => c.Id) + 1 : 1;
                _orders.Add(item);
                return item;
            }
        }

        public bool Update(Order item)
        {
            lock (_lock)
            {
                var order = _orders.FirstOrDefault(c => c.Id == item.Id);

                if (order != null)
                {
                    order.ShoppingCart = item.ShoppingCart;
                    order.ShippingAddress = item.ShippingAddress;
                    order.BillingAddress = item.BillingAddress;
                    order.PaymentMethod = item.PaymentMethod;
                    order.ShippingMethod = item.ShippingMethod;

                    return true;
                }

                return false;
            }
        }

        public bool Delete(int id)
        {
            lock (_lock)
            {
                var order = _orders.FirstOrDefault(c => c.Id == id);
                return _orders.Remove(order);
            }
        }

        public void Reset()
        {
            lock (_lock)
            {
                _orders = new List<Order>();
            }
        }
    }
}