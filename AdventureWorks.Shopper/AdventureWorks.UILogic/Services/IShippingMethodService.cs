using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IShippingMethodService
    {
        Task<IEnumerable<ShippingMethod>> GetShippingMethodsAsync();

        Task<ShippingMethod> GetBasicShippingMethodAsync();
    }
}
