using System;
using System.Threading.Tasks;
using System.Windows.Input;
using UsefulWpfLibrary.Logic.ViewModels;

namespace WpfApp.Test
{
    public class MainWindowViewModel : AdvancedViewModelBase
    {
        private DateTime _buffTime;

        public MainWindowViewModel()
        {
            CommandUpdateTime = CreateCommand(() =>
                {
                    _buffTime = DateTime.Now;
                    Time = DateTime.Now;
                },
                _ => Task.FromResult(
                    DateTime.Now - _buffTime >= TimeSpan.FromSeconds(1)));
            CommandIAdd = CreateCommand(() => I++);
            CommandOpenWindow1 = CreateCommand(() => new Window1().ShowDialog());
            CommandDispose = CreateCommand(Dispose);
        }

        public ICommand CommandIAdd { get; }
        public ICommand CommandOpenWindow1 { get; }
        public ICommand CommandDispose { get; }
        public ICommand CommandUpdateTime { get; }
        public decimal I { get; set; }
        public DateTime Time { get; set; }
    }
}
