using System.Threading;
using UsefulWpfLibrary.Logic;
using UsefulWpfLibrary.Logic.Tools;

namespace WpfApp.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
#pragma warning disable IDE0052 // 删除未读的私有成员
        private readonly Mutex _mutex;
#pragma warning restore IDE0052 // 删除未读的私有成员

        public App()
        {
            _ = ConsoleTools.AllocConsole();
            Ioc.Init(_ =>
            {
            });
            InitializeComponent();
#pragma warning disable DF0021 // Marks undisposed objects assinged to a field, originated from method invocation.
            _mutex = SingletonProgramTools.GenToken(
                "26CC549B-ACED-41DD-ADAF-6F252EC835F4");
#pragma warning restore DF0021 // Marks undisposed objects assinged to a field, originated from method invocation.
        }
    }
}
