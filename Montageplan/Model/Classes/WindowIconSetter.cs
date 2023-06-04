using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Montageplan.Model;

namespace Montageplan.Classes
{
    public static class WindowIconSetter
    {
        public static void TrySetIcon(Window window)
        {
            // Nur bei Window7 setzen (Bei XP stürzt das ab)
            try
            {
                if (OSVersionChecker.IsWindows7() || OSVersionChecker.IsWindows8())
                {
                    Uri iconUri = new Uri("pack://application:,,,/montageplan.ico", UriKind.RelativeOrAbsolute);
                    window.Icon = BitmapFrame.Create(iconUri);
                }
            }
            catch (Exception exp)
            {
                AppConfig.Logger.Write("OS konnte nicht ermittelt werden", exp.Message,true);
            }
        }
    }
}



