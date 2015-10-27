// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace AdventureWorks.UILogic.Services
{
    public interface ICacheService
    {
        Task<T> GetDataAsync<T>(string cacheKey);

        Task SaveDataAsync<T>(string cacheKey, T content);
    }
}
