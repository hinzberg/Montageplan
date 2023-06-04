using Montageplan.Model;
using Montageplan.Model.DAL.Database;
using Montageplan.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Montageplan.Presenter
{
    public class SplashPresenter
    {
        private readonly SplashWindow window;
        private bool hasRights;
        private bool isDatabaseOk;

        public SplashPresenter(SplashWindow window)
        {
            this.window = window;
            this.hasRights = false;
            this.isDatabaseOk = false;

            this.window.txtUsername.Focus();
            this.window.Title = "Anmeldung " + AppConfig.AppTitle;
            this.window.btnSubmit.Click += btnSubmit_Click;
            this.window.btnCancel.Click += btnCancel_Click;

            this.window.WindowStyle = WindowStyle.None;
            this.window.AllowsTransparency = true;
            this.window.Opacity = 0;

            window.Loaded += window_Loaded;
            ////// TODO - Nur zum testen
            ////this.window.txtUsername.Text = "Max Mustermann";
            ////this.window.txtPassword.Password = "mustermann";
        }

        void window_Loaded(object sender, RoutedEventArgs e)
        {
            this.window.panelRoot.Visibility = Visibility.Collapsed;
            this.window.buttonsDockPanel.Visibility = Visibility.Collapsed;

            AppConfig.Init();
            AppConfig.Load();
            this.TryUpdateDatabase();

            this.hasRights = true;
            this.window.DialogResult = true;
        }

        public bool HasRights
        {
            get { return this.hasRights; }
        }
        public bool IsDatabaseOk
        {
            get { return this.isDatabaseOk; }
        }

        public void ShowDialog()
        {
            this.window.ShowDialog();
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.window.DialogResult = false;
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            AppConfig.Init();
            this.hasRights = true;
            //this.CheckUserRights();
            if (this.hasRights)
            {
                AppConfig.Load();
                this.TryUpdateDatabase();
            }
            this.window.DialogResult = true;
        }

        private void CheckUserRights()
        {
            string user = this.window.txtUsername.Text.Trim();
            string pwd = this.window.txtPassword.Password.Trim();

            // TODO - Nur zum testen
            if (user == "Max Mustermann" && pwd == "mustermann")
                this.hasRights = true;
            else
            {
                MessageBoxHelper.ShowError("Sie haben keine Berechtigungen für das Programm!");
                AppConfig.Logger.Write("Login ist fehlgeschlagen", string.Format("Benutzer: {0}\r\nPasswort: {1}", user, pwd));
            }
        }

        private void TryUpdateDatabase()
        {
            bool isSucceeded = this.CreateDatabaseIfNotExists();
            if (isSucceeded)
            {
                DatabaseBackupper backupper = new DatabaseBackupper(AppConfig.BackupsDirectory);
                backupper.Create(AppConfig.DatabaseFile);

                DatabaseUpdater database = new DatabaseUpdater();
                database.Update();
                this.isDatabaseOk = database.IsSucceeded;
            }
            else
                MessageBoxHelper.ShowError("Es konnte keine Datenbank erstellt werden!");
        }

        private bool CreateDatabaseIfNotExists()
        {
            bool isSucceeded = false;
            try
            {
                string dbFile = AppConfig.RootDirectory + @"\" + AppConfig.DATABASE_NAME;
                SqlCeDatabaseCreator db = new SqlCeDatabaseCreator(dbFile, AppConfig.DATABASE_PWD);
                db.CreateIfNotExists();
                isSucceeded = true;
            }
            catch (Exception exp)
            {
                AppConfig.Logger.Write("Datenbankerstellung", exp.Message, true);
                MessageBoxHelper.ShowError(exp.Message);
            }
            return isSucceeded;
        }

    }
}
