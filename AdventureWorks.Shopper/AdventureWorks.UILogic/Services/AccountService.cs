// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AdventureWorks.UILogic.Models;
using Prism.Windows.AppModel;
using System;
using System.Security;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.Services
{
    public class AccountService : IAccountService
    {
        public const string SignedInUserKey = "AccountService_SignedInUser";
        public const string PasswordVaultResourceName = "AdventureWorksShopper";
        private const string UserNameKey = "AccountService_UserName";
        private const string PasswordKey = "AccountService_Password";
        private readonly IIdentityService _identityService;
        private readonly ISessionStateService _sessionStateService;
        private readonly ICredentialStore _credentialStore;
        private UserInfo _signedInUser;
        private string _userName;
        private string _password;

        public AccountService(IIdentityService identityService, ISessionStateService sessionStateService, ICredentialStore credentialStore)
        {
            _identityService = identityService;
            _sessionStateService = sessionStateService;
            _credentialStore = credentialStore;
            if (_sessionStateService != null)
            {
                if (_sessionStateService.SessionState.ContainsKey(SignedInUserKey))
                {
                    _signedInUser = _sessionStateService.SessionState[SignedInUserKey] as UserInfo;
                }

                if (_sessionStateService.SessionState.ContainsKey(UserNameKey))
                {
                    _userName = _sessionStateService.SessionState[UserNameKey].ToString();
                }

                if (_sessionStateService.SessionState.ContainsKey(PasswordKey))
                {
                    _password = _sessionStateService.SessionState[PasswordKey].ToString();
                }
            }
        }

        public event EventHandler<UserChangedEventArgs> UserChanged;

        public UserInfo SignedInUser
        {
            get
            {
                return _signedInUser;
            }
        }

        /// <summary>
        /// Gets the current active user signed in the app.
        /// </summary>
        /// <returns>A Task that, when complete, retrieves an active user that is ready to be used for any operation against the service.</returns>
        public async Task<UserInfo> VerifyUserAuthenticationAsync()
        {
            try
            {
                // If user is logged in, verify that the session in the service is still active
                if (_signedInUser != null && await _identityService.VerifyActiveSessionAsync(_signedInUser.UserName))
                {
                    return _signedInUser;
                }
            }
            catch (SecurityException)
            {
                // User's session has expired.
            }

            // Attempt to sign in using credentials stored in session state
            // If succeeds, ask for a new active session
            if (_userName != null && _password != null)
            {
                if (await SignInUserAsync(_userName, _password, false))
                {
                    return _signedInUser;
                }
            }

            return await VerifySavedCredentialsAsync();
        }

        public async Task<UserInfo> VerifySavedCredentialsAsync()
        {
            // Attempt to sign in using credentials stored locally
            // If succeeds, ask for a new active session
            var savedCredentials = _credentialStore.GetSavedCredentials(PasswordVaultResourceName);
            if (savedCredentials != null)
            {
                savedCredentials.RetrievePassword();
                if (await SignInUserAsync(savedCredentials.UserName, savedCredentials.Password, false))
                {
                    return _signedInUser;
                }
            }

            return null;
        }

        public async Task<bool> SignInUserAsync(string userName, string password, bool useCredentialStore)
        {
            try
            {
                var result = await _identityService.LogOnAsync(userName, password);
                UserInfo previousUser = _signedInUser;
                _signedInUser = result.UserInfo;

                // Save SignedInUser in the StateService
                _sessionStateService.SessionState[SignedInUserKey] = _signedInUser;

                // Save username and password in state service
                _userName = userName;
                _password = password;
                _sessionStateService.SessionState[UserNameKey] = userName;
                _sessionStateService.SessionState[PasswordKey] = password;
                if (useCredentialStore)
                {
                    // Save credentials in the CredentialStore
                    _credentialStore.SaveCredentials(PasswordVaultResourceName, userName, password);

                    // Documentation on managing application data is at http://go.microsoft.com/fwlink/?LinkID=288818&clcid=0x409
                }

                if (previousUser == null)
                {
                    // Raise use changed event if user logged in
                    RaiseUserChanged(_signedInUser, previousUser);
                }
                else if (_signedInUser != null && _signedInUser.UserName != previousUser.UserName)
                {
                    // Raise use changed event if user changed
                    RaiseUserChanged(_signedInUser, previousUser);
                }

                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        public void SignOut()
        {
            var previousUser = _signedInUser;
            _signedInUser = null;
            _userName = null;
            _password = null;

            _sessionStateService.SessionState.Remove(SignedInUserKey);
            _sessionStateService.SessionState.Remove(UserNameKey);
            _sessionStateService.SessionState.Remove(PasswordKey);

            // remove user from the CredentialStore, if any
            _credentialStore.RemoveSavedCredentials(PasswordVaultResourceName);

            RaiseUserChanged(_signedInUser, previousUser);
        }

        private void RaiseUserChanged(UserInfo newUserInfo, UserInfo oldUserInfo)
        {
            var handler = UserChanged;
            if (handler != null)
            {
                handler(this, new UserChangedEventArgs(newUserInfo, oldUserInfo));
            }
        }
    }
}
