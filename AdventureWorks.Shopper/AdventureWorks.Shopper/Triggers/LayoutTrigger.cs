using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace AdventureWorks.Shopper.Triggers
{
    public enum LayoutStateType
    {
        Minimal, Portrait, Landscape
    }

    public class LayoutTrigger : StateTriggerBase
    {
        public static readonly DependencyProperty MinimalStateWidthProperty = DependencyProperty.Register("MinimalStateWidth", typeof(double), typeof(LayoutTrigger), new PropertyMetadata(500.0, OnTriggerPropertyChanged));

        public static readonly DependencyProperty LayoutStateProperty = DependencyProperty.Register("LayoutState", typeof(LayoutStateType), typeof(LayoutTrigger), new PropertyMetadata(LayoutStateType.Landscape, OnTriggerPropertyChanged));

        public LayoutTrigger()
        {
            Window.Current.SizeChanged += Window_SizeChanged;
            UpdateTrigger();
        }

        public double MinimalStateWidth
        {
            get
            {
                return (double)GetValue(MinimalStateWidthProperty);
            }

            set
            {
                SetValue(MinimalStateWidthProperty, value);
            }
        }

        public LayoutStateType LayoutState
        {
            get
            {
                return (LayoutStateType)GetValue(LayoutStateProperty);
            }

            set
            {
                SetValue(LayoutStateProperty, value);
            }
        }

        private static void OnTriggerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var trigger = d as LayoutTrigger;
            trigger.UpdateTrigger();
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            UpdateTrigger();
        }

        private void UpdateTrigger()
        {
            switch (LayoutState)
            {
                case LayoutStateType.Minimal:
                    if (Window.Current.Bounds.Width <= MinimalStateWidth)
                    {
                        SetActive(true);
                    }
                    else
                    {
                        SetActive(false);
                    }

                    break;
                case LayoutStateType.Portrait:
                    if (Window.Current.Bounds.Width < Window.Current.Bounds.Height)
                    {
                        SetActive(true);
                    }
                    else
                    {
                        SetActive(false);
                    }

                    break;
                case LayoutStateType.Landscape:
                default:
                    if (Window.Current.Bounds.Width < Window.Current.Bounds.Height)
                    {
                        SetActive(false);
                    }
                    else
                    {
                        SetActive(true);
                    }

                    break;
            }
        }
    }
}
