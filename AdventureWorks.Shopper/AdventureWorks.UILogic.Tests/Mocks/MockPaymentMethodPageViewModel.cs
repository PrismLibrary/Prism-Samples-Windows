// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Prism.Windows.Navigation;

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

        public event PropertyChangedEventHandler PropertyChanged;
        public void SetLoadDefault(bool loadDefault)
        {
            throw new NotImplementedException();
        }
    }
}
