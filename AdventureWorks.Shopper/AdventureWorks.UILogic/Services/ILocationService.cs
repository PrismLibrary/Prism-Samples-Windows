// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.Services
{
    public interface ILocationService
    {
        Task<IReadOnlyCollection<string>> GetStatesAsync();
    }
}
