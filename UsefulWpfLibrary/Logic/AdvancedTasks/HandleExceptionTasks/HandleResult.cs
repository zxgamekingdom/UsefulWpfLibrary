namespace UsefulWpfLibrary.Logic.AdvancedTasks.HandleExceptionTasks
{
    public record HandleResult(bool IsHandle)
    {
        public static HandleResult Handle()
        {
            return new HandleResult(true);
        }

        public static HandleResult NotHandle()
        {
            return new HandleResult(false);
        }
    }

    public record HandleResult<TResult>(bool IsHandle, TResult? Result)
    {
        public static HandleResult<TResult> Handle(TResult result)
        {
            return new HandleResult<TResult>(true, result);
        }

        public static HandleResult<TResult?> NotHandle()
        {
            return new HandleResult<TResult?>(false, default);
        }
    }
}
