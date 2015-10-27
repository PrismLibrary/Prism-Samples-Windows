// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Views
{
    public static class ScrollViewerUtilities
    {
        public static void ScrollToProportion(ScrollViewer scrollViewer, double scrollViewerOffsetProportion)
        {
            // Update the Horizontal and Vertical offset
            if (scrollViewer == null)
            {
                return;
            }

            var scrollViewerHorizontalOffset = scrollViewerOffsetProportion * scrollViewer.ScrollableWidth;
            var scrollViewerVerticalOffset = scrollViewerOffsetProportion * scrollViewer.ScrollableHeight;

            scrollViewer.ChangeView(scrollViewerHorizontalOffset, scrollViewerVerticalOffset, null);
        }

        public static double GetScrollViewerOffsetProportion(ScrollViewer scrollViewer)
        {
            if (scrollViewer == null)
            {
                return 0;
            }

            var horizontalOffsetProportion = (scrollViewer.ScrollableWidth == 0) ? 0 : (scrollViewer.HorizontalOffset / scrollViewer.ScrollableWidth);
            var verticalOffsetProportion = (scrollViewer.ScrollableHeight == 0) ? 0 : (scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight);

            var scrollViewerOffsetProportion = Math.Max(horizontalOffsetProportion, verticalOffsetProportion);
            return scrollViewerOffsetProportion;
        }
    }
}
