namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public static partial class ParallelTask
    {
        public static IParallelTask Create()
        {
            return new InternalParallelTask();
        }
    }
}
