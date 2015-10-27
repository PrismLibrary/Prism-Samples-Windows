// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace AdventureWorks.Shopper.Behaviors
{
    public class HighlightSearchAction : DependencyObject, IAction
    {
        public static readonly DependencyProperty SearchTermProperty =
            DependencyProperty.RegisterAttached("SearchTerm", typeof(string), typeof(HighlightSearchAction), new PropertyMetadata(null));

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.RegisterAttached("SearchText", typeof(string), typeof(HighlightSearchAction), new PropertyMetadata(null));

        public static readonly DependencyProperty HighlightBrushProperty =
            DependencyProperty.Register("HighlightBrush", typeof(Brush), typeof(HighlightSearchAction), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        public string SearchTerm
        {
            get { return (string)GetValue(SearchTermProperty); }
            set { SetValue(SearchTermProperty, value); }
        }

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public Brush HighlightBrush
        {
            get { return (Brush)GetValue(HighlightBrushProperty); }
            set { SetValue(HighlightBrushProperty, value); }
        }

        public object Execute(object sender, object parameter)
        {
            var textBlock = sender as TextBlock;
            if (textBlock == null)
            {
                return false;
            }

            var searchTerm = SearchTerm;
            var originalText = SearchText;
            if (string.IsNullOrEmpty(originalText) || (searchTerm == null))
            {
                return false;
            }

            if (searchTerm.Length == 0)
            {
                textBlock.Text = originalText;
                return true;
            }

            textBlock.Inlines.Clear();
            var currentIndex = 0;
            var searchTermLength = searchTerm.Length;
            int index = originalText.IndexOf(searchTerm, 0, StringComparison.CurrentCultureIgnoreCase);
            while (index > -1)
            {
                textBlock.Inlines.Add(new Run() { Text = originalText.Substring(currentIndex, index - currentIndex) });
                currentIndex = index + searchTermLength;
                textBlock.Inlines.Add(new Run() { Text = originalText.Substring(index, searchTermLength), Foreground = HighlightBrush });
                index = originalText.IndexOf(searchTerm, currentIndex, 0, StringComparison.CurrentCultureIgnoreCase);
            }

            textBlock.Inlines.Add(new Run() { Text = originalText.Substring(currentIndex) });
            return true;
        }
    }
}