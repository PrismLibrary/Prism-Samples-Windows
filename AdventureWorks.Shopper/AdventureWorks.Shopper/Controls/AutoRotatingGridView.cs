// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace AdventureWorks.Shopper.Controls
{
    public class AutoRotatingGridView : GridView
    {
        // Dependency Properties for portrait layout
        public static readonly DependencyProperty PortraitItemTemplateProperty =
            DependencyProperty.Register("PortraitItemTemplate", typeof(DataTemplate), typeof(AutoRotatingGridView), new PropertyMetadata(null));

        public static readonly DependencyProperty PortraitItemsPanelProperty =
            DependencyProperty.Register("PortraitItemsPanel", typeof(ItemsPanelTemplate), typeof(AutoRotatingGridView), new PropertyMetadata(null));

        public static readonly DependencyProperty PortraitGroupStyleProperty =
            DependencyProperty.Register("PortraitGroupStyle", typeof(ObservableCollection<GroupStyle>), typeof(AutoRotatingGridView), new PropertyMetadata(null));

        // Dependency Properties for minimum layout
        public static readonly DependencyProperty MinimalItemTemplateProperty =
            DependencyProperty.Register("MinimalItemTemplate", typeof(DataTemplate), typeof(AutoRotatingGridView), new PropertyMetadata(null));

        public static readonly DependencyProperty MinimalItemsPanelProperty =
            DependencyProperty.Register("MinimalItemsPanel", typeof(ItemsPanelTemplate), typeof(AutoRotatingGridView), new PropertyMetadata(null));

        public static readonly DependencyProperty MinimalGroupStyleProperty =
            DependencyProperty.Register("MinimalGroupStyle", typeof(ObservableCollection<GroupStyle>), typeof(AutoRotatingGridView), new PropertyMetadata(null));

        public static readonly DependencyProperty MinimalLayoutWidthProperty =
            DependencyProperty.Register("MinimalLayoutWidth", typeof(int), typeof(AutoRotatingGridView), new PropertyMetadata(500));

        // Default styles
        protected static readonly string DefaultLandscapeItemsPanelTemplate =
            "<ItemsPanelTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><ItemsWrapGrid Orientation='Vertical' /></ItemsPanelTemplate>";

        protected static readonly string DefaultPortraitItemsPanelTemplate =
            "<ItemsPanelTemplate  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><ItemsWrapGrid Orientation='Horizontal' /></ItemsPanelTemplate>";

        // Private members
        private DataTemplate landscapeItemTemplate = null;
        private ItemsPanelTemplate landscapeItemsPanel = null;
        private IList<GroupStyle> landscapeGroupStyle = null;
        private AutoRotateGridViewLayouts previousState = AutoRotateGridViewLayouts.Unknown;

        public AutoRotatingGridView()
        {
            this.SizeChanged += RotatingGridview_SizeChanged;

            // These styles will be overriden by the custom ones
            this.ItemsPanel = XamlReader.Load(DefaultLandscapeItemsPanelTemplate) as ItemsPanelTemplate;
            this.PortraitItemsPanel = XamlReader.Load(DefaultPortraitItemsPanelTemplate) as ItemsPanelTemplate;

            this.PortraitGroupStyle = new ObservableCollection<GroupStyle>();
            this.MinimalGroupStyle = new ObservableCollection<GroupStyle>();
        }

        // Layouts
        private enum AutoRotateGridViewLayouts
        {
            Landscape,
            Portrait,
            Minimal,
            Unknown,
        }

        // Properties
        public DataTemplate PortraitItemTemplate
        {
            get { return (DataTemplate)GetValue(PortraitItemTemplateProperty); }
            set { SetValue(PortraitItemTemplateProperty, value); }
        }

        public ItemsPanelTemplate PortraitItemsPanel
        {
            get { return (ItemsPanelTemplate)GetValue(PortraitItemsPanelProperty); }
            set { SetValue(PortraitItemsPanelProperty, value); }
        }

        public ObservableCollection<GroupStyle> PortraitGroupStyle
        {
            get { return (ObservableCollection<GroupStyle>)GetValue(PortraitGroupStyleProperty); }
            set { SetValue(PortraitGroupStyleProperty, value); }
        }

        public DataTemplate MinimalItemTemplate
        {
            get { return (DataTemplate)GetValue(MinimalItemTemplateProperty); }
            set { SetValue(MinimalItemTemplateProperty, value); }
        }

        public ItemsPanelTemplate MinimalItemsPanel
        {
            get { return (ItemsPanelTemplate)GetValue(MinimalItemsPanelProperty); }
            set { SetValue(MinimalItemsPanelProperty, value); }
        }

        public ObservableCollection<GroupStyle> MinimalGroupStyle
        {
            get { return (ObservableCollection<GroupStyle>)GetValue(MinimalGroupStyleProperty); }
            set { SetValue(MinimalGroupStyleProperty, value); }
        }

        public int MinimalLayoutWidth
        {
            get { return (int)GetValue(MinimalLayoutWidthProperty); }
            set { SetValue(MinimalLayoutWidthProperty, value); }
        }

        // Methods and handlers
        private void RotatingGridview_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // We save the landscape styles as these properties will be changed later
            if (this.landscapeItemTemplate == null)
            {
                this.landscapeItemTemplate = this.ItemTemplate;
            }

            if (this.landscapeItemsPanel == null)
            {
                this.landscapeItemsPanel = this.ItemsPanel;
            }

            if (this.landscapeGroupStyle == null)
            {
                this.landscapeGroupStyle = new List<GroupStyle>();
                foreach (GroupStyle style in this.GroupStyle)
                {
                    this.landscapeGroupStyle.Add(style);
                }
            }

            // We search for the corresponding layout and update it if it changed
            if (this.MinimalLayoutWidth > Window.Current.Bounds.Width)
            {
                if (this.previousState != AutoRotateGridViewLayouts.Minimal)
                {
                    this.previousState = AutoRotateGridViewLayouts.Minimal;
                    UpdateLayout(AutoRotateGridViewLayouts.Minimal);
                }
            }
            else if (Window.Current.Bounds.Height > Window.Current.Bounds.Width)
            {
                if (this.previousState != AutoRotateGridViewLayouts.Portrait)
                {
                    this.previousState = AutoRotateGridViewLayouts.Portrait;
                    UpdateLayout(AutoRotateGridViewLayouts.Portrait);
                }
            }
            else
            {
                if (this.previousState != AutoRotateGridViewLayouts.Landscape)
                {
                    this.previousState = AutoRotateGridViewLayouts.Landscape;
                    UpdateLayout(AutoRotateGridViewLayouts.Landscape);
                }
            }
        }

        private void UpdateLayout(AutoRotateGridViewLayouts layout)
        {
            // Landscape layout
            if (layout == AutoRotateGridViewLayouts.Landscape)
            {
                this.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
                this.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                this.SetValue(ScrollViewer.HorizontalScrollModeProperty, ScrollMode.Enabled);
                this.SetValue(ScrollViewer.VerticalScrollModeProperty, ScrollMode.Disabled);
                this.SetValue(ScrollViewer.ZoomModeProperty, ZoomMode.Disabled);
            }
            else
            {
                this.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                this.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
                this.SetValue(ScrollViewer.HorizontalScrollModeProperty, ScrollMode.Disabled);
                this.SetValue(ScrollViewer.VerticalScrollModeProperty, ScrollMode.Enabled);
                this.SetValue(ScrollViewer.ZoomModeProperty, ZoomMode.Disabled);
            }

            this.ChangeItemsPanel(layout);
            this.ChangeGroupStyle(layout);
            this.ChangeItemTemplate(layout);
        }

        // Styles setter methods with fallback
        private void ChangeItemTemplate(AutoRotateGridViewLayouts layout)
        {
            switch (layout)
            {
                case AutoRotateGridViewLayouts.Minimal:
                    if (this.MinimalItemTemplate != null)
                    {
                        this.ItemTemplate = this.MinimalItemTemplate;
                    }
                    else
                    {
                        this.ChangeItemTemplate(AutoRotateGridViewLayouts.Portrait);
                    }

                    break;

                case AutoRotateGridViewLayouts.Portrait:
                    if (this.PortraitItemTemplate != null)
                    {
                        this.ItemTemplate = this.PortraitItemTemplate;
                    }
                    else
                    {
                        this.ChangeItemTemplate(AutoRotateGridViewLayouts.Landscape);
                    }

                    break;

                case AutoRotateGridViewLayouts.Landscape:
                    this.ItemTemplate = landscapeItemTemplate;
                    break;
            }
        }

        private void ChangeItemsPanel(AutoRotateGridViewLayouts layout)
        {
            switch (layout)
            {
                case AutoRotateGridViewLayouts.Minimal:
                    if (this.MinimalItemsPanel != null)
                    {
                        this.ItemsPanel = this.MinimalItemsPanel;
                    }
                    else
                    {
                        this.ChangeItemsPanel(AutoRotateGridViewLayouts.Portrait);
                    }

                    break;

                case AutoRotateGridViewLayouts.Portrait:
                    if (this.PortraitItemsPanel != null)
                    {
                        this.ItemsPanel = this.PortraitItemsPanel;
                    }
                    else
                    {
                        this.ChangeItemsPanel(AutoRotateGridViewLayouts.Landscape);
                    }

                    break;

                case AutoRotateGridViewLayouts.Landscape:
                    this.ItemsPanel = landscapeItemsPanel;
                    break;
            }
        }

        private void ChangeGroupStyle(AutoRotateGridViewLayouts layout)
        {
            switch (layout)
            {
                case AutoRotateGridViewLayouts.Minimal:
                    if (this.MinimalGroupStyle != null && this.MinimalGroupStyle.Count > 0)
                    {
                        this.GroupStyle.Clear();
                        foreach (GroupStyle style in this.MinimalGroupStyle)
                        {
                            this.GroupStyle.Add(style);
                        }
                    }
                    else
                    {
                        this.ChangeGroupStyle(AutoRotateGridViewLayouts.Portrait);
                    }

                    break;

                case AutoRotateGridViewLayouts.Portrait:
                    if (this.PortraitGroupStyle != null && this.PortraitGroupStyle.Count > 0)
                    {
                        this.GroupStyle.Clear();
                        foreach (GroupStyle style in this.PortraitGroupStyle)
                        {
                            this.GroupStyle.Add(style);
                        }
                    }
                    else
                    {
                        this.ChangeGroupStyle(AutoRotateGridViewLayouts.Landscape);
                    }

                    break;

                case AutoRotateGridViewLayouts.Landscape:
                    this.GroupStyle.Clear();
                    if (this.landscapeGroupStyle != null)
                    {
                        foreach (GroupStyle style in this.landscapeGroupStyle)
                        {
                            this.GroupStyle.Add(style);
                        }
                    }

                    break;
            }
        }
    }
}
