namespace UsefulWpfLibrary.Logic.AdvancedTasks.HandleExceptionTasks
{
    public record HandleReturnResult<TResult>(bool IsHandle, TResult? Result)
    {
        public static HandleReturnResult<TResult> Handle(TResult result)
        {
            return new HandleReturnResult<TResult>(true, result);
        }

        public static HandleReturnResult<TResult> NotHandle()
        {
            return new HandleReturnResult<TResult>(false, default);
        }
    }
}