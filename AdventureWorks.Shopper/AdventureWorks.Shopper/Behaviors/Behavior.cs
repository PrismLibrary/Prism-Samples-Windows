// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Xaml.Interactivity;
using System;
using System.Reflection;
using Windows.UI.Xaml;

namespace AdventureWorks.Shopper.Behaviors
{
    public abstract class Behavior<T> : DependencyObject, IBehavior where T : DependencyObject
    {
        public T AssociatedObject { get; private set; }

        DependencyObject IBehavior.AssociatedObject
        {
            get { return this.AssociatedObject; }
        }

        public void Attach(DependencyObject associatedObject)
        {
            if (associatedObject != null && !typeof(T).GetTypeInfo().IsAssignableFrom(associatedObject.GetType().GetTypeInfo()))
            {
                throw new Exception(string.Format("associatedObject is not assignable to type:", typeof(T)));
            }

            this.AssociatedObject = associatedObject as T;
            this.OnAttached();
        }

        public void Detach()
        {
            this.OnDetached();
            this.AssociatedObject = null;
        }

        protected abstract void OnAttached();

        protected abstract void OnDetached();
    }
}
