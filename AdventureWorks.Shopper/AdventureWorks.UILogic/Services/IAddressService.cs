// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IAddressService
    {
        Task<ICollection<Address>> GetAddressesAsync();

        Task SaveAddressAsync(Address address);

        Task SetDefault(string defaultAddressId, AddressType addressType);
    }
}
