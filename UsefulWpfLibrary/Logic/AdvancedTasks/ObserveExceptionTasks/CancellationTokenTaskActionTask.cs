﻿using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        private class CancellationTokenTaskActionTask : AbstractActionTask
        {
            public CancellationTokenTaskActionTask(Func<CancellationToken, Task> func) :
                base(func)
            {
            }
        }
    }
}
