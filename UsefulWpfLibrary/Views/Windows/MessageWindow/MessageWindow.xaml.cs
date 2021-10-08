using System;
using System.Windows;
using System.Windows.Input;

namespace UsefulWpfLibrary.Views.Windows.MessageWindow
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    partial class MessageWindow
    {
        public static readonly DependencyProperty MessageBoxButtonProperty =
            DependencyProperty.Register(nameof(MessageBoxButton),
                typeof(MessageBoxButton),
                typeof(MessageWindow),
                new PropertyMetadata(default(MessageBoxButton),
                    (o, args) =>
                    {
                        var window = (MessageWindow)o;
                        var messageBoxButton = (MessageBoxButton)args.NewValue;
                        window.OkButton.Visibility = Visibility.Collapsed;
                        window.YesButton.Visibility = Visibility.Collapsed;
                        window.NoButton.Visibility = Visibility.Collapsed;
                        window.CancelButton.Visibility = Visibility.Collapsed;
                        switch (messageBoxButton)
                        {
                            case MessageBoxButton.OK:
                                window.OkButton.Visibility = Visibility.Visible;
                                break;
                            case MessageBoxButton.OKCancel:
                                window.OkButton.Visibility = Visibility.Visible;
                                window.CancelButton.Visibility = Visibility.Visible;
                                break;
                            case MessageBoxButton.YesNoCancel:
                                window.YesButton.Visibility = Visibility.Visible;
                                window.NoButton.Visibility = Visibility.Visible;
                                window.CancelButton.Visibility = Visibility.Visible;
                                break;
                            case MessageBoxButton.YesNo:
                                window.YesButton.Visibility = Visibility.Visible;
                                window.NoButton.Visibility = Visibility.Visible;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }));
        public static readonly DependencyProperty MessageContentProperty =
            DependencyProperty.Register(nameof(MessageContent),
                typeof(string),
                typeof(MessageWindow),
                new PropertyMetadata(default(string)));
        public static readonly DependencyProperty MessageTypeProperty =
            DependencyProperty.Register(nameof(MessageType),
                typeof(MessageType),
                typeof(MessageWindow),
                new PropertyMetadata(default(MessageType)));
        public MessageWindow()
        {
            InitializeComponent();
        }

        public MessageBoxButton MessageBoxButton
        {
            get => (MessageBoxButton)GetValue(MessageBoxButtonProperty);
            set => SetValue(MessageBoxButtonProperty, value);
        }
        public MessageBoxResult? MessageBoxResult { get; private set; } =
                   System.Windows.MessageBoxResult.None;
        public string MessageContent
        {
            get => (string)GetValue(MessageContentProperty);
            set => SetValue(MessageContentProperty, value);
        }

        public MessageType MessageType
        {
            get => (MessageType)GetValue(MessageTypeProperty);
            set => SetValue(MessageTypeProperty, value);
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (Equals(sender, OkButton))
            {
                MessageBoxResult = System.Windows.MessageBoxResult.OK;
            }
            else if (Equals(sender, YesButton))
            {
                MessageBoxResult = System.Windows.MessageBoxResult.Yes;
            }
            else if (Equals(sender, NoButton))
            {
                MessageBoxResult = System.Windows.MessageBoxResult.No;
            }
            else if (Equals(sender, CancelButton))
            {
                MessageBoxResult = System.Windows.MessageBoxResult.Cancel;
            }

            Close();
        }
        private void CloseBorder_OnMouseLeftButtonUp(object sender,
                    MouseButtonEventArgs e)
        {
            Close();
        }
    }
}