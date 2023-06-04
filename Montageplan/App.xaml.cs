using Montageplan.Model;
using Montageplan.Model.DAL.Database;
using Montageplan.Presenter;
using Montageplan.View.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace Montageplan
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            SplashPresenter splashPresenter = null;
            try
            {
                splashPresenter = new SplashPresenter(new SplashWindow());
                splashPresenter.ShowDialog();
            }
            catch (Exception exp)
            {
                splashPresenter = null;
                MessageBoxHelper.ShowError(exp.Message);
                if (AppConfig.Logger != null)
                    AppConfig.Logger.Write("Splash-Exception", exp.Message, true);
            }

            if (splashPresenter != null && splashPresenter.HasRights && splashPresenter.IsDatabaseOk)
            {
                try
                {
                    MainPresenter presenter = new MainPresenter(new MainWindow());
                    presenter.ShowDialog();
                }
                catch (Exception exp)
                {
                    AppConfig.Logger.Write("Main-Exception", exp.Message, true);
                    MessageBoxHelper.ShowError(exp.Message);
                }
            }
            this.Shutdown();
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string expMessage = "Kein Exception-Objekt vorhanden";
            if (e.Exception != null)
                expMessage = e.Exception.Message;

            AppConfig.Logger.Write("App-UnhandledException", expMessage, true);
        }

    }
}
