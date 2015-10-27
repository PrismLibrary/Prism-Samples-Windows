// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class SignOutFlyoutViewModelFixture
    {
        [TestMethod]
        public async Task SignOut_CallsSignOutinAccountServiceAndRemovesSavedCredentials()
        {
            bool closeFlyoutCalled = false;
            bool accountServiceSignOutCalled = false;
            bool clearHistoryCalled = false;
            bool navigateCalled = false;

            var accountService = new MockAccountService
                {
                    SignOutDelegate = () => accountServiceSignOutCalled = true,
                    VerifyUserAuthenticationAsyncDelegate = () => Task.FromResult(new UserInfo())
                };

            var navigationService = new MockNavigationService
                {
                    ClearHistoryDelegate = () => clearHistoryCalled = true,
                    NavigateDelegate = (s, o) =>
                        {
                            navigateCalled = true;
                            Assert.AreEqual("Hub", s);
                            return true;
                        }
                };

            var target = new SignOutFlyoutViewModel(accountService, navigationService) { CloseFlyout = () => closeFlyoutCalled = true };

            await target.SignOutCommand.Execute();

            Assert.IsTrue(accountServiceSignOutCalled);
            Assert.IsTrue(closeFlyoutCalled);
            Assert.IsTrue(clearHistoryCalled);
            Assert.IsTrue(navigateCalled);
        }
    }
}
