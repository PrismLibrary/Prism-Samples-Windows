using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public class PaymentMethodUserControlViewModel : ViewModelBase, IPaymentMethodUserControlViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private bool _loadDefault;
        private PaymentMethod _paymentMethod;

        public PaymentMethodUserControlViewModel(ICheckoutDataRepository checkoutDataRepository)
        {
            _paymentMethod = new PaymentMethod();
            _checkoutDataRepository = checkoutDataRepository;
        }

        [RestorableState]
        public PaymentMethod PaymentMethod
        {
            get { return _paymentMethod; }
            set { SetProperty(ref _paymentMethod, value); }
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewState)
        {
            if (viewState != null)
            {
                base.OnNavigatedTo(e, viewState);

                if (e.NavigationMode == NavigationMode.Refresh)
                {
                    // Restore the errors collection manually
                    var errorsCollection = RetrieveEntityStateValue<IDictionary<string, ReadOnlyCollection<string>>>("errorsCollection", viewState);

                    if (errorsCollection != null)
                    {
                        _paymentMethod.SetAllErrors(errorsCollection);
                    }
                }
            }

            if (e.NavigationMode == NavigationMode.New)
            {
                if (_loadDefault)
                {
                    var defaultPaymentMethod = await _checkoutDataRepository.GetDefaultPaymentMethodAsync();
                    if (defaultPaymentMethod != null)
                    {
                        // Update the information and validate the values
                        PaymentMethod.CardNumber = defaultPaymentMethod.CardNumber;
                        PaymentMethod.CardVerificationCode = defaultPaymentMethod.CardVerificationCode;
                        PaymentMethod.CardholderName = defaultPaymentMethod.CardholderName;
                        PaymentMethod.ExpirationMonth = defaultPaymentMethod.ExpirationMonth;
                        PaymentMethod.ExpirationYear = defaultPaymentMethod.ExpirationYear;
                        PaymentMethod.Phone = defaultPaymentMethod.Phone;

                        ValidateForm();
                    }
                }
            }
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewState, suspending);

            // Store the errors collection manually
            if (viewState != null)
            {
                AddEntityStateValue("errorsCollection", _paymentMethod.GetAllErrors(), viewState);
            }
        }

        public async Task ProcessFormAsync()
        {
            var existingPaymentMethods = await _checkoutDataRepository.GetAllPaymentMethodsAsync();
            var matchingExistingPaymentMethod = FindMatchingPaymentMethod(PaymentMethod, existingPaymentMethods);
            if (matchingExistingPaymentMethod != null)
            {
                PaymentMethod = matchingExistingPaymentMethod;
            }
            else
            {
                await _checkoutDataRepository.SavePaymentMethodAsync(PaymentMethod);
            }
        }

        public bool ValidateForm()
        {
            return _paymentMethod.ValidateProperties();
        }

        public void SetLoadDefault(bool loadDefault)
        {
            _loadDefault = loadDefault;
        }

        private static PaymentMethod FindMatchingPaymentMethod(PaymentMethod searchPaymentMethod, IEnumerable<PaymentMethod> paymentMethods)
        {
            // This method is not comparing the Card Number since the Card Number value is being replaced with asterisks
            // when persisted to the service. In a real production app using SSL, you would send/receive the actual card number
            // securely.
            return paymentMethods.FirstOrDefault(paymentMethod =>
                searchPaymentMethod.CardVerificationCode == paymentMethod.CardVerificationCode &&
                searchPaymentMethod.CardholderName == paymentMethod.CardholderName &&
                searchPaymentMethod.ExpirationMonth == paymentMethod.ExpirationMonth &&
                searchPaymentMethod.ExpirationYear == paymentMethod.ExpirationYear &&
                searchPaymentMethod.Phone == paymentMethod.Phone);
        }
    }
}
