// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Windows.Security.Credentials;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockCredentialStore : ICredentialStore
    {
        public Action<string, string, string> SaveCredentialsDelegate { get; set; }
        public Func<string, PasswordCredential> GetSavedCredentialsDelegate { get; set; }
        public Action<string> RemoveSavedCredentialsDelegate { get; set; }

        public void SaveCredentials(string resource, string userName, string password)
        {
            SaveCredentialsDelegate(resource, userName, password);
        }

        public PasswordCredential GetSavedCredentials(string resource)
        {
            return GetSavedCredentialsDelegate(resource);
        }

        public void RemoveSavedCredentials(string resource)
        {
            RemoveSavedCredentialsDelegate(resource);
        }
    }
}
