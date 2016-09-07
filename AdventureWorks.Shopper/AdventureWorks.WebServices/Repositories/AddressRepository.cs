

using AdventureWorks.WebServices.Models;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorks.WebServices.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private static Dictionary<string, List<Address>> _addressesDictionary = new Dictionary<string, List<Address>>(); 

        public IEnumerable<Address> GetAll(string userName)
        {
            return _addressesDictionary.ContainsKey(userName) ? _addressesDictionary[userName] : null;
        }

        public void AddUpdate(string userName, Address address)
        {
            if (!_addressesDictionary.ContainsKey(userName))
            {
                _addressesDictionary[userName] = new List<Address>();
            }

            var userAddresses = _addressesDictionary[userName];
            var matchingAddress = userAddresses.Find(a => a.Id == address.Id);
            if (matchingAddress != null)
            {
                userAddresses.Remove(matchingAddress);
            }
            _addressesDictionary[userName].Add(address);
        }

        public void SetDefault(string userName, string defaultAddressId, AddressType addressType)
        {
            var userAddresses = _addressesDictionary[userName];
            
            //Clear old default addresses
            var oldDefaults = userAddresses.Where(a => a.IsDefault && a.AddressType == addressType);
            foreach (var oldDefault in oldDefaults)
            {
                oldDefault.IsDefault = false;
            }

            var defaultAddress = userAddresses.Find(a => a.Id == defaultAddressId);
            if (defaultAddress != null)
            {
                defaultAddress.IsDefault = true;
            }
        }

        public static void Reset()
        {
            _addressesDictionary = new Dictionary<string, List<Address>>();
        }
    }
}