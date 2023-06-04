using System;
using System.Windows;
using Montageplan.Classes;
using Montageplan.Model;

namespace Montageplan.View.MessageBox
{
    public enum MessageBoxWindowIcon { Information, Question, Warning, Error }
    public enum MessageBoxWindowButton { Ok, YesNo }

    public partial class MessageBoxWindow : Window
    {
        public MessageBoxWindow()
        {
            InitializeComponent();
            this.mainText.Text = "";
            this.AdditionalMessage = "";
            WindowIconSetter.TrySetIcon(this);
            this.SetIconStyle(MessageBoxWindowIcon.Information);
            this.SetButtonStyle(MessageBoxWindowButton.Ok);
            this.Title = AppConfig.APP_NAME;
            this.btnOk.Click += BtnOkClick;
            this.btnNo.Click += BtnNoClick;
        }

        void BtnNoClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void BtnOkClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string MainMessage { get; set; }
        public string AdditionalMessage { get; set; }
        public MessageBoxWindowIcon WindowIcon
        {
            set
            {
                this.SetIconStyle(value);
            }
        }

        public MessageBoxWindowButton WindowButton
        {
            set
            {
                this.SetButtonStyle(value);
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.subText.Text = this.AdditionalMessage;
            this.mainText.Text = this.MainMessage;

            if (string.IsNullOrEmpty(this.AdditionalMessage))
                this.subText.Visibility = Visibility.Collapsed;

            if (string.IsNullOrEmpty(this.MainMessage))
                this.mainText.Visibility = Visibility.Collapsed;
        }

        private void SetButtonStyle(MessageBoxWindowButton buttonStyle)
        {
            this.btnOk.Visibility = Visibility.Collapsed;
            this.btnNo.Visibility = Visibility.Collapsed;

            switch (buttonStyle)
            {
                case MessageBoxWindowButton.Ok:
                    this.btnOk.Visibility = Visibility.Visible;
                    break;
                case MessageBoxWindowButton.YesNo:
                    this.btnOk.Visibility = Visibility.Visible;
                    this.btnNo.Visibility = Visibility.Visible;
                    this.btnOk.Content = "Ja";
                    break;
            }
        }

        private void SetIconStyle(MessageBoxWindowIcon icon)
        {
            this.iconInfo.Visibility = Visibility.Collapsed;
            this.iconQuestion.Visibility = Visibility.Collapsed;
            this.iconWarning.Visibility = Visibility.Collapsed;
            this.iconError.Visibility = Visibility.Collapsed;
            this.iconInfo.Visibility = Visibility.Collapsed;

            switch (icon)
            {
                case MessageBoxWindowIcon.Information:
                    this.iconInfo.Visibility = Visibility.Visible;
                    break;
                case MessageBoxWindowIcon.Question:
                    this.iconQuestion.Visibility = Visibility.Visible;
                    break;
                case MessageBoxWindowIcon.Warning:
                    this.iconWarning.Visibility = Visibility.Visible;
                    break;
                case MessageBoxWindowIcon.Error:
                    this.iconError.Visibility = Visibility.Visible;
                    break;
                default:
                    this.iconInfo.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}