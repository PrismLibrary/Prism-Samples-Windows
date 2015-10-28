using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.Services
{
    public interface ILocationService
    {
        Task<IReadOnlyCollection<string>> GetStatesAsync();
    }
}
