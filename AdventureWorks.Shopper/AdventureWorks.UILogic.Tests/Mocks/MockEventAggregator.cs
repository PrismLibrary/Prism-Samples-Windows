

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
