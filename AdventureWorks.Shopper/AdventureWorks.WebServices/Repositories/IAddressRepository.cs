

using System.Collections.Generic;
using AdventureWorks.WebServices.Models;

namespace AdventureWorks.WebServices.Repositories
{
    public interface IAddressRepository
    {
        IEnumerable<Address> GetAll(string userName);
        void AddUpdate(string userName, Address address);
        void SetDefault(string userName, string defaultAddressId, AddressType addressType);
    }
}
