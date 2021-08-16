using System.Windows;
using System.Windows.Controls;

namespace UsefulWpfLibrary.Views.CustomControls.SingleValueEditor
{
    public class SingleValueEditor : Control
    {
        public static readonly DependencyProperty ValueNameProperty =
            DependencyProperty.Register(nameof(ValueName),
                typeof(string),
                typeof(SingleValueEditor),
                new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value),
                typeof(object),
                typeof(SingleValueEditor),
                new FrameworkPropertyMetadata(default(object),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        static SingleValueEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SingleValueEditor),
                new FrameworkPropertyMetadata(typeof(SingleValueEditor)));
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