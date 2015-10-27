// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AdventureWorks.UILogic.Services;
using System;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockIdentityService : IIdentityService
    {
        public Func<string, string, Task<LogOnResult>> LogOnAsyncDelegate { get; set; }
        public Func<string, Task<bool>> VerifyActiveSessionDelegate { get; set; }
 
        public Task<LogOnResult> LogOnAsync(string userId, string password)
        {
            return LogOnAsyncDelegate(userId, password);
        }

        public Task<bool> VerifyActiveSessionAsync(string userId)
        {
            return VerifyActiveSessionDelegate(userId);
        }
    }
}
