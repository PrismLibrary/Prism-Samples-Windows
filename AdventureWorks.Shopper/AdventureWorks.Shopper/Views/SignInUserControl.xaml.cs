using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class SignInUserControl : UserControl
    {
        public SignInUserControl()
        {
            this.InitializeComponent();
            this.IsEnabledChanged += SignInUserControl_IsEnabledChanged;
            this.PasswordBox.KeyDown += PasswordBox_KeyDown;
        }

        private void PasswordBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SubmitButton.Command.Execute(null);
            }
        }

        private void SignInUserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                Username.Focus(FocusState.Programmatic);
            }
        }
    }
}
