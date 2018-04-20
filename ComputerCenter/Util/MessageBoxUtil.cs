using System.Windows;

namespace ComputerCenter.Util
{
    public static class MessageBoxUtil
    {
        public static MessageBoxResult PromptForDelete(string message)
        {
            return MessageBox.Show(message, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        }
    }
}