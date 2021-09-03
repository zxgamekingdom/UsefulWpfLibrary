using System;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.Logic
{
    public record CheckTaskState
    {
        private Exception? _exception;

        public Exception? Exception
        {
            get
            {
                CheckFinished();
                return _exception;
            }
            set => _exception = value;
        }

        public bool IsFinished { get; private set; }
        public bool IsStarted { get; private set; }

        public void CheckNotStarted()
        {
            if (IsStarted) throw new InvalidOperationException("任务已经开始运行了,无法执行此操作");
        }

        /// <summary>
        /// 设置任务状态为完成
        /// </summary>
        public void Finished()
        {
            if (IsStarted == false)
                throw new InvalidOperationException("任务未启动,不能设置任务的状态为完成");
            IsFinished = true;
        }

        public void CheckFinished()
        {
            if ((IsStarted && IsFinished) is false)
            {
                throw new InvalidOperationException(@"任务状态必须已经开始且已经完成");
            }
        }

        public void CheckIsRunning()
        {
            if ((IsStarted && IsFinished == false) is false)
            {
                throw new InvalidOperationException("任务的状态必须为已经开始且未完成状态");
            }
        }

        public void Run(Action action)
        {
            CheckIsRunning();
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Exception = e;
                throw;
            }
            finally
            {
                Finished();
            }
        }

        public T Run<T>(Func<T> action)
        {
            CheckIsRunning();
            try
            {
                return action.Invoke();
            }
            catch (Exception e)
            {
                Exception = e;
                throw;
            }
            finally
            {
                Finished();
            }
        }

        public async Task Run(Func<Task> func)
        {
            CheckIsRunning();
            try
            {
                await func.Invoke();
            }
            catch (Exception e)
            {
                Exception = e;
                throw;
            }
            finally
            {
                Finished();
            }
        }

        public async Task<T> Run<T>(Func<Task<T>> func)
        {
            CheckIsRunning();
            try
            {
                return await func.Invoke();
            }
            catch (Exception e)
            {
                Exception = e;
                throw;
            }
            finally
            {
                Finished();
            }
        }

        /// <summary>
        /// 设置任务状态为已经开始
        /// </summary>
        public void Start()
        {
            CheckNotStarted();
            if (IsFinished) throw new InvalidOperationException("任务已经完成,无法启动");
            IsStarted = true;
        }
    }
}
