// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Prism.Events;
using System;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockEventAggregator : IEventAggregator
    {
        public Func<Type, EventBase> GetEventDelegate { get; set; }

        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            return (TEventType)GetEventDelegate(typeof(TEventType));
        }
    }
}
