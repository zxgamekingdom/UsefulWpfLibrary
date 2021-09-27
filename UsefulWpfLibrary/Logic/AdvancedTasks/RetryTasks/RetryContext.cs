using System;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public record RetryContext<TException>(TException Exception,
        uint TotalRetriesCount,
        uint ThisExceptionRetriesCount) where TException : Exception;

    public record RetryContext(Exception Exception,
        uint TotalRetriesCount,
        uint ThisExceptionRetriesCount);
}
