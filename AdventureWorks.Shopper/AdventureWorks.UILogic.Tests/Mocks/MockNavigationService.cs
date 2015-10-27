// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Prism.Windows.Navigation;
using System;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockNavigationService : INavigationService
    {
        public Func<string, object, bool> NavigateDelegate { get; set; }
        public Action GoBackDelegate { get; set; }
        public Func<bool> CanGoBackDelegate { get; set; }
        public Action ClearHistoryDelegate { get; set; }

        public bool Navigate(string pageToken, object parameter)
        {
            return this.NavigateDelegate(pageToken, parameter);
        }

        public void GoBack()
        {
            this.GoBackDelegate();
        }

        public bool CanGoBack()
        {
            return this.CanGoBackDelegate();
        }

        public void ClearHistory()
        {
            ClearHistoryDelegate();
        }

        public void RestoreSavedNavigation()
        {
            throw new NotImplementedException();
        }

        public void Suspending()
        {
            throw new NotImplementedException();
        }

        public void GoForward()
        {
            throw new NotImplementedException();
        }

        public bool CanGoForward()
        {
            throw new NotImplementedException();
        }

        public void RemoveFirstPage(string pageToken = null, object parameter = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveLastPage(string pageToken = null, object parameter = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllPages(string pageToken = null, object parameter = null)
        {
            throw new NotImplementedException();
        }

        public int BackStackDepth { get; set; }
    }
}
