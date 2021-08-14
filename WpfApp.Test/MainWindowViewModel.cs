using System;
using System.Windows.Input;
using UsefulWpfLibrary.Logic.ViewModels;

namespace WpfApp.Test
{
    public class MainWindowViewModel : AdvancedViewModelBase
    {
        private DateTime _buffTime;
        public DateTime Time { get; set; }

        public MainWindowViewModel()
        {
            CommandUpdateTime = CreateCommand(() =>
                {
                    _buffTime = DateTime.Now;
                    Time = DateTime.Now;
                },
                (_, _) => DateTime.Now - _buffTime >= TimeSpan.FromSeconds(10));
            CommandIAdd = CreateCommand(() =>
            {
                if (I > 1)
                {
                    I /= 999;
                }
                else
                {
                    I *= 999;
                }
            });
            CommandOpenWindow1 = CreateCommand(() =>
            {
                new Window1().ShowDialog();
            });
        }

        public decimal I { get; set; }
        public ICommand CommandUpdateTime { get; }
        public ICommand CommandIAdd { get; }
        public ICommand CommandOpenWindow1 { get; }
    }
}
