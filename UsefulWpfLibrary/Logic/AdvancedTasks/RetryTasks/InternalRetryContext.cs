using System;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    internal record InternalRetryContext(Exception Exception,
        uint TotalRetriedCount,
        uint ExceptionRetriedCount)
    {
        internal object Convert()
        {
            Type te = Exception.GetType();
            Type t = typeof(RetryContext<>);
            Type genericType = t.MakeGenericType(te);
            return Activator.CreateInstance(genericType,
                   Exception,
                   TotalRetriedCount,
                   ExceptionRetriedCount) ??
                throw new InvalidOperationException();
        }
    }
}