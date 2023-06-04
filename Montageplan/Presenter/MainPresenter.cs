using System.Windows.Controls;
using System.Windows.Documents;
using Montageplan.Print;
using Montageplan.View.Windows;
using Montageplan.ViewModel;
using Montageplan.EasyOptions;
using Montageplan.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Montageplan.Model;
using System.Windows;
using Montageplan.Model.DAL.Database;

namespace Montageplan.Presenter
{
    public class MainPresenter
    {
        // Das Hauptfenster
        private readonly MainWindow window;
        // Die Kalender-Ansicht inklusive Naviagtionsleiste.
        private readonly CalendarPresenter calenderPresenter;
        private readonly RepositoryContainer repositories;
        private readonly MontageCalendar calendar;
        private readonly ProjectsPresenter projectsPresenter;
        private readonly MainPrinter mainPrinter;

        public MainPresenter(MainWindow window)
        {
            this.window = window;
            this.repositories = new RepositoryContainer();
            this.calendar = new MontageCalendar(this.repositories);
            this.FillRepositoriesWithDummies();
            this.LoadEntitiesFromDatabase();
            this.projectsPresenter = new ProjectsPresenter(window.projectsView);
            this.projectsPresenter.ProjectSelectionChanged = new Action<ViewModel.ProjektViewModelEventArgs>(this.NotifyProjektSelectionChanged);
            this.calenderPresenter = new CalendarPresenter(this.window.calenderView, this.calendar, this.repositories.Engagements);
            this.mainPrinter = new MainPrinter(this.window, this.repositories);
            this.window.Title = AppConfig.AppTitle;


            // Menüpunkte deaktievieren wenn keine Rechte.
            this.window.MenuGroupVerwaltung.SubmenuOpened += MenuGroupVerwaltungSubmenuOpened;
            this.window.mainMenuCrew.Click += MainMenuCrewClick;
            this.window.mainMenuProjekt.Click += MainMenuProjektClick;
            this.window.mainMenuStaff.Click += MainMenuStaffClick;
            this.window.mainMenuFunctions.Click += MainMenuFunctionsClick;
            this.window.mainMenuViewOptions.Click += MainMenuViewOptionsClick;
            this.window.mainMenuPrintReport.Click += MainMenuPrintReportClick;
            this.window.mainMenuUsers.Click += MainMenuUsersClick;
            this.window.Closed += window_Closed;
            this.window.showHideMitarbeiterMenuItem.Click += showHideMitarbeiterMenuItem_Click;
        }


        public void ShowDialog()
        {
            this.UpdateView();
            this.UpdateShowHideMitarbeiterMenuItemText();
            this.window.ShowDialog();
        }

        private void FillRepositoriesWithDummies()
        {
            //this.repositories.Projekte.SetCollection(Montageplan._Test.ObjectFactory.CreateProjektListe());
            //this.repositories.Funktionen.SetCollection(Montageplan._Test.ObjectFactory.CreateFunktionen());
            //this.repositories.Mitarbeiter.SetCollection(Montageplan._Test.ObjectFactory.CreateMitarbeiterList(this.repositories.Funktionen.GetAll()));
            //this.repositories.Kolonnen.SetCollection(Montageplan._Test.ObjectFactory.CreateKolonnenListe());
            //this.repositories.Users.SetCollection(_Test.ObjectFactory.CreateUsers(this.repositories.Users.GetAll()));
        }

        private void LoadEntitiesFromDatabase()
        {
            try
            {
                TryLoadEntitiesFromDatabase();
            }
            catch (Exception exp)
            {
                AppConfig.Logger.Write("LoadEntitiesFromDatabase", exp.Message, true);
                MessageBoxHelper.ShowError(exp.Message);
            }
        }

        private void TryLoadEntitiesFromDatabase()
        {
            this.repositories.Projekte.ClearAll();

            UsersDbTable usersTable = new UsersDbTable();
            List<User> users = usersTable.RealAll();
            usersTable.CloseConnection();
            this.repositories.Users.SetCollection(users);

            ProjekteDbTable projectTable = new ProjekteDbTable();
            List<Projekt> projekte = projectTable.ReadAll();
            projectTable.CloseConnection();
            this.repositories.Projekte.SetCollection(projekte);

            FunktionenDbTable funktionenTable = new FunktionenDbTable();
            List<MitarbeiterFunktion> funktionen = funktionenTable.RealAll();
            funktionenTable.CloseConnection();
            this.repositories.Funktionen.SetCollection(funktionen);

            FehlzeitenDbTable fehlzeitenTable = new FehlzeitenDbTable();
            List<MitarbeiterFehlzeit> fehlzeiten = fehlzeitenTable.RealAll();
            fehlzeitenTable.CloseConnection();

            FehlzeitartenDbTable artenTable = new FehlzeitartenDbTable();
            List<Fehlzeitart> arten = artenTable.RealAll();
            artenTable.CloseConnection();
            this.repositories.Fehlzeitarten.SetCollection(arten);

            MitarbeiterDbTable mitarbeiterTable = new MitarbeiterDbTable();
            List<Mitarbeiter> mitarbeiter = mitarbeiterTable.RealAll();
            mitarbeiterTable.CloseConnection();
            this.repositories.Mitarbeiter.SetCollection(mitarbeiter);

            // Funktionen und Fehlzeiten den Mitarbeitern zuweisen
            foreach (var mitarbItem in this.repositories.Mitarbeiter.GetAll())
            {
                if (mitarbItem.Funktion != null)
                {
                    MitarbeiterFunktion funkt = this.repositories.Funktionen.GetFunktion(mitarbItem.Funktion.Id);
                    if (funkt != null)
                        mitarbItem.Funktion = funkt;
                }
                foreach (var fehlzeit in fehlzeiten)
                {
                    if (fehlzeit.MitarbeiterId == mitarbItem.Id)
                    {
                        mitarbItem.Fehlzeiten.Add(fehlzeit);
                    }
                }
            }

            KolonnenDbTable kolonnenTable = new KolonnenDbTable();
            List<Kolonne> kolonnen = kolonnenTable.ReadAll();
            kolonnenTable.CloseConnection();
            this.repositories.Kolonnen.SetCollection(kolonnen);


            KolonnenMitarbeiterDbTable kolMitTable = new KolonnenMitarbeiterDbTable();
            List<KolonnenMitarbeiterItem> kolMitarbeiter = kolMitTable.RealAll();

            if (kolMitarbeiter.Count > 0)
            {
                // Mitarbeiter den Kolonnen zuweisen
                foreach (var kolonneItem in this.repositories.Kolonnen.GetAll())
                {
                    foreach (var kolMitItem in kolMitarbeiter)
                    {
                        if (kolMitItem.KolonneId == kolonneItem.Id)
                        {
                            Mitarbeiter matchedMitarbeiter = this.repositories.Mitarbeiter.GetMitarbeiter(kolMitItem.MitarbeiterId);
                            if (matchedMitarbeiter != null)
                            {
                                if (kolMitItem.IstFuehrer)
                                    kolonneItem.Fuehrer = matchedMitarbeiter;
                                else
                                    kolonneItem.AdditionalMitarbeiter.Add(matchedMitarbeiter);
                            }
                        }
                    }
                }
            }

            EngagementsCombinedDbTable engagementsComTable = new EngagementsCombinedDbTable();
            List<EngagementEntity> engagementEntities = engagementsComTable.RealAll();
            foreach (var entity in engagementEntities)
            {
                Engagement engagement = new Engagement();
                engagement.Id = entity.Id;
                engagement.Date = entity.Date;
                engagement.Kolonne = this.repositories.Kolonnen.GetKolonne(entity.KolonneId);

                foreach (var mitarEntity in entity.MitarbeiterEntities)
                {
                    DayMitarbeiter dayMitarbeiter = new DayMitarbeiter(
                        this.repositories.Mitarbeiter.GetMitarbeiter(mitarEntity.MitarbeiterId), mitarEntity.IstFuehrer);
                    engagement.Mitarbeiter.Add(dayMitarbeiter);
                }
                foreach (var proEntity in entity.ProjektEntities)
                {
                    EngagementProjekt engProjekt = EngagementProjekt.CreateFromEntity(proEntity, this.repositories.Projekte.GetProjekt(proEntity.ProjektId));
                    engProjekt.Projekt.IsLinkedToEngagement = true;
                    engagement.Projekte.Add(engProjekt);
                }
                this.repositories.Engagements.Add(engagement);

                this.calendar.CreateOrUpdateDay(engagement.Date);
                this.calendar.ReplaceKolonneAtDate(engagement);
            }
        }

        private void UpdateView()
        {
            this.calenderPresenter.UpdateView();
            this.calenderPresenter.UpdateDayItemsAlignment();
            this.UpdateProjekteView();
        }

        private void UpdateProjekteView()
        {
            this.projectsPresenter.SetProjects(this.repositories.Projekte.GetTemplateUncompletedProjects());
            this.projectsPresenter.UpdateView();
        }

        private void NotifyProjektSelectionChanged(ProjektViewModelEventArgs e)
        {

        }

        private void UpdateShowHideMitarbeiterMenuItemText()
        {
            if (AppConfig.IsMitarbeiterListVisible)
                this.window.showHideMitarbeiterTextBlock.Text = "Mitarbeiterlisten ausblenden";
            else
                this.window.showHideMitarbeiterTextBlock.Text = "Mitarbeiterlisten anzeigen";
        }

        // EventHandler //

        /// <summary>
        /// Alle Menüpunkte deaktivieren wenn der User keine Modifiy-Rechte hat.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MenuGroupVerwaltungSubmenuOpened(object sender, RoutedEventArgs e)
        {
            bool enabled = AppConfig.UserCanModify;
            this.window.mainMenuCrew.IsEnabled = enabled;
            this.window.mainMenuProjekt.IsEnabled = enabled;
            this.window.mainMenuFunctions.IsEnabled = enabled;
            this.window.mainMenuStaff.IsEnabled = enabled;
            this.window.mainMenuUsers.IsEnabled = enabled;
        }

        void MainMenuCrewClick(object sender, System.Windows.RoutedEventArgs e)
        {
            KolonneListWindow kolonneListWindow = new KolonneListWindow();
            kolonneListWindow.Owner = this.window;

            KolonneListPresenter presenter = new KolonneListPresenter(
                kolonneListWindow, this.repositories.Kolonnen, this.repositories.Mitarbeiter, this.repositories);
            presenter.ShowDialog();
            // Alles aktualisieren
            this.UpdateView();
        }

        void MainMenuProjektClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ProjektListWindow projektListWindow = new ProjektListWindow();
            projektListWindow.Owner = this.window;

            ProjektListPresenter presenter = new ProjektListPresenter(projektListWindow, this.repositories.Projekte);
            presenter.ShowDialog();

            // Alles aktualisieren
            this.UpdateView();
        }

        void MainMenuStaffClick(object sender, System.Windows.RoutedEventArgs e)
        {
            MitarbeiterListWindow mitarbeiterListWindow = new MitarbeiterListWindow();
            mitarbeiterListWindow.Owner = this.window;
            MitarbeiterListPresenter presenter = new MitarbeiterListPresenter(mitarbeiterListWindow,
                this.repositories.Mitarbeiter, this.repositories.Funktionen, this.repositories.Fehlzeitarten);
            presenter.ShowDialog();

            // Alles aktualisieren
            this.UpdateView();
        }

        void MainMenuFunctionsClick(object sender, RoutedEventArgs e)
        {
            FunktionListWindow funktionListWindow = new FunktionListWindow();
            funktionListWindow.Owner = this.window;
            FunktionListPresenter presenter = new FunktionListPresenter(funktionListWindow, this.repositories.Funktionen, this.repositories.Mitarbeiter);
            presenter.ShowDialog();
            if (presenter.GetDialogResult())
            {
                // Alles aktualisieren
                this.UpdateView();
            }
        }

        /// <summary>
        /// Die Liste der Nutzerverwaltung anzeigen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainMenuUsersClick(object sender, RoutedEventArgs e)
        {
            UserListWindow userListWindow = new UserListWindow();
            userListWindow.Owner = this.window;
            UserListPresenter presenter = new UserListPresenter(userListWindow, this.repositories.Users);
            presenter.ShowDialog();
            if (presenter.GetDialogResult())
            {
                // Muss ich hier was tun?
                // Denke nicht!
            }
        }

        /// <summary>
        /// Das Optionen-Fenster öffnen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainMenuViewOptionsClick(object sender, System.Windows.RoutedEventArgs e)
        {
            EasyOptionsWindow optionsWin = new EasyOptionsWindow();
            optionsWin.Owner = this.window;
            bool? result = optionsWin.ShowDialog();
            if (result == true)
            {
                // Optionen wurden mit "Übernehmen" beendet, jetzt speichern
                AppConfig.Save();
                // Breiteneinstellung der Kalendertage haben sich evtl. verändert -> aktualisieren
                this.calenderPresenter.UpdateDayItemsAlignment();

                NotificationCenter.GetInstance().Notify((int)Notifications.RefreshCompleteCalendar);
            }
        }

        void showHideMitarbeiterMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AppConfig.IsMitarbeiterListVisible = !AppConfig.IsMitarbeiterListVisible;
            this.UpdateShowHideMitarbeiterMenuItemText();
            this.calenderPresenter.SetMitarbeiterListsVisibility(AppConfig.IsMitarbeiterListVisible);
        }

        /// <summary>
        /// Drucken : Berichte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainMenuPrintReportClick(object sender, RoutedEventArgs e)
        {
            this.mainPrinter.CreateTimespanReport();
        }

        void window_Closed(object sender, EventArgs e)
        {
            AppConfig.Save();
        }



    }
}
