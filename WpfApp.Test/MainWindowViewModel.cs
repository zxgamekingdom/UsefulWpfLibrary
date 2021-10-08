using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;
using UsefulWpfLibrary.Logic.Extensions;
using UsefulWpfLibrary.Logic.ViewModels;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options;

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

        public ICommand CommandDispose { get; }
        public ICommand CommandIAdd { get; }
        public ICommand CommandOpenWindow1 { get; }
        public ICommand CommandUpdateTime { get; }
        public decimal I { get; set; }
        public TestInfo Info { get; } = new();
        public DateTime Time { get; set; }

        protected override void OnDispose()
        {
            Info.Dispose();
        }

        public class TestInfo : AdvancedViewModelBase, IGetMultiValueEditorOptions
        {
            public TestInfo()
            {
                CommandShow = CreateCommand(() =>
                    this.JsonSerialize(Formatting.Indented).ShowInfoMessage());
                CommandAdd = CreateCommand(() => I1 += 100);
            }

            [Ignore]
            [JsonIgnore]
            public ICommand CommandAdd { get; }

            [Ignore]
            [JsonIgnore]
            public ICommand CommandShow { get; }

            [PropertyName("III")]
            [TitleMenuItemUseCommandName("Add", nameof(CommandAdd))]
            [Order(0)]
            public int I1 { get; set; }

            [PropertyName("III2")]
            [Order(1)]
            public int I2 { get; set; }

            [Order(2)]
            public bool B { get; set; }

            public MultiValueEditorOptions GetOptions()
            {
                return new MultiValueEditorOptions()
                {
                    AfterCreateDefaultTitleControl = args =>
                    {
                        if (args.DefaultTitleControl is TextBlock textBlock && args.PropertyInfo?.Name is nameof(I2))
                        {
                            textBlock.Background = Brushes.Red;
                        }
                    },
                    AfterCreateDefaultSingleValueEditor = (args) =>
                    {
                        if (args.DefaultSingleValueEditor is Control control)
                        {
                            control.Foreground = Brushes.Red;
                        }
                    }
                };
            }
        }
    }
}
