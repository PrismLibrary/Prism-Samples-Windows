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
