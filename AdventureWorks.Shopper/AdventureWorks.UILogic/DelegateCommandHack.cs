using System;
using System.Threading.Tasks;
using Prism.Commands;

namespace AdventureWorks.UILogic {
    /// <summary>Bring back FromAsyncHandler temporarily to limit the number and extend of changes.</summary>
    public class DelegateCommandHack
    {
        public static DelegateCommand FromAsyncHandler(Func<Task> executeAsync, Func<bool> canExecute)
            => new DelegateCommand(() => executeAsync?.Invoke()?.Wait(), canExecute);
        public static DelegateCommand FromAsyncHandler(Func<Task> executeAsync)
            => new DelegateCommand(() => executeAsync?.Invoke()?.Wait());
    }
    /// <summary>Bring back FromAsyncHandler temporarily to limit the number and extend of changes.</summary>
    public class DelegateCommandHack<T> {
        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeAsync, Func<T, bool> canExecute)
            => new DelegateCommand<T>(value => executeAsync?.Invoke(value)?.Wait(), canExecute);
        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeAsync)
            => new DelegateCommand<T>(value => executeAsync?.Invoke(value)?.Wait());
    }
}
