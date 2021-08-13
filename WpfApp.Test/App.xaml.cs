using System.Threading;
using System.Windows;
using UsefulWpfLibrary.Logic.TasksHelpers;
using UsefulWpfLibrary.Logic.Tools;
using UsefulWpfLibrary.Views.Logic;
using UsefulWpfLibrary.Views.Windows.MessageWindow;

namespace WpfApp.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Mutex _mutex;

        public App()
        {
            InitializeComponent();
            AdvancedTaskRun.Run(token =>
            {
                MessageBoxResult? messageBoxResult =
                    MessageWindowTools.Show("1", messageType: MessageType.错误);
                MessageWindowTools.Show(messageBoxResult.ToString());
            });
            AdvancedTaskRun.Run(token =>
                MessageWindowTools.Show("2", messageType: MessageType.信息));
            AdvancedTaskRun.Run(token =>
                MessageWindowTools.Show("3", messageType: MessageType.警告));
            _mutex = SingletonProgramTools.GenToken(
                "26CC549B-ACED-41DD-ADAF-6F252EC835F4");
        }
    }
}
