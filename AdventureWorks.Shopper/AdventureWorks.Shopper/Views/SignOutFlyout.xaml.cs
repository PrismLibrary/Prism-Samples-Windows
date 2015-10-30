using AdventureWorks.UILogic;
using Prism.Windows.AppModel;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class SignOutFlyout : SettingsFlyout
    {
        public SignOutFlyout()
        {
            this.InitializeComponent();

            var viewModel = this.DataContext as IFlyoutViewModel;
            viewModel.CloseFlyout = () => this.Hide();
        }
    }
}
