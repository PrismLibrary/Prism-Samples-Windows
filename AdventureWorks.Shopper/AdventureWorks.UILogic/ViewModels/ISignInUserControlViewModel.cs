using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public interface ISignInUserControlViewModel
    {
        void Open(Action successAction);

        void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewState);

        void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewState, bool suspending);
    }
}
