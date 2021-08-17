using System;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.Tools
{
    public static class CatchExceptionTools
    {
        public static Exception? Run(Action action)
        {
            try
            {
                action.Invoke();
                return default;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public static (Exception? exception, T? result) Run<T>(Func<T> func)
        {
            try
            {
                return (default, func.Invoke());
            }
            catch (Exception e)
            {
                return (e, default);
            }
        }

        public static async Task<Exception?> Run(Func<Task> func)
        {
            try
            {
                await func.Invoke();
                return default;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public static async Task<(Exception? exception, T? result)> Run<T>(
            Func<Task<T>> func)
        {
            try
            {
                return (default, await func.Invoke());
            }
            catch (Exception e)
            {
                return (e, default);
            }
        }
    }
}
