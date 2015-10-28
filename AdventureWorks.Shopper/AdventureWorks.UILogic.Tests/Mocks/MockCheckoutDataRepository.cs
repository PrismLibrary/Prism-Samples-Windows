

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockCheckoutDataRepository : ICheckoutDataRepository
    {
        public Func<Task<ICollection<Address>>> GetAllShippingAddressesAsyncDelegate { get; set; }
        public Func<Task<ICollection<Address>>> GetAllBillingAddressesAsyncDelegate { get; set; }
        public Func<Task<ICollection<PaymentMethod>>> GetAllPaymentMethodsAsyncDelegate { get; set; }
        public Func<string, Task<Address>> GetShippingAddressAsyncDelegate { get; set; }
        public Func<string, Task<Address>> GetBillingAddressAsyncDelegate { get; set; }
        public Func<string, Task<PaymentMethod>> GetPaymentMethodDelegate { get; set; }
        public Func<Task<Address>> GetDefaultShippingAddressAsyncDelegate { get; set; }
        public Func<Task<Address>> GetDefaultBillingAddressAsyncDelegate { get; set; }
        public Func<Task<PaymentMethod>> GetDefaultPaymentMethodAsyncDelegate { get; set; }
        public Func<Address, Task> SaveShippingAddressAsyncDelegate { get; set; }
        public Func<Address, Task> SaveBillingAddressAsyncDelegate { get; set; }
        public Func<PaymentMethod, Task> SavePaymentMethodAsyncDelegate { get; set; }

        public Task<Address> GetShippingAddressAsync(string id)
        {
            return GetShippingAddressAsyncDelegate(id);
        }

        public Task<Address> GetBillingAddressAsync(string id)
        {
            return GetBillingAddressAsyncDelegate(id);
        }

        public Task<PaymentMethod> GetPaymentMethodAsync(string id)
        {
            return GetPaymentMethodDelegate(id);
        }

        public Task<Address> GetDefaultShippingAddressAsync()
        {
            return GetDefaultShippingAddressAsyncDelegate();
        }

        public Task<Address> GetDefaultBillingAddressAsync()
        {
            return GetDefaultBillingAddressAsyncDelegate();
        }

        public Task<PaymentMethod> GetDefaultPaymentMethodAsync()
        {
            return GetDefaultPaymentMethodAsyncDelegate();
        }

        public Task<ICollection<Address>> GetAllShippingAddressesAsync()
        {
            return GetAllShippingAddressesAsyncDelegate();
        }

        public Task<ICollection<Address>> GetAllBillingAddressesAsync()
        {
            return GetAllBillingAddressesAsyncDelegate();
        }

        public Task<ICollection<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            return GetAllPaymentMethodsAsyncDelegate();
        }

        public Task SaveShippingAddressAsync(Address address)
        {
            return SaveShippingAddressAsyncDelegate(address);
        }

        public Task SaveBillingAddressAsync(Address address)
        {
            return SaveBillingAddressAsyncDelegate(address);
        }

        public Task SavePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            return SavePaymentMethodAsyncDelegate(paymentMethod);
        }

        public Task SetDefaultShippingAddressAsync(string addressId)
        {
            throw new NotImplementedException();
        }

        public Task SetDefaultBillingAddressAsync(string addressId)
        {
            throw new NotImplementedException();
        }

        public Task SetDefaultPaymentMethodAsync(string paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultShippingAddress(string address)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultBillingAddress(string address)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultPaymentMethod(string paymentMethod)
        {
            throw new NotImplementedException();
        }

        public Task RemoveDefaultShippingAddressAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveDefaultBillingAddressAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveDefaultPaymentMethodAsync()
        {
            throw new NotImplementedException();
        }
    }
}
