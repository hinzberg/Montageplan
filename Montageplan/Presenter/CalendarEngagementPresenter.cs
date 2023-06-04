using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Montageplan.Model;
using Montageplan.View;
using Montageplan.ViewModel;
using Montageplan.Model.DAL.Database;
using System.Linq;
using Montageplan.View.Windows;
using Montageplan.Windows;

namespace Montageplan.Presenter
{
    public class CalendarEngagementPresenter : NotificationModel
    {
        private readonly CalendarDay day;
        private readonly Engagement engagement;
        private readonly CalendarEngagementView view;
        private readonly EngagementRepository engagementRepository;
        private readonly MontageCalendar calendar;
        private bool cancelExpandedEvent;

        public CalendarEngagementPresenter(
            CalendarEngagementView view, Engagement engagement, CalendarDay day, EngagementRepository engagementRepository, MontageCalendar calendar)
        {
            this.view = view;
            this.engagement = engagement;
            this.day = day;
            this.engagementRepository = engagementRepository;
            this.calendar = calendar;
            this.IsFirstDisplayedDay = false;
            this.KolonneExpandedOrCollapsed = null;
            this.MitarbeiterInChooseListStatusUpdateAndRefreshing = null;
            this.cancelExpandedEvent = false;

            // Entfernen
            this.view.removeMitarbeiterMenuItem.Click += RemoveMitarbeiterMenuItemClick;
            // Als Kolonnenführer setzten
            this.view.setKolonnenFuehrerMenuItem.Click += SetKolonnenFuehrerMenuItemClick;
            // Projekt hinzufügen
            this.view.addProjektControlMenuItem.Click += addProjektControlMenuItem_Click;
            this.view.addProjektMitarbeiterMenuItem.Click += addProjektControlMenuItem_Click;
            this.view.addProjektProjekteMenuItem.Click += addProjektControlMenuItem_Click;
            // Projekt(e) entfernen
            this.view.removeProjektMenuItem.Click += removeProjektMenuItem_Click;
            this.view.removeAllProjekteMenuItem.Click += removeAllProjekteMenuItem_Click;
            this.view.removeAllProjekteOnMitarbeiterMenuItem.Click += removeAllProjekteMenuItem_Click;
            this.view.removeEngagementControlMenuItem.Click += removeAllProjekteMenuItem_Click;
            // Kontextmenü öffnet
            this.view.ContextMenuOpening += view_ContextMenuOpening;
            this.view.mitarbeiterListBox.ContextMenuOpening += MitarbeiterListBoxContextMenuOpening;
            //this.view.noProjektGrid.MouseDown += CreateProject_MouseDown;
            //this.view.projekteListBox.MouseDown += CreateProject_MouseDown;
            //this.view.mitarbeiterListBox.MouseDown += CreateProject_MouseDown;
            this.view.MouseDown += CreateProject_MouseDown;

            this.UpdateKolonneExpandedState();
            this.view.headerExpander.Expanded += headerExpander_ExpandesOrCollapsed;
            this.view.headerExpander.Collapsed += headerExpander_ExpandesOrCollapsed;
        }

        /// <summary>
        /// Ist der Tag der erste, der im Kalender ganz links angezeigt wird ?
        /// </summary>
        public bool IsFirstDisplayedDay { get; set; }

        public Action KolonneExpandedOrCollapsed { get; set; }
        /// <summary>
        /// Beansprucht, dass der Status vom Mitarbeiter in der Auswahlliste aktualisiert wird und
        /// komplett neu gebunden wird, damit die Änderungen sichtbar werden (per MitarbeiterId).
        /// </summary>
        public Action<int> MitarbeiterInChooseListStatusUpdateAndRefreshing { get; set; }

        public Engagement GetDayKolonne()
        {
            return this.engagement;
        }

        public void Init()
        {
            ListBoxDropHelper dropHelper = new ListBoxDropHelper(this.view, this.engagement);
            dropHelper.DropCompleted = new ListBoxDropHelper.DropCompletedHandler(this.NotifyObjectDropped);
            ListBoxDropHelper.AddHelper(dropHelper);
        }

        public void UpdateView()
        {
            // Ob vor den Kolonnenbezeichnung jetzt "Kolonne" 
            // steht kann in den Optionen eingestellt werden.
            string prefix = "";
            if (AppConfig.Settings.ShowKolonnePrefix)
                prefix = "Kolonne: ";

            this.view.kolonneNummerTextBlock.Text = string.Format("{0}{1}", prefix, this.engagement.Kolonne.Nummer);
            this.view.kolonneBezeichnungTextBlock.Text = this.engagement.Kolonne.Bezeichnung;
            this.view.projekteCountTextBlock.Text = this.engagement.Projekte.GetCount().ToString();

            string toolText = "Ein Projekt ist der Kolonne an diesem Tag zugeordnet";
            if (this.engagement.Projekte.GetCount() > 1)
                toolText = string.Format("{0} Projekte sind der Kolonne an diesem Tag zugeordnet", this.engagement.Projekte.GetCount());
            this.view.projekteCountTextBlock.ToolTip = toolText;

            this.UpdateMitarbeiter();
            this.UpdateProjekte();
        }

        public void UpdateKolonneExpandedState()
        {
            this.cancelExpandedEvent = true;
            this.view.headerExpander.IsExpanded = this.engagement.Kolonne.IsExpanded;
            this.cancelExpandedEvent = false;

            if (this.engagement.Kolonne.IsExpanded)
                this.SetDefaultHeight();
            else
                this.SetCollapsedHeight();
        }

        private void SetDefaultHeight()
        {
            this.view.Height = AppConfig.KOLONNE_DEFAULT_HEIGHT;
            ScrollViewer.SetVerticalScrollBarVisibility(this.view.mitarbeiterListBox, ScrollBarVisibility.Auto);
        }

        private void SetCollapsedHeight()
        {
            this.view.Height = AppConfig.KOLONNE_COLLAPSED_HEIGHT;
            ScrollViewer.SetVerticalScrollBarVisibility(this.view.mitarbeiterListBox, ScrollBarVisibility.Disabled);
        }

        private void UpdateMitarbeiter()
        {
            this.view.mitarbeiterListBox.ItemsSource =
                DayMitarbeiterViewModel.CreateViewModels(this.engagement.Mitarbeiter.GetSortedByFuehrer(), this.day.Date);
        }

        private void UpdateProjekte()
        {
            this.SetControlBrushes();

            this.view.noProjektGrid.Visibility = (this.engagement.HasProjekte) ? Visibility.Collapsed : Visibility.Visible;
            this.view.projekteListBox.ItemsSource = EngagementProjektViewModel.CreateViewModels(this.engagement.Projekte.GetAllTimeSorted());
        }

        private void SetControlBrushes()
        {
            this.view.kolonneNummerTextBlock.FontWeight = FontWeights.Bold;
            this.view.kolonneBezeichnungTextBlock.FontWeight = FontWeights.Bold;
            this.view.projekteCountTextBlock.FontWeight = FontWeights.Bold;

            // Steuerelemente grau anzeigen, wenn zu der Kolonne kein Projekt zugewiesen wurde
            if (this.engagement != null && this.engagement.HasProjekte)
            {
                this.view.Foreground = Brushes.Black;
                this.view.Background = Brushes.Transparent;
                this.view.emptyInfoBorder.Background = null;
            }
            else
            {
                SolidColorBrush foregroundBrush = new SolidColorBrush(Colors.DimGray);
                SolidColorBrush backgroundBrush = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
                SolidColorBrush headerBackgroundBrush = new SolidColorBrush(Color.FromArgb(100, 160, 160, 160)); // Schleier über dem Header

                this.view.Foreground = foregroundBrush;
                this.view.Background = backgroundBrush;
                this.view.emptyInfoBorder.Background = headerBackgroundBrush;
            }

            if (this.engagement != null)
                this.view.kolonneColorHeaderGrid.Background = this.engagement.Kolonne.KolonneColorBrush;

            base.NotifyPropertyChanged("Foreground");
        }

        private void NotifyObjectDropped(object droppedData)
        {
            if (droppedData is ProjektViewModel)
            {
                ProjektViewModel droppedProjectViewModel = (ProjektViewModel)droppedData;
                Projekt droppedProject = droppedProjectViewModel.GetModel();

                TimePickerWindow timePickerWin = new TimePickerWindow();
                timePickerWin.SetTimes(droppedProject.Startdatum, droppedProject.Endedatum);
                bool? isOk = timePickerWin.ShowDialog();
                if (isOk.Value)
                {
                    // Zunächst prüfen, ob der Tag zum Zeitplan des Projektes passt.
                    bool canDrop = this.CanDropProjectOnDate(droppedProject, timePickerWin);
                    if (canDrop)
                    {
                        EngagementProjekt engProjekt = new EngagementProjekt();
                        engProjekt.Projekt = droppedProject;

                        DateTime? startTime = timePickerWin.GetStartTime();
                        DateTime? endTime = timePickerWin.GetEndTime();
                        if (startTime.HasValue)
                            engProjekt.StartTime = new DateTime(
                                this.engagement.Date.Year, this.engagement.Date.Month, this.engagement.Date.Day,
                                startTime.Value.Hour, startTime.Value.Minute, 0);
                        if (endTime.HasValue)
                            engProjekt.EndTime = new DateTime(
                                this.engagement.Date.Year, this.engagement.Date.Month, this.engagement.Date.Day,
                                endTime.Value.Hour, endTime.Value.Minute, 0);

                        engProjekt.Projekt.IsLinkedToEngagement = this.engagementRepository.IsProjectLinkedToEngagement(engProjekt.Projekt.Id);
                        NotificationCenter.GetInstance().Notify((int)Notifications.RefreshDraggableProjectVM, engProjekt.Projekt);
                        if (!engProjekt.Projekt.IsLinkedToEngagement)
                        {
                            engProjekt.Projekt.IsLinkedToEngagement = true;
                            droppedProjectViewModel.NotifyStatus();
                        }

                        this.AddProjektToEngagement(engProjekt);

                        if (engProjekt.Projekt.Endedatum.HasValue)
                        {
                            this.AddProjektTillEnddate(engProjekt);
                        }
                    }
                }
            }
            else if (droppedData is DayMitarbeiterViewModel)
            {
                DayMitarbeiterViewModel droppedMitarbeiterDayLink = (DayMitarbeiterViewModel)droppedData;
                DayMitarbeiter dayMitarbeiter = droppedMitarbeiterDayLink.GetModel();
                if (!this.engagement.ContainsMitarbeiter(dayMitarbeiter))
                {
                    this.engagement.Mitarbeiter.Add(dayMitarbeiter);
                    this.day.UpdateMitarbeiterStatus(droppedMitarbeiterDayLink.GetModel());
                    droppedMitarbeiterDayLink.NotifyStatusColor();
                    this.UpdateMitarbeiter();

                    EngagementsMitarbeiterDbTable table = new EngagementsMitarbeiterDbTable();
                    table.Insert(new EngagementMitarbeiterEntity(this.engagement.Id, dayMitarbeiter.Mitarbeiter.Id, false));

                    ////if (this.engagement.HasProjekte)
                    ////    this.AddMitarbeiterOnKolonneToEnddate(dayMitarbeiter, this.engagement.Projekt.Endedatum.Value); // TODO
                }
            }
        }

        private void AddProjektToEngagement(EngagementProjekt engProjekt)
        {
            this.engagement.Projekte.Add(engProjekt);

            this.UpdateProjekte();

            // Auftrag ist komplett - Tag, Kolonne mit Kolonnenführer und Projekt bilden den Auftrag
            // In Datenbank speichern
            EngagementsCombinedDbTable table = new EngagementsCombinedDbTable();
            if (this.engagement.Id > 0)
            {
                EngagementsProjekteDbTable engProTable = new EngagementsProjekteDbTable();
                int pk = engProTable.Insert(EngagementProjektEntity.Create(this.engagement.Id, engProjekt));
                engProjekt.Id = pk;
            }
            else
            {
                this.engagementRepository.Add(this.engagement);
                EngagementEntity tempEntity = EngagementEntity.Create(this.engagement);
                table.Insert(tempEntity);
                this.engagement.Id = tempEntity.Id;
            }
        }

        private void AddProjektTillEnddate(EngagementProjekt engProjekt)
        {
            DateTime nextDate = this.day.Date.AddDays(1);
            DateTime endDate = engProjekt.Projekt.Endedatum.Value;
            int days = (int)endDate.Subtract(nextDate).TotalDays + 1;
            for (int d = 0; d < days; d++)
            {
                DateTime date = nextDate.AddDays(d);
                CalendarDay calendarDay = this.calendar.CreateOrUpdateDay(date);
                foreach (var engagementItem in calendarDay.Engagements.GetAll())
                {
                    if (engagementItem.Kolonne.Id == this.engagement.Kolonne.Id)
                    {
                        if (engagementItem.IsAvailableProjektTime(engProjekt.StartTime, engProjekt.EndTime))
                        {
                            // Engagement speichern (Repository und db) - Falls noch nicht in der db vorhandne
                            if (engagementItem.Id == 0)
                            {
                                EngagementsCombinedDbTable table = new EngagementsCombinedDbTable();
                                EngagementEntity tempEntity = EngagementEntity.Create(engagementItem);
                                table.Insert(tempEntity);
                                engagementItem.Id = tempEntity.Id;
                            }

                            // Engagement-Projekt speichern (Repository und db)
                            EngagementProjekt newEngProjekt = EngagementProjekt.CreateFromEngagementProjekt(date, engProjekt);
                            engagementItem.Projekte.Add(newEngProjekt);
                            EngagementsProjekteDbTable engProTable = new EngagementsProjekteDbTable();
                            engProTable.Insert(EngagementProjektEntity.Create(engagementItem.Id, newEngProjekt));
                        }
                    }
                }
            }
            NotificationCenter.GetInstance().Notify((int)Notifications.RefreshCompleteCalendar);
        }

        //private void AddMitarbeiterOnKolonneToEnddate(DayMitarbeiter dayMitarbeiter, DateTime endDate)
        //{
        //    DateTime nextDate = this.day.Date.AddDays(1);
        //    int days = (int)endDate.Subtract(nextDate).TotalDays + 1;
        //    EngagementsMitarbeiterDbTable table = new EngagementsMitarbeiterDbTable();

        //    for (int d = 0; d < days; d++)
        //    {
        //        DateTime date = nextDate.AddDays(d);
        //        CalendarDay calendarDay = this.calendar.CreateOrUpdateDay(date);
        //        foreach (var engItem in calendarDay.Engagements.GetAll())
        //        {
        //            if (engItem.Kolonne.Id == this.engagement.Kolonne.Id)
        //            {
        //                if (engItem.HasProjekte)
        //                {
        //                    if (!engItem.ContainsMitarbeiter(dayMitarbeiter))
        //                    {
        //                        engItem.Mitarbeiter.Add(dayMitarbeiter);
        //                        table.Insert(new EngagementMitarbeiterEntity(engItem.Id, dayMitarbeiter.Mitarbeiter.Id, false));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    NotificationCenter.GetInstance().Notify((int)Notifications.RefreshCompleteCalendar);
        //}

        private bool CanDropProjectOnDate(Projekt projekt, TimePickerWindow timePickerWin)
        {
            bool canDrop = true;

            DateTime? startTime = timePickerWin.GetStartTime();
            DateTime? endTime = timePickerWin.GetEndTime();

            if (projekt.Endedatum.HasValue && this.day.Date.Date > projekt.Endedatum.Value.Date)
            {
                // Nach einem Endedatum abgelegt.
                const string message =
                    "Das ausgewählte Datum ist nach dem Endedatum des Projektes. Das Projekt trotzdem ablegen?";
                bool result = MessageBoxHelper.ShowYesNoQuestionWindow("", message, null);
                if (result != true)
                    canDrop = false;
            }
            else if (projekt.Startdatum.HasValue && this.day.Date.Date < projekt.Startdatum.Value.Date)
            {
                // Vor dem Startdaum abgelegt.
                const string message =
                    "Das ausgewählte Datum ist noch vor dem Startdatum des Projektes. Das Projekt trotzdem ablegen?";

                bool result = MessageBoxHelper.ShowYesNoQuestionWindow("", message, null);
                if (result != true)
                    canDrop = false;
            }


            if ((startTime.HasValue && !endTime.HasValue) || (!startTime.HasValue && endTime.HasValue))
            {
                const string message = "Ein Projekt ist nur gültig, wenn entweder keine Zeit angegeben wird oder eine Startzeit mit einer Endzeit.";
                MessageBoxHelper.ShowOkBoxWindow("", message, null);
                canDrop = false;
            }
            else if ((startTime.HasValue && endTime.HasValue) && (startTime.Value > endTime.Value))
            {
                const string message = "Die Startzeit muss vor der Endzeit liegen.";
                MessageBoxHelper.ShowOkBoxWindow("", message, null);
                canDrop = false;
            }
            else if (!this.engagement.IsAvailableProjektTime(startTime, endTime))
            {
                const string message = "Der Zeitraum kann nicht verwendet werden. Es würde zu Projektüberschneidungen kommen.";
                MessageBoxHelper.ShowOkBoxWindow("", message, null);
                canDrop = false;
            }
            return canDrop;
        }


        private List<DayMitarbeiterViewModel> GetMitarbeiterSelection()
        {
            List<DayMitarbeiterViewModel> selection = new List<DayMitarbeiterViewModel>();
            foreach (var item in this.view.mitarbeiterListBox.SelectedItems)
                selection.Add((DayMitarbeiterViewModel)item);
            return selection;
        }

        private void SetRemoveEngagementMenuItemEnable()
        {
            // Termin entfernen, wenn Projekt vorhanden.
            if (this.engagement.HasProjekte)
            {
                this.view.removeEngagementControlMenuItem.IsEnabled = true;
            }
            else
            {
                this.view.removeEngagementControlMenuItem.IsEnabled = false;
            }
        }

        private List<EngagementProjekt> GetProjekteSelection()
        {
            List<EngagementProjekt> engProjekte = new List<EngagementProjekt>();
            foreach (var selected in this.view.projekteListBox.SelectedItems)
            {
                engProjekte.Add((selected as EngagementProjektViewModel).GetModel());
            }
            return engProjekte;
        }

        private void DeleteProjekte(IList<EngagementProjekt> engProjekte)
        {
            if (engProjekte.Count > 0)
            {
                foreach (var engProjekt in engProjekte)
                {
                    this.engagement.Projekte.Remove(engProjekt);
                    EngagementsProjekteDbTable table = new EngagementsProjekteDbTable();
                    table.Delete(engProjekt.Id);
                }

                // Überprüfen, ob das projekt noch bei einem anderen Termin zugewiesen ist bzw. in diesem Termin zur anderen Zeit
                foreach (var engProItem in this.engagement.Projekte.GetAll())
                {
                    bool isLinkedBefore = engProItem.Projekt.IsLinkedToEngagement;
                    engProItem.Projekt.IsLinkedToEngagement = this.engagementRepository.IsProjectLinkedToEngagement(engProItem.Projekt.Id);
                    if (isLinkedBefore != engProItem.Projekt.IsLinkedToEngagement)
                        NotificationCenter.GetInstance().Notify((int)Notifications.RefreshDraggableProjectVM, engProItem.Projekt);
                }

                this.UpdateProjekte();
            }
        }

        private void DeleteEngagement()
        {
            EngagementsCombinedDbTable table = new EngagementsCombinedDbTable();
            table.Delete(EngagementEntity.Create(this.engagement));

            this.engagement.Id = 0;
            this.engagement.Mitarbeiter.ClearAll();
            // Default-Kolonnenführer und Mitarbeiter wieder setzen
            this.engagement.RefreshMitarbeiter();
            this.engagementRepository.Remove(this.engagement);

            this.UpdateView();
        }

        // EventHandler

        private void headerExpander_ExpandesOrCollapsed(object sender, RoutedEventArgs e)
        {
            if (!this.cancelExpandedEvent)
            {
                this.engagement.Kolonne.IsExpanded = this.view.headerExpander.IsExpanded;
                this.UpdateKolonneExpandedState();

                if (this.KolonneExpandedOrCollapsed != null)
                    this.KolonneExpandedOrCollapsed();
            }
        }

        void view_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            this.SetRemoveEngagementMenuItemEnable();
        }

        private void MitarbeiterListBoxContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // Keine Rechte zur Manipulation
            if (!AppConfig.UserCanModify)
            {
                this.view.removeEngagementControlMenuItem.IsEnabled = false;
                this.view.setKolonnenFuehrerMenuItem.IsEnabled = false;
                this.view.removeMitarbeiterMenuItem.IsEnabled = false;
                return;
            }

            this.SetRemoveEngagementMenuItemEnable();

            if (this.view.mitarbeiterListBox.SelectedItems.Count == 0)
            {
                this.view.setKolonnenFuehrerMenuItem.IsEnabled = false;
                this.view.removeMitarbeiterMenuItem.IsEnabled = false;
            }
            else
            {

                // Wenn nur 1 Mitarbeiter ausgewählt ist und dieser ist der Kolonnenführer, dann darf das
                // Entfernen-MenuItem nicht verfügbar sein
                List<DayMitarbeiterViewModel> selection = this.GetMitarbeiterSelection();
                this.view.removeMitarbeiterMenuItem.IsEnabled =
                    !(selection.Count == 1 && selection[0].GetModel().IstFuehrer);

                // Kann der gewählte Mitarbeiter der Kolonnenführer werden?
                if (selection.Count == 1 && selection[0].GetModel().Mitarbeiter.KannFuehrerSein)
                {
                    // Bin ich vielleicht schon Kolonnenführer?
                    // Dann kann ich es nicht mehr werden.
                    DayMitarbeiterRepository rep = this.engagement.Mitarbeiter;
                    Mitarbeiter mitarbeiter = selection[0].GetModel().Mitarbeiter;
                    DayMitarbeiter dayMitarbeiter = rep.GetDayMitarbeiterFromMitarbeiter(mitarbeiter);
                    if (dayMitarbeiter.IstFuehrer)
                    {
                        this.view.setKolonnenFuehrerMenuItem.IsEnabled = false;
                    }
                    else
                    {
                        this.view.setKolonnenFuehrerMenuItem.IsEnabled = true;
                    }
                }
                else
                {
                    this.view.setKolonnenFuehrerMenuItem.IsEnabled = false;
                }
            }

            if (!this.engagement.HasProjekte)
                this.view.removeMitarbeiterMenuItem.IsEnabled = false;
        }

        private void RemoveMitarbeiterMenuItemClick(object sender, RoutedEventArgs e)
        {
            List<DayMitarbeiterViewModel> selection = this.GetMitarbeiterSelection();
            if (selection.Count > 0)
            {
                foreach (var selected in selection)
                {
                    if (!selected.GetModel().IstFuehrer) // Kolonnenführer nicht entfernen
                    {
                        DayMitarbeiter dayMitarbeiter = selected.GetModel();
                        this.engagement.Mitarbeiter.Remove(dayMitarbeiter);
                        this.day.UpdateMitarbeiterStatus(dayMitarbeiter);
                        if (this.MitarbeiterInChooseListStatusUpdateAndRefreshing != null)
                            this.MitarbeiterInChooseListStatusUpdateAndRefreshing(dayMitarbeiter.Mitarbeiter.Id);

                        EngagementsCombinedDbTable table = new EngagementsCombinedDbTable();
                        table.DeleteMitarbeiterLink(this.engagement.Id, dayMitarbeiter.Mitarbeiter.Id);
                    }
                }
                this.UpdateMitarbeiter();
            }
        }

        /// <summary>
        /// Neuen Kolonnenführer setzten, über Kontextmenü
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetKolonnenFuehrerMenuItemClick(object sender, RoutedEventArgs e)
        {
            List<DayMitarbeiterViewModel> selection = this.GetMitarbeiterSelection();

            if (selection.Count == 1 &&
                selection[0].GetModel().Mitarbeiter.KannFuehrerSein)
            {
                DayMitarbeiter fuehrerBefore = null;
                DayMitarbeiterRepository rep = this.engagement.Mitarbeiter;
                foreach (DayMitarbeiter arbeiter in rep.GetAll())
                {
                    if (arbeiter.IstFuehrer)
                    {
                        fuehrerBefore = arbeiter;
                        arbeiter.IstFuehrer = false;
                        break;
                    }
                }

                Mitarbeiter mitarbeiter = selection[0].GetModel().Mitarbeiter;
                DayMitarbeiter dayMitarbeiter = rep.GetDayMitarbeiterFromMitarbeiter(mitarbeiter);
                // Er ist der neue Führer.
                dayMitarbeiter.IstFuehrer = true;

                this.engagement.Mitarbeiter.RemoveFuehrer();
                this.engagement.Mitarbeiter.Add(dayMitarbeiter);

                EngagementsCombinedDbTable table = new EngagementsCombinedDbTable();
                if (fuehrerBefore != null)
                    table.UpdateMitarbeiterLink(this.engagement.Id, fuehrerBefore.Mitarbeiter.Id, false);

                table.UpdateMitarbeiterLink(this.engagement.Id, dayMitarbeiter.Mitarbeiter.Id, true);

                this.UpdateView();

            }
        }

        void removeProjektMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteProjekte(this.GetProjekteSelection());
            if (this.engagement.Projekte.GetCount() == 0)
                this.DeleteEngagement();
        }

        void removeAllProjekteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteProjekte(this.engagement.Projekte.GetAll());
            this.DeleteEngagement();
        }

        /// <summary>
        /// Projekt hinzufügen ohne Vorlage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void addProjektControlMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.ShowNewProjektWindow();
        }

        /// <summary>
        /// Projekt hinzufügen ohne Vorlage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CreateProject_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                this.ShowNewProjektWindow();
            }
        }

        private void ShowNewProjektWindow()
        {
            ProjektEditPresenter newProjPresenter = new ProjektEditPresenter(new ProjektEditWindow());
            // Ich muss hier das Datum übergeben, da ich keine Projekte
            // mit Zeiten ohne Datum anlegen kann.
            newProjPresenter.CreateNewProjektForDate(this.day.Date);
            newProjPresenter.ShowDialog();

            if (newProjPresenter.GetDialogResult())
            {
                // Projekt
                Projekt newProjekt = newProjPresenter.GetProjekt();
                ProjekteDbTable projekteTable = new ProjekteDbTable();
                projekteTable.Insert(newProjekt);

                // Termin-Projekt (EngagementProjekt)
                EngagementProjekt engProjekt = new EngagementProjekt();
                engProjekt.Projekt = newProjekt;
                engProjekt.StartTime = newProjekt.GetStartTime();
                engProjekt.EndTime = newProjekt.GetEndTime();
                this.AddProjektToEngagement(engProjekt);
            }
        }

    }
}