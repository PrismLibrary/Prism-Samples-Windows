

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.ViewModels;
using Prism.Windows.Navigation;
using Prism.Windows.Validation;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockPaymentMethodPageViewModel : IPaymentMethodUserControlViewModel
    {
        public PaymentMethod PaymentMethod { get; set; }
        public bool SetAsDefault { get; set; }
        public string FirstError { get; set; }
        public BindableValidator Errors { get; private set; }
        public ICommand GoBackCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public string EntityId { get; set; }
        public Func<bool> ValidateFormDelegate { get; set; }
        public Func<Task> ProcessFormAsyncDelegate { get; set; }

        public void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewState)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewState, bool suspending)
        {
            throw new NotImplementedException();
        }

        public void Register()
        {
            throw new System.NotImplementedException();
        }

        public async Task ProcessFormAsync()
        {
            await ProcessFormAsyncDelegate();
        }

        public bool ValidateForm()
        {
            return ValidateFormDelegate();
        }

#pragma warning disable CS0067 // The event is never used
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore
        public void SetLoadDefault(bool loadDefault)
        {
            throw new NotImplementedException();
        }
    }
}
