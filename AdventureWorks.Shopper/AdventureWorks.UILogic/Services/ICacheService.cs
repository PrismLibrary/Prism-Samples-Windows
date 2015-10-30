using System.Threading.Tasks;

namespace AdventureWorks.UILogic.Services
{
    public interface ICacheService
    {
        Task<T> GetDataAsync<T>(string cacheKey);

        Task SaveDataAsync<T>(string cacheKey, T content);
    }
}
