using AdventureWorks.UILogic.Models;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public interface IBillingAddressUserControlViewModel
    {
        event PropertyChangedEventHandler PropertyChanged;

        [RestorableState]
        Address Address { get; set; }

        IReadOnlyCollection<ComboBoxItemValue> States { get; }

        bool IsEnabled { get; set; }

        void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewState);

        void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewState, bool suspending);

        Task ProcessFormAsync();

        bool ValidateForm();

        Task PopulateStatesAsync();
        
        void SetLoadDefault(bool loadDefault);
    }
}