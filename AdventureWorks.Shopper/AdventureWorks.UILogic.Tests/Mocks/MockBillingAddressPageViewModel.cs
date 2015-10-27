// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using Prism.Windows.Navigation;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockBillingAddressPageViewModel : IBillingAddressUserControlViewModel
    {
        public Func<bool> ValidateFormDelegate { get; set; }
        public Func<Task> ProcessFormAsyncDelegate { get; set; }

        public Address Address { get; set; }        

        public IReadOnlyCollection<ComboBoxItemValue> States { get; set; }

        public bool SetAsDefault { get; set; }

        public string FirstError { get; set; }

        public BindableValidator Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Windows.Input.ICommand GoBackCommand
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Windows.Input.ICommand GoNextCommand
        {
            get { throw new System.NotImplementedException(); }
        }

        public string EntityId { get; set; }

        public void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewState)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewState, bool suspending)
        {
            throw new NotImplementedException();
        }

        public void GoNext()
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

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public void SetLoadDefault(bool loadDefault)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled { get; set; }

        public Task PopulateStatesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
