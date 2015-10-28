

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockAlertMessageService : IAlertMessageService
    {
        public Func<string, string, Task> ShowAsyncDelegate { get; set; }

        public Func<string, string, IEnumerable<DialogCommand>, Task> ShowAsyncWithCommandsDelegate { get; set; }

        public Task ShowAsync(string message, string title)
        {
            return ShowAsyncDelegate(message, title);
        }

        public Task ShowAsync(string message, string title, IEnumerable<DialogCommand> dialogCommands)
        {
            return ShowAsyncWithCommandsDelegate(message, title, dialogCommands);
        }
    }
}
