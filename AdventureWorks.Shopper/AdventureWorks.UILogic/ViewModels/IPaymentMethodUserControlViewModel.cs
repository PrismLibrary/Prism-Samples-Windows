using System.Collections.Generic;
using System.ComponentModel;
using AdventureWorks.UILogic.Models;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public interface IPaymentMethodUserControlViewModel
    {
        event PropertyChangedEventHandler PropertyChanged;

        [RestorableState]
        PaymentMethod PaymentMethod { get; set; }

        void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewState);

        void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewState, bool suspending);

        Task ProcessFormAsync();

        bool ValidateForm();

        void SetLoadDefault(bool loadDefault);
    }
}