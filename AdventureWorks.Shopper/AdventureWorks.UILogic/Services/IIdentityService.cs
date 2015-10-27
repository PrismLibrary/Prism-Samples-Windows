// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IIdentityService
    {
        Task<LogOnResult> LogOnAsync(string userId, string password);

        Task<bool> VerifyActiveSessionAsync(string userId);
    }
}
