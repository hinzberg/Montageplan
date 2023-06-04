using System.Windows;
using Montageplan.View.MessageBox;

namespace Montageplan.Model
{
    /// <summary>
    /// Wrapper zum vereinfachten Handling der MessageBox.
    /// </summary>
    public static class MessageBoxHelper
    {
        public static void ShowError(string message)
        {
            ShowMessageBox(message, MessageBoxImage.Error);
        }

        public static void ShowWarning(string message)
        {
            ShowMessageBox(message, MessageBoxImage.Warning);
        }

        public static void ShowInformation(string message)
        {
            ShowMessageBox(message, MessageBoxImage.Information);
        }

        private static void ShowMessageBox(string message, MessageBoxImage image)
        {
            MessageBox.Show(message, AppConfig.APP_NAME, MessageBoxButton.OK, image);
        }

        public static bool ShowYesNoQuestionWindow(string message, string additionalMessage, Window owner)
        {
            MessageBoxWindow window = new MessageBoxWindow();
            if (owner != null)
                window.Owner = owner;
            window.MainMessage = message;
            window.AdditionalMessage = additionalMessage;
            window.WindowIcon = MessageBoxWindowIcon.Question;
            window.WindowButton = MessageBoxWindowButton.YesNo;
            window.ShowDialog();

            if (window.DialogResult == null || window.DialogResult == false)
                return false;

            return true;
        }

        public static void ShowOkBoxWindow(string message, string additionalMessage, Window owner)
        {
            MessageBoxWindow window = new MessageBoxWindow();
            if (owner != null)
                window.Owner = owner;
            window.MainMessage = message;
            window.AdditionalMessage = additionalMessage;
            window.WindowIcon = MessageBoxWindowIcon.Warning;

            window.ShowDialog();
        }
    }
}