using System.Globalization;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public class OrderConfirmationPageViewModel : ViewModelBase
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly INavigationService _navigationService;

        public OrderConfirmationPageViewModel(IResourceLoader resourceLoader, INavigationService navigationService)
        {
            _resourceLoader = resourceLoader;
            _navigationService = navigationService;
        }

        public string OrderConfirmationContent { get; set; }

        public override void OnNavigatedTo(NavigatedToEventArgs e, System.Collections.Generic.Dictionary<string, object> viewModelState)
        {
            OrderConfirmationContent = string.Format(CultureInfo.InvariantCulture, _resourceLoader.GetString("OrderConfirmationContent"), e.Parameter);
            _navigationService.ClearHistory();
        }
    }
}