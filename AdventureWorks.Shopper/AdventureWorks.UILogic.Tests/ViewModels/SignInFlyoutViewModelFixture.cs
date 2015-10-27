// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class SignInFlyoutViewModelFixture
    {
        [TestMethod]
        public async Task FiringSignInCommand_Persists_Credentials_And_Turns_Invisible()
        {
            bool accountServiceSignInCalled = false;
            bool flyoutClosed = false;

            var accountService = new MockAccountService()
                {
                    SignInUserAsyncDelegate = (username, password, useCredentialStore) =>
                        {
                            Assert.AreEqual("TestUsername", username);
                            Assert.AreEqual("TestPassword", password);
                            Assert.IsTrue(useCredentialStore);
                            accountServiceSignInCalled = true;
                            return Task.FromResult(true);
                        }
                };

            var target = new SignInFlyoutViewModel(accountService, null, null)
                {
                    CloseFlyout = () => flyoutClosed = true,
                    UserName = "TestUsername",
                    Password = "TestPassword",
                    SaveCredentials = true
                };

            await target.SignInCommand.Execute();

            Assert.IsTrue(accountServiceSignInCalled);
            Assert.IsTrue(flyoutClosed);
        }

        [TestMethod]
        public async Task FiringSignInCommand_WithNotRememberPassword_DoesNotSaveInCredentialStore()
        {
            var accountService = new MockAccountService()
                {
                    SignInUserAsyncDelegate = (username, password, useCredentialStore) =>
                        {
                            Assert.IsFalse(useCredentialStore);
                            return Task.FromResult(true);
                        }
                };

            var target = new SignInFlyoutViewModel(accountService, null, null)
                {
                    CloseFlyout = () => Assert.IsTrue(true),
                    SaveCredentials = false
                };

            await target.SignInCommand.Execute();
        }

        [TestMethod]
        public void UserName_ReturnsLastSignedInUser_IfAvailable()
        {
            var accountService = new MockAccountService()
                {
                    SignedInUser = new UserInfo { UserName = "TestUserName" }
                };

            var target = new SignInFlyoutViewModel(accountService, null, null);

            Assert.AreEqual("TestUserName", target.UserName);
            Assert.IsFalse(target.IsNewSignIn);
        }
    }
}
