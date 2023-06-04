using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Montageplan.Model;
using Montageplan.View.Windows;
using Montageplan.ViewModel;
using Montageplan.Windows;
using System.Windows.Data;
using System.ComponentModel;
using Montageplan.Model.DAL.Database;

namespace Montageplan.Presenter
{
    internal class UserListPresenter
    {
        private readonly UserRepository users;
        private readonly UserListWindow window;
        private ObservableCollection<UserViewModel> userViewModels;

        public UserListPresenter(UserListWindow win, UserRepository userep)
        {
            this.window = win;
            this.users = userep;
            (this.window.userListView.Items as CollectionView).SortDescriptions.Add(new SortDescription("Username", ListSortDirection.Ascending));

            this.window.mainToolbar.NewButtonText = "Neuer Nutzer";
            this.window.mainToolbar.NewButtonClicked += MainToolbarNewButtonClicked;
            this.window.mainToolbar.DeleteButtonClicked += MainToolbarDeleteButtonClicked;
            this.window.mainToolbar.EditButtonClicked += MainToolbarEditButtonClicked;

            // Contextmenü-Events
            this.window.newUserMenuItem.Click += NewUserMenuItemClick;
            this.window.editUserMenuItem.Click += EditUserMenuItemClick;
            this.window.deleteUserMenuItem.Click += DeleteUserMenuItemClick;
            this.window.userListView.ContextMenuOpening += UserListViewContextMenuOpening;
            this.window.userListView.SelectionChanged += UserListViewSelectionChanged;

            this.window.btnSubmit.Click += BtnSubmitClick;
            this.window.userListView.AddHandler(
                ListViewItem.MouseDoubleClickEvent, new MouseButtonEventHandler(this.ListViewItemMouseDoubleClick), true);

            this.ValidateToolbarButtons();
        }

        public void ShowDialog()
        {
            this.UpdateView();
            this.window.ShowDialog();
        }

        private void UpdateView()
        {
            userViewModels = new ObservableCollection<UserViewModel>(UserViewModel.CreateViewModels(this.users.GetAll()));
            this.window.userListView.ItemsSource = userViewModels;
        }

        public bool GetDialogResult()
        {
            return this.window.DialogResult.Value;
        }

        private void BtnSubmitClick(object sender, RoutedEventArgs e)
        {
            this.window.DialogResult = true;
        }

        void UserListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ValidateToolbarButtons();
        }

        private void ValidateToolbarButtons()
        {
            if (this.window.userListView.SelectedItems.Count > 0)
            {
                this.window.mainToolbar.SetEnabledDeleteButton(true);
                this.window.mainToolbar.SetEnabledEditButton(true);
            }
            else
            {
                this.window.mainToolbar.SetEnabledDeleteButton(false);
                this.window.mainToolbar.SetEnabledEditButton(false);
            }
        }

        void MainToolbarNewButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ShowNewUserWindow();
        }

        void MainToolbarEditButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ShowEditUserWindow();
        }

        void MainToolbarDeleteButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DeleteSelectedUsers();
        }

        private void ListViewItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.ShowEditUserWindow();
        }

        private void NewUserMenuItemClick(object sender, RoutedEventArgs e)
        {
            this.ShowNewUserWindow();
        }

        private void EditUserMenuItemClick(object sender, RoutedEventArgs e)
        {
            this.ShowEditUserWindow();
        }

        private void UserListViewContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            bool isItemSelected = (this.window.userListView.SelectedItems.Count == 0);
            this.window.editUserMenuItem.IsEnabled = !isItemSelected;
            this.window.deleteUserMenuItem.IsEnabled = !isItemSelected;
        }

        private UserViewModel GetFirstSelected()
        {
            UserViewModel selected = null;
            if (this.window.userListView.SelectedItems.Count > 0)
                selected = (UserViewModel)this.window.userListView.SelectedItem;
            return selected;
        }

        private List<UserViewModel> GetSelection()
        {
            List<UserViewModel> selection = new List<UserViewModel>();
            foreach (var item in this.window.userListView.SelectedItems)
                selection.Add((UserViewModel)item);

            return selection;
        }

        /// <summary>
        /// Neuen Nutzer anlegen
        /// </summary>
        private void ShowNewUserWindow()
        {
            UserEditWindow window = new UserEditWindow();
            window.Owner = this.window;
            UserEditPresenter presenter = new UserEditPresenter(window, this.users);
            presenter.CreateNewUser();
            presenter.ShowDialog();
            if (presenter.GetDialogResult())
            {
                User u = presenter.GetUser();
                // Nur gültig wenn Benutzername eingetragen.
                if (!string.IsNullOrEmpty(u.Username))
                {
                    this.users.Add(u);
                    this.userViewModels.Add(new UserViewModel(u));
                    UsersDbTable usersTable = new UsersDbTable();
                    usersTable.Insert(u);
                }
            }
        }

        /// <summary>
        /// Ausgewählten Nutzer bearbeiten
        /// </summary>
        private void ShowEditUserWindow()
        {
            UserViewModel uservm = this.GetFirstSelected();
            if (uservm != null)
            {
                UserEditWindow window = new UserEditWindow();
                window.Owner = this.window;
                UserEditPresenter presenter = new UserEditPresenter(window, this.users);
                presenter.EditUser(uservm.GetModel());
                presenter.ShowDialog();
                if (presenter.GetDialogResult())
                {
                    uservm.NotifyPropertyChanged();
                    UsersDbTable usersTable = new UsersDbTable();
                    usersTable.Update(uservm.GetModel());
                }
            }
        }

        private void DeleteSelectedUsers()
        {
            List<UserViewModel> selection = this.GetSelection();
            if (selection.Count > 0)
            {
                UsersDbTable usersTable = new UsersDbTable();
                foreach (var item in selection)
                {
                    User deletedUser = item.GetModel();

                    this.userViewModels.Remove(item);
                    this.users.Remove(deletedUser);
                    usersTable.Delete(deletedUser);
                }
            }
        }

        /// <summary>
        /// Nutzer löschen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteUserMenuItemClick(object sender, RoutedEventArgs e)
        {
            this.DeleteSelectedUsers();
        }

    }
}