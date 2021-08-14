using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.Commands
{
    public class DelegateCommand<TParameter> : NotifyPropertyChangedBase,
        ICommand,
        IDisposable where TParameter : class
    {
        private readonly Action? _actionExecute;
        private readonly Action<TParameter?>? _actionParameterExecute;
        private readonly Func<Task>? _taskExecute;
        private readonly Func<TParameter?, Task>? _taskParameterExecute;
        private readonly CancellationTokenSource _tokenSource = new();
        private TParameter? _buffParameter;

        public DelegateCommand(Action execute)
        {
            _actionExecute =
                execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public DelegateCommand(Action execute,
            Func<TParameter?, CancellationToken, bool> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Action execute,
            Func<TParameter?, CancellationToken, Task<bool>> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Action<TParameter?> execute)
        {
            _actionParameterExecute =
                execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public DelegateCommand(Action<TParameter?> execute,
            Func<TParameter?, CancellationToken, bool> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Action<TParameter?> execute,
            Func<TParameter?, CancellationToken, Task<bool>> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Func<Task> execute)
        {
            _taskExecute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public DelegateCommand(Func<Task> execute,
            Func<TParameter?, CancellationToken, bool> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Func<Task> execute,
            Func<TParameter?, CancellationToken, Task<bool>> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Func<TParameter?, Task> execute)
        {
            _taskParameterExecute =
                execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public DelegateCommand(Func<TParameter?, Task> execute,
            Func<TParameter?, CancellationToken, bool> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public DelegateCommand(Func<TParameter?, Task> execute,
            Func<TParameter?, CancellationToken, Task<bool>> canExecute) : this(execute)
        {
            LoopRun(canExecute);
        }

        public event EventHandler? CanExecuteChanged;
        public bool IsCanExecute { get; private set; } = true;
        private CancellationToken Token => _tokenSource.Token;

        public bool CanExecute(object? parameter)
        {
            _buffParameter = parameter as TParameter;
            return IsCanExecute;
        }

        public void Execute(object? parameter)
        {
            var buff = parameter as TParameter;
            _buffParameter = buff;
            _actionExecute?.Invoke();
            _taskExecute?.Invoke();
            _actionParameterExecute?.Invoke(buff);
            _taskParameterExecute?.Invoke(buff);
        }

        protected virtual void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void LoopRun(
            Func<TParameter?, CancellationToken, Task<bool>> canExecute)
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
                        IsCanExecute = await canExecute.Invoke(_buffParameter, ct);
                        await Task.Delay(1, ct);
                    }
                },
                Token);
        }

        private void LoopRun(Func<TParameter?, CancellationToken, bool> canExecute)
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
                        IsCanExecute = canExecute.Invoke(_buffParameter, ct);
                        await Task.Delay(1, ct);
                    }
                },
                Token);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tokenSource.Cancel();
                _tokenSource.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
