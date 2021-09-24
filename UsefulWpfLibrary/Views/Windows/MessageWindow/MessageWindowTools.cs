using System.Windows;
using System.Windows.Threading;

namespace UsefulWpfLibrary.Views.Windows.MessageWindow
{
    public static class MessageWindowTools
    {
        public static DispatcherOperation<MessageBoxResult?> Show(string message,
            string? title = null,
            MessageType messageType = MessageType.信息,
            MessageBoxButton messageBoxButton = MessageBoxButton.OK)
        {
            title ??= messageType.ToString();
            return Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MessageWindow messageWindow = new()
                {
                    Title = title,
                    MessageType = messageType,
                    MessageContent = message,
                    MessageBoxButton = messageBoxButton
                };
                _ = messageWindow.ShowDialog();
                return messageWindow.MessageBoxResult;
            });
        }
    }
}
