using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Prism.Commands;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public class CheckoutSummaryPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository;
        private readonly IShippingMethodService _shippingMethodService;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IAccountService _accountService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAlertMessageService _alertMessageService;
        private readonly ISignInUserControlViewModel _signInUserControlViewModel;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private Order _order;
        private string _orderSubtotal;
        private string _shippingCost;
        private string _taxCost;
        private string _grandTotal;
        private bool _isBottomAppBarOpened;
        private string _selectCheckoutDataTypeLabel;
        private IReadOnlyCollection<ShoppingCartItemViewModel> _shoppingCartItemViewModels;
        private IReadOnlyCollection<ShippingMethod> _shippingMethods;
        private ObservableCollection<CheckoutDataViewModel> _checkoutDataViewModels;
        private IReadOnlyCollection<CheckoutDataViewModel> _allCheckoutDataViewModels;
        private ShippingMethod _selectedShippingMethod;
        private CheckoutDataViewModel _selectedCheckoutData;
        private CheckoutDataViewModel _selectedAllCheckoutData;

        public CheckoutSummaryPageViewModel(INavigationService navigationService,
            IOrderService orderService,
            IOrderRepository orderRepository,
            IShippingMethodService shippingMethodService,
            ICheckoutDataRepository checkoutDataRepository,
            IShoppingCartRepository shoppingCartRepository,
            IAccountService accountService,
            IResourceLoader resourceLoader,
            IAlertMessageService alertMessageService,
            ISignInUserControlViewModel signInUserControlViewModel)
        {
            _navigationService = navigationService;
            _orderService = orderService;
            _orderRepository = orderRepository;
            _shippingMethodService = shippingMethodService;
            _checkoutDataRepository = checkoutDataRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _resourceLoader = resourceLoader;
            _accountService = accountService;
            _alertMessageService = alertMessageService;
            _signInUserControlViewModel = signInUserControlViewModel;

            SubmitCommand =  new DelegateCommand(async () => await SubmitAsync(), CanSubmit);

            EditCheckoutDataCommand = new DelegateCommand(EditCheckoutData);
            SelectCheckoutDataCommand = new DelegateCommand(async () => await SelectCheckoutDataAsync());
            AddCheckoutDataCommand = new DelegateCommand(AddCheckoutData);
        }

        public string OrderSubtotal
        {
            get { return _orderSubtotal; }
            private set { SetProperty(ref _orderSubtotal, value); }
        }

        public string ShippingCost
        {
            get { return _shippingCost; }
            private set { SetProperty(ref _shippingCost, value); }
        }

        public string TaxCost
        {
            get { return _taxCost; }
            private set { SetProperty(ref _taxCost, value); }
        }

        public string GrandTotal
        {
            get { return _grandTotal; }
            private set { SetProperty(ref _grandTotal, value); }
        }

        public bool IsBottomAppBarOpened
        {
            get
            {
                return _isBottomAppBarOpened;
            }

            set
            {
                // We always fire the PropertyChanged event because the 
                // AppBar.IsOpen property doesn't notify when the property is set.
                // See http://go.microsoft.com/fwlink/?LinkID=288840
                _isBottomAppBarOpened = value;
                RaisePropertyChanged(nameof(IsBottomAppBarOpened));
            }
        }

        public string SelectCheckoutDataLabel
        {
            get { return _selectCheckoutDataTypeLabel; }
            set { SetProperty(ref _selectCheckoutDataTypeLabel, value); }
        }

        public IReadOnlyCollection<ShoppingCartItemViewModel> ShoppingCartItemViewModels
        {
            get { return _shoppingCartItemViewModels; }
            private set { SetProperty(ref _shoppingCartItemViewModels, value); }
        }

        public ObservableCollection<CheckoutDataViewModel> CheckoutDataViewModels
        {
            get { return _checkoutDataViewModels; }
            private set { SetProperty(ref _checkoutDataViewModels, value); }
        }

        public IReadOnlyCollection<CheckoutDataViewModel> AllCheckoutDataViewModels
        {
            get { return _allCheckoutDataViewModels; }
            private set { SetProperty(ref _allCheckoutDataViewModels, value); }
        }

        public IReadOnlyCollection<ShippingMethod> ShippingMethods
        {
            get { return _shippingMethods; }
            private set { SetProperty(ref _shippingMethods, value); }
        }

        public ShippingMethod SelectedShippingMethod
        {
            get
            {
                return _selectedShippingMethod;
            }

            set
            {
                if (SetProperty(ref _selectedShippingMethod, value))
                {
                    _order.ShippingMethod = _selectedShippingMethod;
                    SubmitCommand.RaiseCanExecuteChanged();
                }

                var shippingCost = _selectedShippingMethod != null ? _selectedShippingMethod.Cost : 0;
                UpdatePrices(shippingCost);
            }
        }

        public CheckoutDataViewModel SelectedCheckoutData
        {
            get
            {
                return _selectedCheckoutData;
            }

            set
            {
                if (SetProperty(ref _selectedCheckoutData, value))
                {
                    // Display the AppBar if there is a something selected
                    IsBottomAppBarOpened = _selectedCheckoutData != null;
                }
            }
        }

        public CheckoutDataViewModel SelectedAllCheckoutData
        {
            get
            {
                return _selectedAllCheckoutData;
            }

            set
            {
                var oldValue = _selectedAllCheckoutData;
                if (SetProperty(ref _selectedAllCheckoutData, value) && value != null && oldValue != null)
                {
                    // Update the CheckoutData of the Order
                    UpdateOrderCheckoutData(_selectedAllCheckoutData);
                }
            }
        }

        public DelegateCommand SubmitCommand { get; private set; }

        public DelegateCommand EditCheckoutDataCommand { get; private set; }

        public DelegateCommand SelectCheckoutDataCommand { get; private set; }

        public DelegateCommand AddCheckoutDataCommand { get; private set; }

        public ISignInUserControlViewModel SignInUserControlViewModel
        {
            get { return _signInUserControlViewModel; }
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            // Get latest shopping cart
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();
            _order = _orderRepository.CurrentOrder;
            _order.ShoppingCart = shoppingCart;

            // Populate the ShoppingCart items
            var shoppingCartItemVMs = _order.ShoppingCart.ShoppingCartItems.Select(item => new ShoppingCartItemViewModel(item, _resourceLoader));
            ShoppingCartItemViewModels = new ReadOnlyCollection<ShoppingCartItemViewModel>(shoppingCartItemVMs.ToList());

            // Populate the ShippingMethods and set the selected one
            var shippingMethods = await _shippingMethodService.GetShippingMethodsAsync();
            ShippingMethods = new ReadOnlyCollection<ShippingMethod>(shippingMethods.ToList());
            SelectedShippingMethod = _order.ShippingMethod != null ? ShippingMethods.FirstOrDefault(c => c.Id == _order.ShippingMethod.Id) : null;

            // Update order's address and payment information
            _order.ShippingAddress = await _checkoutDataRepository.GetShippingAddressAsync(_order.ShippingAddress.Id);
            _order.BillingAddress = await _checkoutDataRepository.GetBillingAddressAsync(_order.BillingAddress.Id);
            _order.PaymentMethod = await _checkoutDataRepository.GetPaymentMethodAsync(_order.PaymentMethod.Id);

            // Populate the CheckoutData items (Addresses & payment information)
            CheckoutDataViewModels = new ObservableCollection<CheckoutDataViewModel>
                {
                    CreateCheckoutData(_order.ShippingAddress, Constants.ShippingAddress),
                    CreateCheckoutData(_order.BillingAddress, Constants.BillingAddress),
                    CreateCheckoutData(_order.PaymentMethod)
                };

            base.OnNavigatedTo(e, viewModelState);

            if (e.NavigationMode == NavigationMode.Refresh)
            {
                // Restore the selected CheckoutData manually
                string selectedCheckoutData = RetrieveEntityStateValue<string>("selectedCheckoutData", viewModelState);

                if (!string.IsNullOrWhiteSpace(selectedCheckoutData))
                {
                    SelectedCheckoutData = CheckoutDataViewModels.FirstOrDefault(c => c.EntityId == selectedCheckoutData);
                }
            }
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);

            if (SelectedCheckoutData != null)
            {
                // Store the selected CheckoutData manually
                AddEntityStateValue("selectedCheckoutData", SelectedCheckoutData.EntityId, viewModelState);
            }
        }

        private bool CanSubmit()
        {
            return SelectedShippingMethod != null;
        }

        private async Task SubmitAsync()
        {
            string errorMessage = string.Empty;
            try
            {
                if (await _accountService.VerifySavedCredentialsAsync() == null)
                {
                    _signInUserControlViewModel.Open(async () => await SubmitOrderTransactionAsync());
                }
                else
                {
                    await SubmitOrderTransactionAsync();
                }
            }
            catch (Exception ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorProcessingOrder"));
            }
        }

        private async Task<bool> SubmitOrderTransactionAsync()
        {
            string errorMessage = string.Empty;

            try
            {
                await _orderService.ProcessOrderAsync(_order);
                await _shoppingCartRepository.ClearCartAsync();

                _navigationService.Navigate("OrderConfirmation", Guid.NewGuid().ToString());
                return true;
            }
            catch (ModelValidationException mvex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, mvex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorProcessingOrder"));
            }

            return false;
        }

        private void AddCheckoutData()
        {
            var selectedData = SelectedCheckoutData;
            if (selectedData == null)
            {
                return;
            }

            // Add a new address/payment
            string addNewAddressType = selectedData.DataType == Constants.ShippingAddress ? "ShippingAddress"
                                    : selectedData.DataType == Constants.BillingAddress ? "BillingAddress" : "PaymentMethod";

            _navigationService.Navigate(addNewAddressType, null);
        }

        private void EditCheckoutData()
        {
            var selectedData = SelectedCheckoutData;
            if (selectedData == null)
            {
                return;
            }

            // Hide the App Bar
            IsBottomAppBarOpened = false;

            // Edit selected address/payment
            _navigationService.Navigate(selectedData.DataType, selectedData.EntityId);
        }

        private async Task SelectCheckoutDataAsync()
        {
            var selectedData = SelectedCheckoutData;
            IEnumerable<CheckoutDataViewModel> checkoutData = null;

            switch (selectedData.DataType)
            {
                case Constants.ShippingAddress:
                    checkoutData = (await _checkoutDataRepository.GetAllShippingAddressesAsync()).Select(address => CreateCheckoutData(address, Constants.ShippingAddress));
                    AllCheckoutDataViewModels = new ReadOnlyCollection<CheckoutDataViewModel>(checkoutData.ToList());
                    SelectCheckoutDataLabel = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("SelectCheckoutData"), _resourceLoader.GetString("ShippingAddress"));
                    break;
                case Constants.BillingAddress:
                    checkoutData = (await _checkoutDataRepository.GetAllBillingAddressesAsync()).Select(address => CreateCheckoutData(address, Constants.BillingAddress));
                    AllCheckoutDataViewModels = new ReadOnlyCollection<CheckoutDataViewModel>(checkoutData.ToList());
                    SelectCheckoutDataLabel = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("SelectCheckoutData"), _resourceLoader.GetString("BillingAddress"));
                    break;
                case Constants.PaymentMethod:
                    checkoutData = (await _checkoutDataRepository.GetAllPaymentMethodsAsync()).Select(CreateCheckoutData);
                    AllCheckoutDataViewModels = new ReadOnlyCollection<CheckoutDataViewModel>(checkoutData.ToList());
                    SelectCheckoutDataLabel = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("SelectCheckoutData"), _resourceLoader.GetString("PaymentMethod"));
                    break;
            }

            if (AllCheckoutDataViewModels != null)
            {
                // Select the order's CheckoutData
                SelectedAllCheckoutData = AllCheckoutDataViewModels.FirstOrDefault(c => c.EntityId == SelectedCheckoutData.EntityId);
            }
        }

        private void UpdatePrices(double shippingCost)
        {
            var currencyFormatter = new CurrencyFormatter(_order.ShoppingCart.Currency);

            OrderSubtotal = currencyFormatter.FormatDouble(Math.Round(_order.ShoppingCart.TotalPrice, 2));
            ShippingCost = currencyFormatter.FormatDouble(shippingCost);

            var taxCost = Math.Round((_order.ShoppingCart.TotalPrice + shippingCost) * _order.ShoppingCart.TaxRate, 2);
            TaxCost = currencyFormatter.FormatDouble(taxCost);

            var grandTotal = Math.Round(_order.ShoppingCart.TotalPrice + shippingCost + taxCost, 2);
            GrandTotal = currencyFormatter.FormatDouble(grandTotal);
        }

        private async void UpdateOrderCheckoutData(CheckoutDataViewModel checkouData)
        {
            // Update order & CheckoutData collection items with the new info
            switch (checkouData.DataType)
            {
                case Constants.ShippingAddress:
                    var updatedShippingAddress = await _checkoutDataRepository.GetShippingAddressAsync(checkouData.EntityId);
                    CheckoutDataViewModels[0] = CreateCheckoutData(updatedShippingAddress, Constants.ShippingAddress);
                    _order.ShippingAddress = updatedShippingAddress;
                    break;
                case Constants.BillingAddress:
                    var updatedBillingAddress = await _checkoutDataRepository.GetBillingAddressAsync(checkouData.EntityId);
                    CheckoutDataViewModels[1] = CreateCheckoutData(updatedBillingAddress, Constants.BillingAddress);
                    _order.BillingAddress = updatedBillingAddress;
                    break;
                case Constants.PaymentMethod:
                    var updatedPaymentMethod = await _checkoutDataRepository.GetPaymentMethodAsync(checkouData.EntityId);
                    CheckoutDataViewModels[2] = CreateCheckoutData(updatedPaymentMethod);
                    _order.PaymentMethod = updatedPaymentMethod;
                    break;
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