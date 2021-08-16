using System.Threading;

namespace UsefulWpfLibrary.Logic.Commands
{
    public class CanExecuteArgs
    {
        public CanExecuteArgs(object? parameter, CancellationToken token)
        {
            Parameter = parameter;
            Token = token;
        }

        public object? Parameter { get; }
        public CancellationToken Token { get; }
    }
}
