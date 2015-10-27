// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AdventureWorks.UILogic.Services;
using AdventureWorks.UILogic.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using AdventureWorks.UILogic.Models;
using Windows.Security.Credentials;
using System;

namespace AdventureWorks.UILogic.Tests.Services
{
    [TestClass]
    public class AccountServiceFixture
    {
        [TestMethod]
        public async Task SuccessfullSignIn_RaisesUserChangedEvent()
        {
            bool userChangedFired = false;

            var identityService = new MockIdentityService();
            var sessionStateService = new MockSessionStateService();
            identityService.LogOnAsyncDelegate = (userId, password) =>
                {
                    return Task.FromResult(new LogOnResult { UserInfo = new UserInfo{UserName = userId} });
                };

            var target = new AccountService(identityService, sessionStateService, null);
            target.UserChanged += (sender, userInfo) => { userChangedFired = true; }; 

            var retVal = await target.SignInUserAsync("TestUserName", "TestPassword", false);
            Assert.IsTrue(retVal);
            Assert.IsTrue(userChangedFired);
        }

        [TestMethod]
        public async Task GetSignedInUserAsync_Calls_VerifyActiveSessionAsync()
        {
            bool verifyActiveSessionCalled = false;
            var sessionStateService = new MockSessionStateService();
            var identityService = new MockIdentityService()
                {
                    LogOnAsyncDelegate = (userId, password) => Task.FromResult(new LogOnResult { UserInfo = new UserInfo { UserName = userId } }),
                    VerifyActiveSessionDelegate = (userName) =>
                        {
                            verifyActiveSessionCalled = true;
                            return Task.FromResult(true);
                        }
                };

            var target = new AccountService(identityService, sessionStateService, null);
            await target.SignInUserAsync("TestUserName", "TestPassword", false);
            var user = await target.VerifyUserAuthenticationAsync();

            Assert.IsTrue(verifyActiveSessionCalled);
            Assert.IsNotNull(user);
            Assert.IsTrue(user.UserName == "TestUserName");
        }

        [TestMethod]
        public async Task GetSignedInUserAsync_SignsInUsingCredentialStore_IfNoActiveSession()
        {
            var sessionStateService = new MockSessionStateService();
            var identityService = new MockIdentityService()
            {
                LogOnAsyncDelegate = (userId, password) => Task.FromResult(new LogOnResult { UserInfo = new UserInfo { UserName = userId } }),
                VerifyActiveSessionDelegate = (userName) => Task.FromResult(false)
            };
            var credentialStore = new MockCredentialStore()
                {
                    GetSavedCredentialsDelegate = (s) => new PasswordCredential(AccountService.PasswordVaultResourceName, "TestUserName", "TestPassword"),
                    SaveCredentialsDelegate = (a, b, c) => Task.Delay(0)
                };

            var target = new AccountService(identityService, sessionStateService, credentialStore);
            await target.SignInUserAsync("TestUserName", "TestPassword", true);

            var user = await target.VerifyUserAuthenticationAsync();

            Assert.IsNotNull(user);
            Assert.IsTrue(user.UserName == "TestUserName");
        }

        [TestMethod]
        public async Task FailedSignIn_DoesNotRaiseUserChangedEvent()
        {
            bool userChangedFired = false;

            var identityService = new MockIdentityService();
            var sessionStateService = new MockSessionStateService();
            identityService.LogOnAsyncDelegate = (userId, password) =>
            {
                throw new Exception();
            };

            var target = new AccountService(identityService, sessionStateService, null);
            target.UserChanged += (sender, userInfo) => { userChangedFired = true; };

            var retVal = await target.SignInUserAsync("TestUserName", "TestPassword", false);
            Assert.IsFalse(retVal);
            Assert.IsFalse(userChangedFired);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsUserInfo_IfSessionIsStillLive()
        {
            var sessionStateService = new MockSessionStateService();
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName) => Task.FromResult(true);
            identityService.LogOnAsyncDelegate = (userName, password) =>
                {
                    return Task.FromResult(new LogOnResult()
                        {
                            UserInfo = new UserInfo() {UserName = "TestUsername"}
                        });
                };

            var target = new AccountService(identityService, sessionStateService, null);
            bool userSignedIn = await target.SignInUserAsync("TestUsername", "password", false);

            Assert.IsTrue(userSignedIn);

            var userInfo = await target.VerifyUserAuthenticationAsync();

            Assert.IsNotNull(userInfo);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsNull_IfSessionIsStillInactiveAndNoSavedCredentials()
        {
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName) => Task.FromResult(false);
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s => null;
            var sessionStateService = new MockSessionStateService();
            var target = new AccountService(identityService, sessionStateService, credentialStore);

            var userInfo = await target.VerifyUserAuthenticationAsync(); 
            
            Assert.IsNull(userInfo);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsUserInfo_IfSessionIsStillInactiveButHasSavedCredentials()
        {
            var sessionStateService = new MockSessionStateService();
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName) => Task.FromResult(false);
            identityService.LogOnAsyncDelegate =
                (userName, password) =>
                    {
                        Assert.AreEqual("TestUserName", userName);
                        Assert.AreEqual("TestPassword", password);
                        return Task.FromResult(new LogOnResult()
                                            {
                                                UserInfo = new UserInfo(){UserName = "ReturnedUserName"}
                                            });
                    };
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s => new PasswordCredential(AccountService.PasswordVaultResourceName, "TestUserName", "TestPassword");
            var target = new AccountService(identityService, sessionStateService, credentialStore);

            var userInfo = await target.VerifyUserAuthenticationAsync();

            Assert.IsNotNull(userInfo);
            Assert.AreEqual("ReturnedUserName", userInfo.UserName);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsNull_IfSessionIsStillInactiveAndHasInvalidSavedCredentials()
        {
            var sessionStateService = new MockSessionStateService();
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName) => Task.FromResult(false);
            identityService.LogOnAsyncDelegate =
                (userName, password) =>
                {
                    Assert.AreEqual("TestUserName", userName);
                    Assert.AreEqual("TestPassword", password);
                    throw new Exception();
                };
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s => new PasswordCredential(AccountService.PasswordVaultResourceName, "TestUserName", "TestPassword");
            var target = new AccountService(identityService, sessionStateService, credentialStore);

            var userInfo = await target.VerifyUserAuthenticationAsync();

            Assert.IsNull(userInfo);
        }

        [TestMethod]
        public async Task SignOut_RaisesUserChangedEvent()
        {
            bool userChangedRaised = false;
            var sessionStateService = new MockSessionStateService();
            var credentialStore = new MockCredentialStore
                {
                    GetSavedCredentialsDelegate = s => null,
                    RemoveSavedCredentialsDelegate = s => Task.Delay(0)
                };

            var target = new AccountService(null, sessionStateService, credentialStore);
            target.UserChanged += (sender, args) =>
                {
                    userChangedRaised = true;
                    Assert.IsNull(args.NewUserInfo);
                };

            target.SignOut();

            Assert.IsTrue(userChangedRaised);

            var signedInUser = await target.VerifyUserAuthenticationAsync();

            Assert.IsNull(signedInUser);
        }
    }
}
