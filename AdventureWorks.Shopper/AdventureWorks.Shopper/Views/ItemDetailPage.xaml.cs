using Prism.Windows.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232
namespace AdventureWorks.Shopper.Views
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class ItemDetailPage : NavigationAwarePage
    {
        public ItemDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // It is important to call EnableFocusOnKeyboardInput here in the OnNavigatedTo method to
            // give the previous page's SearchUserControl time to tear down.
            this.searchUserControl.EnableFocusOnKeyboardInput();
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            var adventureWorksApp = App.Current as App;
            if (adventureWorksApp != null && !adventureWorksApp.IsSuspending)
            {
                // It is important to call DisableFocusOnKeyboardInput here in the OnNavigatedFrom method 
                // to ensure that this page's SearchUserControl.FocusOnKeyboardInput is set to false 
                // prior to the next page's SearchUserControl.FocusOnKeyboardInput value is set to true
                this.searchUserControl.DisableFocusOnKeyboardInput();
            }
        }
    }
}
