

using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventureWorks.UILogic.Tests.Repositories
{
    [TestClass]
    public class CheckoutDataRepositoryFixture
    {
        [TestMethod]
        public async Task GetEntity_Returns_Entity()
        {
            var target = new CheckoutDataRepository(SetupAddressService(), SetupPaymentMethodService(), null);

            var shippingAddress = await target.GetShippingAddressAsync("3");
            var bilingAddress = await target.GetBillingAddressAsync("2");
            var paymentMethod = await target.GetPaymentMethodAsync("1");

            Assert.AreEqual(shippingAddress.FirstName, "Anne");
            Assert.AreEqual(bilingAddress.FirstName, "Jane");
            Assert.AreEqual(paymentMethod.CardholderName, "John Doe");
        }

        [TestMethod]
        public async Task GetDefaultValues_Returns_DefaultValues()
        {
            var target = new CheckoutDataRepository(SetupAddressService(), SetupPaymentMethodService(), null);

            var defaultShippingAddress = await target.GetDefaultShippingAddressAsync();
            var defaultBilingAddress = await target.GetDefaultBillingAddressAsync();
            var defaultPaymentMethod = await target.GetDefaultPaymentMethodAsync();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.AreEqual(defaultShippingAddress.Id, "3");
            Assert.IsNotNull(defaultBilingAddress);
            Assert.AreEqual(defaultBilingAddress.Id, "2");
            Assert.IsNull(defaultPaymentMethod);
        }

        [TestMethod]
        public async Task GetAllEntities_Returns_AllEntities()
        {
            var target = new CheckoutDataRepository(SetupAddressService(), SetupPaymentMethodService(), null);

            var shippingAddresses = await target.GetAllShippingAddressesAsync();
            var bilingAddresses = await target.GetAllBillingAddressesAsync();
            var paymentMethods = await target.GetAllPaymentMethodsAsync();

            Assert.AreEqual(3, shippingAddresses.Count());
            Assert.AreEqual(2, bilingAddresses.Count());
            Assert.AreEqual(1, paymentMethods.Count());
        }

        [TestMethod]
        public async Task SaveEntity_SavesEntity()
        {
            var target = new CheckoutDataRepository(SetupAddressService(), SetupPaymentMethodService(), null);

            await target.SaveShippingAddressAsync(new Address() { Id="test", FirstName = "TestFirstName", LastName = "TestLastName", AddressType = AddressType.Shipping});
            await target.SaveBillingAddressAsync(new Address() { Id = "test", FirstName = "TestFirstName", LastName = "TestLastName", AddressType = AddressType.Billing });
            await target.SavePaymentMethodAsync(new PaymentMethod() { Id = "test", CardNumber = "12345", CardVerificationCode = "1234", ExpirationMonth = "10", ExpirationYear = "2010", CardholderName = "TestCardholderName" });

            var savedShippingAddress = await target.GetShippingAddressAsync("test");
            var savedBillingAddress = await target.GetBillingAddressAsync("test");
            var savedPaymentMethod = await target.GetPaymentMethodAsync("test");

            Assert.IsNotNull(savedShippingAddress);
            Assert.IsNotNull(savedBillingAddress);
            Assert.IsNotNull(savedPaymentMethod);

            var shippingAddress = await target.GetShippingAddressAsync(savedShippingAddress.Id);
            var billingAddress = await target.GetBillingAddressAsync(savedBillingAddress.Id);
            var paymentMethod = await target.GetPaymentMethodAsync(savedPaymentMethod.Id);
            
            Assert.AreEqual(savedShippingAddress.Id, shippingAddress.Id);
            Assert.AreEqual(savedBillingAddress.Id, billingAddress.Id);
            Assert.AreEqual(savedPaymentMethod.Id, paymentMethod.Id);
        }

        [TestMethod]
        public async Task SetDefaultEntity_SetsDefaultEntity()
        {
            var target = new CheckoutDataRepository(SetupAddressService(), SetupPaymentMethodService(), null);

            var defaultShippingAddress = await target.GetDefaultShippingAddressAsync();
            var defaultBillingAddress = await target.GetDefaultBillingAddressAsync();
            var defaultPaymentMethod = await target.GetDefaultPaymentMethodAsync();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.AreEqual(defaultShippingAddress.Id, "3");
            Assert.IsNotNull(defaultBillingAddress);
            Assert.AreEqual(defaultBillingAddress.Id, "2");
            Assert.IsNull(defaultPaymentMethod);

            await target.SetDefaultShippingAddressAsync("2");
            await target.SetDefaultBillingAddressAsync("1");
            await target.SetDefaultPaymentMethodAsync("1");

            defaultShippingAddress = await target.GetDefaultShippingAddressAsync();
            defaultBillingAddress = await target.GetDefaultBillingAddressAsync();
            defaultPaymentMethod = await target.GetDefaultPaymentMethodAsync();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.AreEqual(defaultShippingAddress.Id, "2");
            Assert.IsNotNull(defaultBillingAddress);
            Assert.AreEqual(defaultBillingAddress.Id, "1");
            Assert.IsNotNull(defaultPaymentMethod);
            Assert.AreEqual(defaultPaymentMethod.Id, "1");
        }

        [TestMethod]
        public async Task RemoveDefaultEntity_RemovesDefaultEntity()
        {
            var target = new CheckoutDataRepository(SetupAddressService(), SetupPaymentMethodService(), null);
            await target.SetDefaultPaymentMethodAsync("1");

            var defaultShippingAddress = await target.GetDefaultShippingAddressAsync();
            var defaultBillingAddress = await target.GetDefaultBillingAddressAsync();
            var defaultPaymentMethod = await target.GetDefaultPaymentMethodAsync();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.IsNotNull(defaultBillingAddress);
            Assert.IsNotNull(defaultPaymentMethod);

            await target.RemoveDefaultShippingAddressAsync();
            await target.RemoveDefaultBillingAddressAsync();
            await target.RemoveDefaultPaymentMethodAsync();

            defaultShippingAddress = await target.GetDefaultShippingAddressAsync();
            defaultBillingAddress = await target.GetDefaultBillingAddressAsync();
            defaultPaymentMethod = await target.GetDefaultPaymentMethodAsync();

            Assert.IsNull(defaultShippingAddress);
            Assert.IsNull(defaultBillingAddress);
            Assert.IsNull(defaultPaymentMethod);
        }

        [TestMethod]
        public async Task GetAllPaymentMethodsAsync_ReturnsEmptyCollection_WhenServiceReturnsNull()
        {
            var paymentMethodService = new MockPaymentMethodService();
            paymentMethodService.PaymentMethods = null;

            var target = new CheckoutDataRepository(null, paymentMethodService, null);

            var paymentMethods = await target.GetAllPaymentMethodsAsync();

            Assert.IsNotNull(paymentMethods);
            Assert.AreEqual(0, paymentMethods.Count);
        }

        [TestMethod]
        public async Task CachedAddressesAndPaymentMethodsExpire_WhenUserChanged()
        {
            var accountService = new MockAccountService();
            var target = new CheckoutDataRepository(SetupAddressService(), SetupPaymentMethodService(), accountService);

            var paymentMethods = await target.GetAllPaymentMethodsAsync();

            Assert.AreSame(await target.GetAllPaymentMethodsAsync(), paymentMethods, "Cached data should be same.");

            accountService.RaiseUserChanged(null, null);

            Assert.AreNotSame(await target.GetAllPaymentMethodsAsync(), paymentMethods);
        }

        private static MockAddressService SetupAddressService()
        {
            var service = new MockAddressService();

            service.SaveEntity(new Address() { AddressType = AddressType.Shipping, Id = "1", FirstName = "Bill", MiddleInitial = "B", LastName = "Doe", City = "Redmond", State = "Washington" });
            service.SaveEntity(new Address() { AddressType = AddressType.Shipping, Id = "2", FirstName = "Jack", MiddleInitial = "B", LastName = "Doe", City = "Redmond", State = "Washington" });
            service.SaveEntity(new Address() { AddressType = AddressType.Shipping, Id = "3", FirstName = "Anne", MiddleInitial = "B", LastName = "Doe", City = "Redmond", State = "Washington", IsDefault = true});

            service.SaveEntity(new Address() { AddressType = AddressType.Billing, Id = "1", FirstName = "John", MiddleInitial = "B", LastName = "Doe", City = "Redmond", State = "Washington" });
            service.SaveEntity(new Address() { AddressType = AddressType.Billing, Id = "2", FirstName = "Jane", MiddleInitial = "B", LastName = "Doe", City = "Redmond", State = "Washington", IsDefault = true});

            return service;
        }

        private static MockPaymentMethodService SetupPaymentMethodService()
        {
            var service = new MockPaymentMethodService();
            service.SaveEntity(new PaymentMethod() { Id = "1", CardholderName = "John Doe", CardNumber = "123512523123", CardVerificationCode = "123" });

            return service;
        }
    }
}
