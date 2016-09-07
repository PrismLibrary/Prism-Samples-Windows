using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Windows.AppModel;
using System;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.ViewModels
{
    public class SignInFlyoutViewModel : BindableBase, IFlyoutViewModel
    {
        private const int Error401 = -2145844847; // BG_E_HTTP_ERROR_401 (0x80190191)

        private readonly IAccountService _accountService;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private readonly UserInfo _lastSignedInUser;
        private string _userName;
        private string _password;
        private bool _saveCredentials;
        private bool _isSignInInvalid;
        private Action _successAction;
        private Action _closeFlyout;

        public SignInFlyoutViewModel(IAccountService accountService, IAlertMessageService alertMessageService, IResourceLoader resourceLoader)
        {
            _accountService = accountService;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
            if (accountService != null)
            {
                _lastSignedInUser = _accountService.SignedInUser;
            }

            SignInCommand = DelegateCommand.FromAsyncHandler(SignInAsync, CanSignIn);
        }

        public string UserName
        {
            get
            {
                if (!IsNewSignIn)
                {
                    return _lastSignedInUser.UserName;
                }

                return _userName;
            }

            set
            {
                if (SetProperty(ref _userName, value))
                {
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsNewSignIn
        {
            get { return _lastSignedInUser == null; }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                if (SetProperty(ref _password, value))
                {
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool SaveCredentials
        {
            get { return _saveCredentials; }
            set { SetProperty(ref _saveCredentials, value); }
        }

        public bool IsSignInInvalid
        {
            get { return _isSignInInvalid; }
            private set { SetProperty(ref _isSignInInvalid, value); }
        }

        public Action CloseFlyout
        {
            get { return _closeFlyout; }
            set { SetProperty(ref _closeFlyout, value); }
        }

        public DelegateCommand SignInCommand { get; private set; }

        public int Error { get; set; }

        public bool CanSignIn()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }

        public async Task SignInAsync()
        {
            var signinCallFailed = false;
            var signinSuccessfull = false;
            try
            {
                signinSuccessfull = await _accountService.SignInUserAsync(UserName, Password, SaveCredentials);
            }
            catch (Exception ex)
            {
                if (ex.HResult != Error401)
                {
                    signinCallFailed = true;
                }
            }

            if (signinCallFailed)
            {
                await _alertMessageService.ShowAsync(_resourceLoader.GetString("ErrorServiceUnreachable"), _resourceLoader.GetString("Error"));
                return;
            }

            if (signinSuccessfull)
            {
                IsSignInInvalid = false;

                if (_successAction != null)
                {
                    _successAction();
                    _successAction = null;
                }

                CloseFlyout();
            }
            else
            {
                IsSignInInvalid = true;
            }
        }
    }
}
