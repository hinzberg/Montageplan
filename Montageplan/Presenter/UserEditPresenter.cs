using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Montageplan.Model;
using Montageplan.View.Windows;

namespace Montageplan.Presenter
{
    public class UserEditPresenter
    {
        private UserEditWindow window;
        private UserRepository userRepository;
        private User selectedUser;

        public UserEditPresenter(UserEditWindow win, UserRepository users)
        {
            this.userRepository = users;
            this.window = win;
            this.window.btnCancel.Click += BtnCancelClick;
            this.window.btnSubmit.Click += BtnSubmitClick;
        }

        void BtnSubmitClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.UpdateModel();
            this.window.DialogResult = true;
        }

        void BtnCancelClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.window.DialogResult = false;
        }

        public void ShowDialog()
        {
            this.UpdateView();
            this.window.ShowDialog();
        }

        private void UpdateModel()
        {
            this.selectedUser.Username = this.window.txtUsername.Text.Trim();
            this.selectedUser.Password = this.window.txtPassword.Password.Trim();
            this.selectedUser.IsAdministrator = this.window.checkIsAdmin.IsChecked.Value;
        }

        private void UpdateView()
        {
            if (this.selectedUser != null)
            {
                this.window.txtUsername.Text = this.selectedUser.Username;
                this.window.txtPassword.Password = this.selectedUser.Password;
                this.window.checkIsAdmin.IsChecked = this.selectedUser.IsAdministrator;
            }
        }

        public void CreateNewUser()
        {
            this.selectedUser = new User();
            this.window.Title = "Neuer Nutzer";
            this.window.txtUsername.IsEnabled = true;
            this.UpdateView();
        }

        public User GetUser()
        {
            return this.selectedUser;
        }

        public void EditUser(User user)
        {
            this.selectedUser = user;
            this.window.Title = "Nutzer bearbeiten";
            this.window.txtUsername.IsEnabled = false;
            this.UpdateView();
        }

        public bool GetDialogResult()
        {
            return this.window.DialogResult.Value;
        }

    }
}
