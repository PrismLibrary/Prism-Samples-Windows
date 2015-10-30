using AdventureWorks.UILogic.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace AdventureWorks.Shopper.Behaviors
{
    public class ComboBoxKeyboardSelection : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
            ComboBox comboBox = this.AssociatedObject;

            if (comboBox != null)
            {
                comboBox.KeyUp += ComboBox_KeyUp;
            }
        }

        protected override void OnDetached()
        {
            ComboBox comboBox = this.AssociatedObject;

            if (comboBox != null)
            {
                comboBox.KeyUp -= ComboBox_KeyUp;
            }
        }

        private void ComboBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            foreach (var item in comboBox.Items)
            {
                var comboBoxItemValue = item as ComboBoxItemValue;
                if (comboBoxItemValue != null && comboBoxItemValue.Value.StartsWith(e.Key.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.SelectedItem = comboBoxItemValue;
                    return;
                }
            }
        }
    }
}
