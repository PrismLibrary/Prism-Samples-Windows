using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.Repositories
{
    public class CheckoutDataRepository : ICheckoutDataRepository
    {
        private readonly IAddressService _addressService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IAccountService _accountService;
        private ICollection<Address> _cachedAddresses;
        private ICollection<PaymentMethod> _cachedPaymentMethods;

        public CheckoutDataRepository(IAddressService addressService, IPaymentMethodService paymentMethodService, IAccountService accountService)
        {
            _addressService = addressService;
            _paymentMethodService = paymentMethodService;
            _accountService = accountService;
            if (accountService != null)
            {
                accountService.UserChanged += (sender, args) =>
                                                  {
                                                      ExpireCachedAddresses();
                                                      ExpireCachedPaymentMethods();
                                                  };
            }
        }

        public async Task<Address> GetShippingAddressAsync(string id)
        {
            return (await GetAllShippingAddressesAsync()).FirstOrDefault(s => s.Id == id);
        }

        public async Task<Address> GetBillingAddressAsync(string id)
        {
            return (await GetAllBillingAddressesAsync()).FirstOrDefault(b => b.Id == id);
        }

        public async Task<PaymentMethod> GetPaymentMethodAsync(string id)
        {
            return (await GetAllPaymentMethodsAsync()).FirstOrDefault(p => p.Id == id);
        }

        public async Task<Address> GetDefaultShippingAddressAsync()
        {
            var addresses = await GetAllAddressesViaCacheAsync();
            var defaultShippingAddress = (addresses != null)
                                             ? addresses.Where(address =>
                                                 address.AddressType == AddressType.Shipping && address.IsDefault).FirstOrDefault()
                                             : null;

            return defaultShippingAddress;
        }

        public async Task<Address> GetDefaultBillingAddressAsync()
        {
            var addresses = await GetAllAddressesViaCacheAsync();
            var defaultBillingAddress = (addresses != null)
                                             ? addresses.Where(address =>
                                                 address.AddressType == AddressType.Billing && address.IsDefault).FirstOrDefault()
                                             : null;

            return defaultBillingAddress;
        }

        public async Task<PaymentMethod> GetDefaultPaymentMethodAsync()
        {
            var paymentMethods = await GetAllPaymentMethodsViaCacheAsync();
            var defaultPaymentMethod = (paymentMethods != null)
                                             ? paymentMethods.Where(p => p.IsDefault).FirstOrDefault()
                                             : null;

            return defaultPaymentMethod;
        }

        public async Task<ICollection<Address>> GetAllShippingAddressesAsync()
        {
            var addresses = await GetAllAddressesViaCacheAsync();
            var shippingAddresses = (addresses != null)
                                        ? addresses.Where(address => address.AddressType == AddressType.Shipping)
                                        : new List<Address>();

            return new Collection<Address>(shippingAddresses.ToList());
        }

        public async Task<ICollection<Address>> GetAllBillingAddressesAsync()
        {
            var addresses = await GetAllAddressesViaCacheAsync();
            var billingAddresses = (addresses != null)
                                        ? addresses.Where(address => address.AddressType == AddressType.Billing)
                                        : new List<Address>();

            return new Collection<Address>(billingAddresses.ToList());
        }

        public async Task<ICollection<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            var paymentMethods = await GetAllPaymentMethodsViaCacheAsync();
            return paymentMethods ?? new Collection<PaymentMethod>(new Collection<PaymentMethod>());
        }

        public async Task SaveShippingAddressAsync(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            address.Id = address.Id ?? Guid.NewGuid().ToString();
            address.AddressType = AddressType.Shipping;

            // If there's no default value stored, use this one
            if (await GetDefaultShippingAddressAsync() == null)
            {
                address.IsDefault = true;
            }

            // Save the address to the service
            await _addressService.SaveAddressAsync(address);

            ExpireCachedAddresses();
        }

        public async Task SaveBillingAddressAsync(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            address.Id = address.Id ?? Guid.NewGuid().ToString();
            address.AddressType = AddressType.Billing;

            // If there's no default value stored, use this one
            if (await GetDefaultBillingAddressAsync() == null)
            {
                address.IsDefault = true;
            }

            // Save the address in the service
            await _addressService.SaveAddressAsync(address);

            ExpireCachedAddresses();
        }

        public async Task SavePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
            {
                throw new ArgumentNullException("paymentMethod");
            }

            paymentMethod.Id = paymentMethod.Id ?? Guid.NewGuid().ToString();

            // Sensitive data replaced with asterisks. Configure secure transport layer (SSL)
            // so that you can securely send sensitive data such as credit card data.
            var paymentMethodToSave = new PaymentMethod()
            {
                Id = paymentMethod.Id,
                CardNumber = "********",
                CardVerificationCode = "****",
                CardholderName = paymentMethod.CardholderName,
                ExpirationMonth = paymentMethod.ExpirationMonth,
                ExpirationYear = paymentMethod.ExpirationYear,
                Phone = paymentMethod.Phone
            };

            // If there's no default value stored, use this one
            if (await GetDefaultPaymentMethodAsync() == null)
            {
                paymentMethodToSave.IsDefault = true;
            }

            // Save the payment method to the service
            await _paymentMethodService.SavePaymentMethodAsync(paymentMethodToSave);

            ExpireCachedPaymentMethods();
        }

        public async Task RemoveDefaultShippingAddressAsync()
        {
            var defaultShippingAddress = await GetDefaultShippingAddressAsync();
            if (defaultShippingAddress != null)
            {
                defaultShippingAddress.IsDefault = false;
                await _addressService.SaveAddressAsync(defaultShippingAddress);
            }

            ExpireCachedAddresses();
        }

        public async Task RemoveDefaultBillingAddressAsync()
        {
            var defaultBillingAddress = await GetDefaultBillingAddressAsync();
            if (defaultBillingAddress != null)
            {
                defaultBillingAddress.IsDefault = false;
                await _addressService.SaveAddressAsync(defaultBillingAddress);
            }

            ExpireCachedAddresses();
        }

        public async Task RemoveDefaultPaymentMethodAsync()
        {
            var defaultPaymentMethod = await GetDefaultPaymentMethodAsync();
            if (defaultPaymentMethod != null)
            {
                defaultPaymentMethod.IsDefault = false;
                await _paymentMethodService.SavePaymentMethodAsync(defaultPaymentMethod);
            }

            ExpireCachedPaymentMethods();
        }

        public async Task SetDefaultShippingAddressAsync(string addressId)
        {
            await _addressService.SetDefault(addressId, AddressType.Shipping);
            ExpireCachedAddresses();
        }

        public async Task SetDefaultBillingAddressAsync(string addressId)
        {
            await _addressService.SetDefault(addressId, AddressType.Billing);
            ExpireCachedAddresses();
        }

        public async Task SetDefaultPaymentMethodAsync(string paymentMethodId)
        {
            await _paymentMethodService.SetDefault(paymentMethodId);
            ExpireCachedPaymentMethods();
        }

        private async Task<ICollection<Address>> GetAllAddressesViaCacheAsync()
        {
            if (_cachedAddresses == null)
            {
                bool retryWithRefreshedUser = false;
                try
                {
                    _cachedAddresses = await _addressService.GetAddressesAsync();
                }
                catch (Exception)
                {
                    retryWithRefreshedUser = true;
                }

                if (retryWithRefreshedUser)
                {
                    await _accountService.VerifyUserAuthenticationAsync();
                    _cachedAddresses = await _addressService.GetAddressesAsync();
                }
            }

            return _cachedAddresses;
        }

        private void ExpireCachedAddresses()
        {
            _cachedAddresses = null;
        }

        private async Task<ICollection<PaymentMethod>> GetAllPaymentMethodsViaCacheAsync()
        {
            if (_cachedPaymentMethods == null)
            {
                bool retryWithRefreshedUser = false;
                try
                {
                    _cachedPaymentMethods = await _paymentMethodService.GetPaymentMethodsAsync();
                }
                catch (Exception)
                {
                    retryWithRefreshedUser = true;
                }

                if (retryWithRefreshedUser)
                {
                    await _accountService.VerifyUserAuthenticationAsync();
                    _cachedPaymentMethods = await _paymentMethodService.GetPaymentMethodsAsync();
                }
            }

            return _cachedPaymentMethods;
        }

        private void ExpireCachedPaymentMethods()
        {
            _cachedPaymentMethods = null;
        }
    }
}