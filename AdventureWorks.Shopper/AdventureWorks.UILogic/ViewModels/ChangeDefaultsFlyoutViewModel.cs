using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Windows.AppModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdventureWorks.UILogic.ViewModels
{
    public class ChangeDefaultsFlyoutViewModel : BindableBase, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IAccountService _accountService;
        private CheckoutDataViewModel _selectedShippingAddress;
        private CheckoutDataViewModel _selectedBillingAddress;
        private CheckoutDataViewModel _selectedPaymentMethod;
        private Action _closeFlyout;

        public ChangeDefaultsFlyoutViewModel(ICheckoutDataRepository checkoutDataRepository, IResourceLoader resourceLoader, IAlertMessageService alertMessageService, IAccountService accountService)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _resourceLoader = resourceLoader;
            _alertMessageService = alertMessageService;
            _accountService = accountService;

            SaveCommand = new DelegateCommand(async () => SaveAsync);

            Initialize();
        }

        public IReadOnlyCollection<CheckoutDataViewModel> PaymentMethods { get; private set; }

        public IReadOnlyCollection<CheckoutDataViewModel> ShippingAddresses { get; private set; }

        public IReadOnlyCollection<CheckoutDataViewModel> BillingAddresses { get; private set; }

        public Action CloseFlyout
        {
            get { return _closeFlyout; }
            set { SetProperty(ref _closeFlyout, value); }
        }

        public ICommand SaveCommand { get; private set; }

        public CheckoutDataViewModel SelectedShippingAddress
        {
            get { return _selectedShippingAddress; }
            set { SetProperty(ref _selectedShippingAddress, value); }
        }

        public CheckoutDataViewModel SelectedBillingAddress
        {
            get { return _selectedBillingAddress; }
            set { SetProperty(ref _selectedBillingAddress, value); }
        }

        public CheckoutDataViewModel SelectedPaymentMethod
        {
            get { return _selectedPaymentMethod; }
            set { SetProperty(ref _selectedPaymentMethod, value); }
        }

        private async void Initialize()
        {
            if (await _accountService.VerifyUserAuthenticationAsync() == null)
            {
                return;
            }

            // Populate ShippingAddress collection
            var shippingAddresses = (await _checkoutDataRepository.GetAllShippingAddressesAsync()).Select(address => CreateCheckoutData(address, Constants.ShippingAddress));
            ShippingAddresses = new ReadOnlyCollection<CheckoutDataViewModel>(shippingAddresses.ToList());

            if (ShippingAddresses != null)
            {
                var defaultShippingAddress = await _checkoutDataRepository.GetDefaultShippingAddressAsync();
                var selectedShippingAddress = defaultShippingAddress != null ? ShippingAddresses.FirstOrDefault(s => s.EntityId == defaultShippingAddress.Id) : null;
                SetProperty(ref _selectedShippingAddress, selectedShippingAddress, "SelectedShippingAddress");
            }

            // Populate BillingAddress collection
            var billingAddresses = (await _checkoutDataRepository.GetAllBillingAddressesAsync()).Select(address => CreateCheckoutData(address, Constants.BillingAddress));
            BillingAddresses = new ReadOnlyCollection<CheckoutDataViewModel>(billingAddresses.ToList());

            if (BillingAddresses != null)
            {
                var defaultBillingAddress = await _checkoutDataRepository.GetDefaultBillingAddressAsync();
                var selectedBillingAddress = defaultBillingAddress != null ? BillingAddresses.FirstOrDefault(s => s.EntityId == defaultBillingAddress.Id) : null;
                SetProperty(ref _selectedBillingAddress, selectedBillingAddress, "SelectedBillingAddress");
            }

            // Populate PaymentMethod collection
            var paymentMethods = (await _checkoutDataRepository.GetAllPaymentMethodsAsync()).Select(CreateCheckoutData);
            PaymentMethods = new ReadOnlyCollection<CheckoutDataViewModel>(paymentMethods.ToList());

            if (PaymentMethods != null)
            {
                var defaultPaymentMethod = await _checkoutDataRepository.GetDefaultPaymentMethodAsync();
                var selectedPaymentMethod = defaultPaymentMethod != null ? PaymentMethods.FirstOrDefault(s => s.EntityId == defaultPaymentMethod.Id) : null;
                SetProperty(ref _selectedPaymentMethod, selectedPaymentMethod, "SelectedPaymentMethod");
            }
        }

        private async Task SaveAsync()
        {
            string errorMessage = string.Empty;

            try
            {
                if (_selectedShippingAddress != null)
                {
                    await _checkoutDataRepository.SetDefaultShippingAddressAsync(_selectedShippingAddress.EntityId);
                }
                else
                {
                    await _checkoutDataRepository.RemoveDefaultShippingAddressAsync();
                }

                if (_selectedBillingAddress != null)
                {
                    await _checkoutDataRepository.SetDefaultBillingAddressAsync(_selectedBillingAddress.EntityId);
                }
                else
                {
                    await _checkoutDataRepository.RemoveDefaultBillingAddressAsync();
                }

                if (_selectedPaymentMethod != null)
                {
                    await _checkoutDataRepository.SetDefaultPaymentMethodAsync(_selectedPaymentMethod.EntityId);
                }
                else
                {
                    await _checkoutDataRepository.RemoveDefaultPaymentMethodAsync();
                }

                CloseFlyout();
            }
            catch (Exception ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
            }
        }

        private CheckoutDataViewModel CreateCheckoutData(Address address, string dataType)
        {
            return new CheckoutDataViewModel()
            {
                EntityId = address.Id,
                DataType = dataType,
                Title = dataType == Constants.ShippingAddress ? _resourceLoader.GetString("ShippingAddress") : _resourceLoader.GetString("BillingAddress"),
                FirstLine = address.StreetAddress,
                SecondLine = string.Format(CultureInfo.CurrentCulture, "{0}, {1} {2}", address.City, address.State, address.ZipCode),
                BottomLine = string.Format(CultureInfo.CurrentCulture, "{0} {1}", address.FirstName, address.LastName),
                LogoUri = dataType == Constants.ShippingAddress ? new Uri(Constants.ShippingAddressLogo, UriKind.Absolute) : new Uri(Constants.BillingAddressLogo, UriKind.Absolute)
            };
        }

        private CheckoutDataViewModel CreateCheckoutData(PaymentMethod paymentMethod)
        {
            return new CheckoutDataViewModel()
            {
                EntityId = paymentMethod.Id,
                DataType = Constants.PaymentMethod,
                Title = _resourceLoader.GetString("PaymentMethod"),
                FirstLine = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("CardEndingIn"), paymentMethod.CardNumber.Substring(paymentMethod.CardNumber.Length - 4)),
                SecondLine = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("CardExpiringOn"), string.Format(CultureInfo.CurrentCulture, "{0}/{1}", paymentMethod.ExpirationMonth, paymentMethod.ExpirationYear)),
                BottomLine = paymentMethod.CardholderName,
                LogoUri = new Uri(Constants.PaymentMethodLogo, UriKind.Absolute)
            };
        }
    }
}