using System.Windows;
using System.Windows.Controls;

namespace UsefulWpfLibrary.Views.CustomControls.OneValueViewer
{
    public class OneValueViewer : Control
    {
        public static readonly DependencyProperty ValueNameProperty =
            DependencyProperty.Register(nameof(ValueName),
                typeof(string),
                typeof(OneValueViewer),
                new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value),
                typeof(object),
                typeof(OneValueViewer),
                new PropertyMetadata(default(object)));
        static OneValueViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OneValueViewer),
                new FrameworkPropertyMetadata(typeof(OneValueViewer)));
        }
        public object Value
        {
            get => (object)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        public string ValueName
        {
            get => (string)GetValue(ValueNameProperty);
            set => SetValue(ValueNameProperty, value);
        }
    }
}