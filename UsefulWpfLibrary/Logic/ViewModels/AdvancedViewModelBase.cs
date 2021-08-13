using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UsefulWpfLibrary.Logic.Commands;

namespace UsefulWpfLibrary.Logic.ViewModels
{
    public class AdvancedViewModelBase : ViewModelBase, IDisposable
    {
        private readonly List<ICommand> _commands = new();

        protected ICommand CreateCommand(Action execute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Action execute,
            Func<object?, CancellationToken, bool> canExecute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Action execute,
            Func<object?, CancellationToken, Task<bool>> canExecute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Action<object?> execute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Action<object?> execute,
            Func<object?, CancellationToken, bool> canExecute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Action<object?> execute,
            Func<object?, CancellationToken, Task<bool>> canExecute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Func<Task> execute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Func<Task> execute,
            Func<object?, CancellationToken, bool> canExecute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Func<Task> execute,
            Func<object?, CancellationToken, Task<bool>> canExecute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Func<object?, Task> execute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Func<object?, Task> execute,
            Func<object?, CancellationToken, bool> canExecute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand(Func<object?, Task> execute,
            Func<object?, CancellationToken, Task<bool>> canExecute)
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Action execute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Action execute,
            Func<T?, CancellationToken, bool> canExecute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            ICommand command = new DelegateCommand<T>(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Action execute,
            Func<T?, CancellationToken, Task<bool>> canExecute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Action<T?> execute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Action<T?> execute,
            Func<T?, CancellationToken, bool> canExecute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Action<T?> execute,
            Func<T?, CancellationToken, Task<bool>> canExecute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Func<Task> execute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Func<Task> execute,
            Func<T?, CancellationToken, bool> canExecute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Func<Task> execute,
            Func<T?, CancellationToken, Task<bool>> canExecute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Func<T?, Task> execute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Func<T?, Task> execute,
            Func<T?, CancellationToken, bool> canExecute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected ICommand CreateCommand<T>(Func<T?, Task> execute,
            Func<T?, CancellationToken, Task<bool>> canExecute) where T : class
        {
#pragma warning disable DF0100 // Marks return values that hides the IDisposable implementation of return value.
            var command = new DelegateCommand<T>(execute, canExecute);
#pragma warning restore DF0100 // Marks return values that hides the IDisposable implementation of return value.
            _commands.Add(command);
            return command;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (ICommand command in _commands)
                {
                    if (command is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }

                _commands.Clear();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
