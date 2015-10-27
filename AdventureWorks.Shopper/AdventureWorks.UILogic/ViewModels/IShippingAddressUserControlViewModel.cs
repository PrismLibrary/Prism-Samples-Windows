// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AdventureWorks.UILogic.Models;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.ViewModels
{
    public interface IShippingAddressUserControlViewModel
    {
        event PropertyChangedEventHandler PropertyChanged;

        [RestorableState]
        Address Address { get; set; }

        IReadOnlyCollection<ComboBoxItemValue> States { get; }

        void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewState);

        void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewState, bool suspending);

        Task ProcessFormAsync();

        bool ValidateForm();

        Task PopulateStatesAsync();

        void SetLoadDefault(bool loadDefault);
    }
}