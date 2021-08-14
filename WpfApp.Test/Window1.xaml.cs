using System.Windows;

using UsefulWpfLibrary.Logic.Extensions;
using UsefulWpfLibrary.Logic.ViewModels;

namespace WpfApp.Test
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
    }

    public class Window1ViewModel : AdvancedViewModelBase, ISetDataContextToolsOptions
    {
        public DataContextToolsOptions Options { get; } =
            new DataContextToolsOptions() { };
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            $@"{nameof(Window1)} {nameof(Dispose)}".WriteLine();
        }
    }
}