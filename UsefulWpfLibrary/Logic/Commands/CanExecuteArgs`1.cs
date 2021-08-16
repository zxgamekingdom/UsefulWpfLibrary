using System.Threading;

namespace UsefulWpfLibrary.Logic.Commands
{
    public class CanExecuteArgs<T>
    {
        public CanExecuteArgs(T? parameter, CancellationToken token)
        {
            Parameter = parameter;
            Token = token;
        }

        public T? Parameter { get; }
        public CancellationToken Token { get; }
    }
}
