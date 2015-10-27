// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229
namespace AdventureWorks.Shopper.Views
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage : NavigationAwarePage
    {
        private double _scrollViewerOffsetProportion;
        private bool _isPageLoading = true;
        private ScrollViewer _itemsGridViewScrollViewer;
        private long _horizontalScrollBarVisibilityEventToken;
        private long _verticalScrollBarVisibilityEventToken;

        public GroupDetailPage()
        {
            InitializeComponent();
            this.SizeChanged += Page_SizeChanged;
#if WINDOWS_APP
            this.TopAppBar = new AppBar
            {
                Style = (Style)App.Current.Resources["AppBarStyle"],
                Content = new TopAppBarUserControl()
            };
            // x:Uid="TopAppBar"
#endif
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // It is important to call EnableFocusOnKeyboardInput here in the OnNavigatedTo method to
            // give the previous page's SearchUserControl time to tear down.
            this.searchUserControl.EnableFocusOnKeyboardInput();
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
