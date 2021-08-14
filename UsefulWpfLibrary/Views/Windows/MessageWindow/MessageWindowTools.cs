using System.Threading;
using System.Windows;

namespace UsefulWpfLibrary.Views.Windows.MessageWindow
{
    public static class MessageWindowTools
    {
        public static MessageBoxResult? Show(string message,
            string? title = null,
            MessageType messageType = MessageType.信息,
            MessageBoxButton messageBoxButton = MessageBoxButton.OK)
        {
            title ??= messageType.ToString();

            MessageWindow messageWindow = null!;
            var thread = new Thread(() =>
            {
                messageWindow = new MessageWindow
                {
                    Title = title,
                    MessageType = messageType,
                    MessageContent = message,
                    MessageBoxButton = messageBoxButton
                };
                messageWindow.ShowDialog();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return messageWindow!.MessageBoxResult;
        }
    }
}