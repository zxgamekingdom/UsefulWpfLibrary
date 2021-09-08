using System;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public record RetryContext<TException>(TException Exception,
        uint TotalRetriedCount,
        uint ExceptionRetriedCount) where TException : Exception;
}