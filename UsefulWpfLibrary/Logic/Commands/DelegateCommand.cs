using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.Commands
{
    public class DelegateCommand : NotifyPropertyChangedBase, ICommand, IDisposable
    {
        private readonly Action? _actionExecute;
        private readonly Action<object?>? _actionParameterExecute;
        private readonly Func<Task>? _taskExecute;
        private readonly Func<object?, Task>? _taskParameterExecute;
        private readonly CancellationTokenSource _tokenSource = new();
        private object? _buffParameter;

        public DelegateCommand(Action execute)
        {
            _actionExecute =
                execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public DelegateCommand(Action execute, Func<CanExecuteArgs, bool> canExecute) :
            this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Action execute,
            Func<CanExecuteArgs, Task<bool>> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Action<object?> execute)
        {
            _actionParameterExecute =
                execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public DelegateCommand(Action<object?> execute,
            Func<CanExecuteArgs, bool> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Action<object?> execute,
            Func<CanExecuteArgs, Task<bool>> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Func<Task> execute)
        {
            _taskExecute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public DelegateCommand(Func<Task> execute,
            Func<CanExecuteArgs, bool> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Func<Task> execute,
            Func<CanExecuteArgs, Task<bool>> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Func<object?, Task> execute)
        {
            _taskParameterExecute =
                execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public DelegateCommand(Func<object?, Task> execute,
            Func<CanExecuteArgs, bool> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Func<object?, Task> execute,
            Func<CanExecuteArgs, Task<bool>> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public event EventHandler? CanExecuteChanged;
        public bool IsCanExecute { get; private set; } = true;
        private CancellationToken Token => _tokenSource.Token;

        public bool CanExecute(object? parameter)
        {
            _buffParameter = parameter;
            return IsCanExecute;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Execute(object? parameter)
        {
            _buffParameter = parameter;
            _actionExecute?.Invoke();
            _taskExecute?.Invoke();
            _actionParameterExecute?.Invoke(parameter);
            _taskParameterExecute?.Invoke(parameter);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tokenSource.Cancel();
                _tokenSource.Dispose();
            }
        }

        protected virtual void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void LoopRun(Func<CanExecuteArgs, Task<bool>> canExecute)
        {
            if (canExecute == null) throw new ArgumentNullException(nameof(canExecute));
            _ = AdvancedTaskRun.Run(async ct =>
                {
                    while (ct.IsCancellationRequested is false)
                    {
                        await Application.Current.Dispatcher.InvokeAsync(
                            RaiseCanExecuteChanged,
                            DispatcherPriority.ApplicationIdle,
                            ct);
                        IsCanExecute =
                            await canExecute.Invoke(
                                new CanExecuteArgs(_buffParameter, ct));
                        await Task.Delay(1, ct);
                    }
                },
                Token);
        }

        private void LoopRun(Func<CanExecuteArgs, bool> canExecute)
        {
            if (canExecute == null) throw new ArgumentNullException(nameof(canExecute));
            _ = AdvancedTaskRun.Run(async ct =>
                {
                    while (ct.IsCancellationRequested is false)
                    {
                        await Application.Current.Dispatcher.InvokeAsync(
                            RaiseCanExecuteChanged,
                            DispatcherPriority.ApplicationIdle,
                            ct);
                        IsCanExecute =
                            canExecute.Invoke(new CanExecuteArgs(_buffParameter, ct));
                        await Task.Delay(1, ct);
                    }
                },
                Token);
        }
    }
}
