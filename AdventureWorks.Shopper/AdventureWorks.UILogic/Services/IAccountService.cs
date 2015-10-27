// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IAccountService
    {
        event EventHandler<UserChangedEventArgs> UserChanged;

        UserInfo SignedInUser { get; }

        Task<UserInfo> VerifyUserAuthenticationAsync();

        Task<UserInfo> VerifySavedCredentialsAsync();

        Task<bool> SignInUserAsync(string userName, string password, bool useCredentialStore);
        
        void SignOut();
    }
}
