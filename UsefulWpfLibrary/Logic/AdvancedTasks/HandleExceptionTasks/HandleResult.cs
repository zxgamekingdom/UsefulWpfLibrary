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
}