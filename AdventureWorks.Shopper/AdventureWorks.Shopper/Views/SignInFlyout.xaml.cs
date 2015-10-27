// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AdventureWorks.Events;
using AdventureWorks.UILogic;
using Prism.Events;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class SignInFlyout : SettingsFlyout
    {
        private readonly IEventAggregator _eventAggregator;

        public SignInFlyout(IEventAggregator eventAggregator)
        {
            this.InitializeComponent();
            _eventAggregator = eventAggregator;
            this.PasswordBox.KeyDown += PasswordBox_KeyDown;
            this.Unloaded += SignInFlyout_Unloaded;
            _eventAggregator.GetEvent<FocusOnKeyboardInputChangedEvent>().Publish(false);
            var viewModel = this.DataContext as IFlyoutViewModel;
            viewModel.CloseFlyout = () => this.Hide();
        }

        private void SignInFlyout_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _eventAggregator.GetEvent<FocusOnKeyboardInputChangedEvent>().Publish(true);
        }

        private void PasswordBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SubmitButton.Command.Execute(null);
            }
        }
    }
}
