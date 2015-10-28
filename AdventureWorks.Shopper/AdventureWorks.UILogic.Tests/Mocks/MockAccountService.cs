

using System;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Services;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockAccountService : IAccountService
    {
        public Func<string> GetUserIdDelegate { get; set; }

        public Func<string, string, bool,  Task<bool>> SignInUserAsyncDelegate { get; set; }

        public Func<Task<UserInfo>> VerifyUserAuthenticationAsyncDelegate { get; set; }
        public Func<Task<UserInfo>> VerifySavedCredentialsAsyncDelegate { get; set; } 

        public Action SignOutDelegate { get; set; }

        public async Task<UserInfo> VerifySavedCredentialsAsync()
        {
            return await VerifySavedCredentialsAsyncDelegate();
        }

        public async Task<bool> SignInUserAsync(string userName, string password, bool useCredentialStore)
        {
            return await this.SignInUserAsyncDelegate(userName, password, useCredentialStore);
        }

        public UserInfo SignedInUser { get; set; }

        public async Task<UserInfo> VerifyUserAuthenticationAsync()
        {
            return await VerifyUserAuthenticationAsyncDelegate();
        }

        public void RaiseUserChanged(UserInfo newUserInfo, UserInfo oldUserInfo)
        {
            UserChanged(this, new UserChangedEventArgs(newUserInfo, oldUserInfo));
        }

        public event EventHandler<UserChangedEventArgs> UserChanged;


        public string ServerCookieHeader
        {
            get { return string.Empty; }
        }

        public void SignOut()
        {
            SignOutDelegate();
        }
    }
}
