// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(Order order);

        Task ProcessOrderAsync(Order order);
    }
}
