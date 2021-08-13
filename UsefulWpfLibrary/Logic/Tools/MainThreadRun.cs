using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace UsefulWpfLibrary.Logic.Tools
{
    public static class MainThreadRun
    {
        public static bool IsInMainThread => GetDispatcher().CheckAccess();
        public static bool IsNotInMainThread => !GetDispatcher().CheckAccess();
        public static Dispatcher GetDispatcher()
        {
            return Application.Current.Dispatcher;
        }
        public static void Invoke(Action callback)
        {
            if (IsNotInMainThread)
                GetDispatcher().Invoke(callback);
            else
                callback.Invoke();
        }

        public static T Invoke<T>(Func<T> callback)
        {
            return IsNotInMainThread ?
                GetDispatcher().Invoke(callback) :
                callback.Invoke();
        }

        public static DispatcherOperation<Task> InvokeAsync(Func<Task> callback)
        {
            return GetDispatcher().InvokeAsync(callback);
        }

        public static DispatcherOperation<Task<T>> InvokeAsync<T>(
            Func<Task<T>> callback)
        {
            return GetDispatcher().InvokeAsync(callback);
        }

        public static DispatcherOperation InvokeAsync(Action callback)
        {
            return GetDispatcher().InvokeAsync(callback);
        }
    }
}