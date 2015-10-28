using AdventureWorks.UILogic;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class ChangeDefaultsFlyout : SettingsFlyout
    {
        public ChangeDefaultsFlyout()
        {
            InitializeComponent();

            var viewModel = this.DataContext as IFlyoutViewModel;
            viewModel.CloseFlyout = () => this.Hide();
        }
    }
}
