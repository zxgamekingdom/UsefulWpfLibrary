using System.Threading;
using System.Windows;

using UsefulWpfLibrary.Logic;
using UsefulWpfLibrary.Logic.Tools;

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
            Ioc.Init(registry =>
            {
            });
            InitializeComponent();
            _mutex = SingletonProgramTools.GenToken(
                "26CC549B-ACED-41DD-ADAF-6F252EC835F4");
        }
    }
}