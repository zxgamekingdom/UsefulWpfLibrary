using System.Windows;
using System.Windows.Controls;

namespace UsefulWpfLibrary.Views.CustomControls.SingleValueViewer
{
    public class SingleValueViewer : Control
    {
        public static readonly DependencyProperty ValueNameProperty =
            DependencyProperty.Register(nameof(ValueName),
                typeof(string),
                typeof(SingleValueViewer),
                new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value),
                typeof(object),
                typeof(SingleValueViewer),
                new PropertyMetadata(default(object)));
        static SingleValueViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SingleValueViewer),
                new FrameworkPropertyMetadata(typeof(SingleValueViewer)));
        }
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        public string ValueName
        {
            get => (string)GetValue(ValueNameProperty);
            set => SetValue(ValueNameProperty, value);
        }
    }
}