// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Globalization;
using Prism.Windows.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231
namespace AdventureWorks.Shopper.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class CategoryPage : NavigationAwarePage
    {
        private double _scrollViewerOffsetProportion;
        private bool _isPageLoading = true;
        private ScrollViewer _itemsGridViewScrollViewer;
        private long _horizontalScrollBarVisibilityEventToken;
        private long _verticalScrollBarVisibilityEventToken;

        public CategoryPage()
        {
#if WINDOWS_APP
            this.TopAppBar = new AppBar
            {
                Style = (Style)App.Current.Resources["AppBarStyle"],
                Content = new TopAppBarUserControl()
            };
            // x:Uid="TopAppBar"
#endif
            this.InitializeComponent();
            this.SizeChanged += Page_SizeChanged;
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = this.DataContext as INotifyPropertyChanged;
            if (viewModel != null)
            {
                viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }

            // It is important to call EnableFocusOnKeyboardInput here in the OnNavigatedTo method to
            // give the previous page's SearchUserControl time to tear down.
            this.searchUserControl.EnableFocusOnKeyboardInput();
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var viewModel = this.DataContext as INotifyPropertyChanged;
            var adventureWorksApp = Application.Current as App;
            if (adventureWorksApp != null && !adventureWorksApp.IsSuspending && viewModel != null)
            {
                viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            if (adventureWorksApp != null && !adventureWorksApp.IsSuspending)
            {
                // It is important to call DisableFocusOnKeyboardInput here in the OnNavigatedFrom method 
                // to ensure that this page's SearchUserControl.FocusOnKeyboardInput is set to false 
                // prior to the next page's SearchUserControl.FocusOnKeyboardInput value is set to true
                this.searchUserControl.DisableFocusOnKeyboardInput();
            }
        }

        protected override void SaveState(System.Collections.Generic.Dictionary<string, object> pageState)
        {
            if (pageState == null)
            {
                return;
            }

            base.SaveState(pageState);

            pageState["scrollViewerOffsetProportion"] = ScrollViewerUtilities.GetScrollViewerOffsetProportion(_itemsGridViewScrollViewer);
        }

        protected override void LoadState(object navigationParameter, System.Collections.Generic.Dictionary<string, object> pageState)
        {
            if (pageState == null)
            {
                return;
            }

            base.LoadState(navigationParameter, pageState);

            if (pageState.ContainsKey("scrollViewerOffsetProportion"))
            {
                _scrollViewerOffsetProportion = double.Parse(pageState["scrollViewerOffsetProportion"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Subcategories")
            {
                var listViewBase = semanticZoom.ZoomedOutView as ListViewBase;
                if (listViewBase != null)
                {
                    listViewBase.ItemsSource = groupedItemsViewSource.View.CollectionGroups;
                }
            }
        }

        private void ScrollBarVisibilityChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (((Visibility)sender.GetValue(dp)) == Visibility.Visible)
            {
                ScrollViewerUtilities.ScrollToProportion(_itemsGridViewScrollViewer, _scrollViewerOffsetProportion);
                if (_horizontalScrollBarVisibilityEventToken != 0L)
                {
                    sender.UnregisterPropertyChangedCallback(dp, _horizontalScrollBarVisibilityEventToken);
                }

                if (_verticalScrollBarVisibilityEventToken != 0L)
                {
                    sender.UnregisterPropertyChangedCallback(dp, _verticalScrollBarVisibilityEventToken);
                }
            }

            if (_isPageLoading)
            {
                itemsGridView.LayoutUpdated += ItemsGridView_LayoutUpdated;
                _isPageLoading = false;
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);

            if (scrollViewer != null)
            {
                if (scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible && scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
                {
                    ScrollViewerUtilities.ScrollToProportion(scrollViewer, _scrollViewerOffsetProportion);
                }
                else
                {
                    _horizontalScrollBarVisibilityEventToken = scrollViewer.RegisterPropertyChangedCallback(ScrollViewer.ComputedHorizontalScrollBarVisibilityProperty, ScrollBarVisibilityChanged);

                    _verticalScrollBarVisibilityEventToken = scrollViewer.RegisterPropertyChangedCallback(ScrollViewer.ComputedVerticalScrollBarVisibilityProperty, ScrollBarVisibilityChanged);
                }
            }
        }

        private void ItemsGridView_LayoutUpdated(object sender, object e)
        {
            _scrollViewerOffsetProportion = ScrollViewerUtilities.GetScrollViewerOffsetProportion(_itemsGridViewScrollViewer);
        }

        private void ItemsGridView_Loaded(object sender, RoutedEventArgs e)
        {
            _itemsGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);
        }
    }
}
