

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockAddressService : IAddressService
    {
        public MockAddressService()
        {
            Addresses = new List<Address>();
        }

        public List<Address> Addresses { get; set; }
        
        public void SaveEntity(Address address)
        {
            Addresses.Add(address);
        }

        public Task<ICollection<Address>> GetAddressesAsync()
        {
            return Task.FromResult<ICollection<Address>>(Addresses);
        }

        public Task SaveAddressAsync(Address address)
        {
            var matchingAddress = Addresses.FirstOrDefault(a => a.Id == address.Id && a.AddressType == address.AddressType);
            if (matchingAddress != null)
            {
                Addresses.Remove(matchingAddress);
            }
            Addresses.Add(address);
            return Task.Delay(0);
        }

        public Task SetDefault(string defaultAddressId, AddressType addressType)
        {
            var matchingAddress = Addresses.FirstOrDefault(a => a.Id == defaultAddressId && a.AddressType == addressType);
            matchingAddress.IsDefault = true;
            return Task.Delay(0);
        }
    }
}
