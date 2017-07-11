

using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class SignOutFlyoutViewModelFixture
    {
        [TestMethod]
        public void SignOut_CallsSignOutinAccountServiceAndRemovesSavedCredentials()
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

            target.SignOutCommand.Execute();

            Assert.IsTrue(accountServiceSignOutCalled);
            Assert.IsTrue(closeFlyoutCalled);
            Assert.IsTrue(clearHistoryCalled);
            Assert.IsTrue(navigateCalled);
        }
    }
}
