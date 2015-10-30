

using System;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockShoppingCartUpdatedEvent : ShoppingCartUpdatedEvent
    {
        public MockShoppingCartUpdatedEvent()
        {
            PublishDelegate = () => { };
        }

        public Action PublishDelegate { get; set; }

        public override void Publish(object argument)
        {
            PublishDelegate();
        }
    }
}
