using System;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.Logic
{
    public class CheckTaskState
    {
        public bool IsStarted { get; private set; }

        public void Start()
        {
            CheckState();
            IsStarted = true;
        }

        public void CheckState()
        {
            if (IsStarted)
            {
                throw new InvalidOperationException("任务已经开始运行了,无法执行此操作");
            }
        }
    }
}
