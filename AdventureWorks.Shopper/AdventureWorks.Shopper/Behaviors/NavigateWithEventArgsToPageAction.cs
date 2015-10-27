// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Xaml.Interactivity;
using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AdventureWorks.Shopper.Behaviors
{
    public class NavigateWithEventArgsToPageAction : DependencyObject, IAction
    {
        public string TargetPage { get; set; }

        public string EventArgsParameterPath { get; set; }

        object IAction.Execute(object sender, object parameter)
        {
            // Walk the ParameterPath for nested properties.
            var propertyPathParts = EventArgsParameterPath.Split('.');
            object propertyValue = parameter;
            foreach (var propertyPathPart in propertyPathParts)
            {
                var propInfo = propertyValue.GetType().GetTypeInfo().GetDeclaredProperty(propertyPathPart);
                propertyValue = propInfo.GetValue(propertyValue);    
            }

            var pageType = Type.GetType(TargetPage);
            
            var frame = GetFrame(sender as DependencyObject);
            return frame.Navigate(pageType, propertyValue);
        }

        private Frame GetFrame(DependencyObject dependencyObject)
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            var parentFrame = parent as Frame;
            if (parentFrame != null)
            {
                return parentFrame;
            }

            return GetFrame(parent);
        }
    }
}
