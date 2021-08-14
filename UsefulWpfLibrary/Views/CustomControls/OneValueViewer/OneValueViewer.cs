using System.Windows;
using System.Windows.Controls;

namespace UsefulWpfLibrary.Views.CustomControls.OneValueViewer
{
    public class OneValueViewer : Control
    {
        static OneValueViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OneValueViewer),
                new FrameworkPropertyMetadata(typeof(OneValueViewer)));
        }

        public static readonly DependencyProperty ValueNameProperty =
            DependencyProperty.Register(nameof(ValueName),
                typeof(string),
                typeof(OneValueViewer),
                new PropertyMetadata(default(string)));

        public string ValueName
        {
            get => (string) GetValue(ValueNameProperty);
            set => SetValue(ValueNameProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value),
                typeof(object),
                typeof(OneValueViewer),
                new PropertyMetadata(default(object)));

        public object Value
        {
            get => (object) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
    }
}
