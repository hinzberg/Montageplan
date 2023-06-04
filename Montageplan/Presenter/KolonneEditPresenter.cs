using System.Windows;
using System.Windows.Media;
using Montageplan.Model;
using Montageplan.View.Windows;
using Montageplan.ViewModel;
using Montageplan.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Montageplan.Model.DAL.Database;
using System.Collections.ObjectModel;

namespace Montageplan.Presenter
{
    public class KolonneEditPresenter
    {
        private readonly KolonneEditWindow window;
        private readonly KolonneRepository kolonneRepository;
        private readonly RepositoryContainer repositories;
        private readonly ControlInputValidator inputValidator;
        private readonly List<MitarbeiterViewModel> mitarbeiterFuehrerViewModels;
        private readonly List<MitarbeiterViewModel> mitarbeiterViewModels;
        private Kolonne kolonne;
        /// <summary>
        /// Alle potenziellen Mitarbeiter des Auswahlfensters
        /// </summary>
        private List<MitarbeiterChooseViewModel> addMitarbeiterViewModelsFromChooseWin;
        /// <summary>
        /// Zusätzliche Mitarbeiter auf dem Fenster
        /// </summary>
        private ObservableCollection<MitarbeiterViewModel> addMitarbeiterViewModels;

        public KolonneEditPresenter(KolonneEditWindow window,
            KolonneRepository kolonneRepository, MitarbeiterRepository mitarbeiter, RepositoryContainer repositories)
        {
            this.window = window;
            this.kolonneRepository = kolonneRepository;
            this.repositories = repositories;
            this.kolonne = null;
            this.addMitarbeiterViewModelsFromChooseWin = new List<MitarbeiterChooseViewModel>();
            this.addMitarbeiterViewModels = new ObservableCollection<MitarbeiterViewModel>();
            this.inputValidator = new ControlInputValidator();
            this.InitTextBoxValidator();

            this.mitarbeiterFuehrerViewModels =
                new List<MitarbeiterViewModel>(MitarbeiterViewModel.CreateViewModels(mitarbeiter.GetPotentialKolonnenfuehrer()));
            this.mitarbeiterViewModels =
                new List<MitarbeiterViewModel>(MitarbeiterViewModel.CreateViewModels(mitarbeiter.GetAll()));
            this.window.comboKolonnenFuehrer.ItemsSource = mitarbeiterFuehrerViewModels;
            //  this.window.comboFirstMitarbeiter.ItemsSource = mitarbeiterViewModels;

            this.window.mitarbeiterListView.ItemsSource = this.addMitarbeiterViewModels;
            this.window.btnSubmit.Click += btnSubmit_Click;
            this.window.btnCancel.Click += btnCancel_Click;

            this.window.mainToolbar.newButton.Visibility = Visibility.Collapsed;
            this.window.mainToolbar.newSeperator.Visibility = Visibility.Collapsed;
            this.window.mainToolbar.deleteButton.Visibility = Visibility.Collapsed;
            this.window.mainToolbar.editSeperactor.Visibility = Visibility.Collapsed;
            this.window.mainToolbar.SetEnabledEditButton(true);

            this.window.mainToolbar.editButton.Click += editButton_Click;
            this.window.mitarbeiterListView.SelectionChanged += MitarbeiterListViewSelectionChanged;
            this.window.colorDisplay.MouseDown += ShowColorPickerWindow;
        }

        /// <summary>
        /// ColorPicker für Kolonnefarbe anzeigen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ShowColorPickerWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ColorPickerWindow colorPickerWindow = new ColorPickerWindow(this.kolonne.KolonneColorBrush.Color);
            colorPickerWindow.Owner = this.window;
            bool? res = colorPickerWindow.ShowDialog();
            if (res != null && res == true)
            {
                Color col = colorPickerWindow.SelectedColor;
                this.window.colorDisplay.Background = new SolidColorBrush(col);
            }
        }


        public void ShowDialog()
        {
            this.window.ShowDialog();
        }

        public bool GetDialogResult()
        {
            return this.window.DialogResult.Value;
        }

        public void CreateNewKolonne()
        {
            this.kolonne = new Kolonne();
            // Default ist der erste Mitarbeiter der Liste der Kolonnenführer
            // Default ist der zweite Mitarbeiter der Liste der Mitarbeiter
            if (this.mitarbeiterFuehrerViewModels.Count > 0)
                this.kolonne.Fuehrer = this.mitarbeiterFuehrerViewModels[0].GetModel();

            this.window.Title = "Neue Kolonne";
            this.UpdateView();
        }

        public void EditKolonne(Kolonne kolonne)
        {
            this.kolonne = kolonne;

            this.window.Title = "Kolonne bearbeiten";
            this.UpdateView();
        }

        public Kolonne GetKolonne()
        {
            return this.kolonne;
        }

        private void InitTextBoxValidator()
        {
            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.NotEmptyInputValidator(), this.window.txtNummer, "Bitte geben Sie eine Nummer an!"));

            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.NotEmptyInputValidator(), this.window.txtBezeichnung, "Bitte geben Sie eine Bezeichnung an!"));

            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.NotEmptyInputValidator(), this.window.comboKolonnenFuehrer, "Bitte wählen Sie einen Kolonnenführer aus!"));
        }

        private void UpdateView()
        {
            if (this.kolonne != null)
            {
                this.window.txtNummer.Text = this.kolonne.Nummer;
                this.window.colorDisplay.Background = this.kolonne.KolonneColorBrush;
                this.window.txtBezeichnung.Text = this.kolonne.Bezeichnung;
                this.window.comboKolonnenFuehrer.SelectedItem = this.GetMitarbeiterViewModel(this.kolonne.Fuehrer, this.mitarbeiterFuehrerViewModels);
                this.window.checkAktiv.IsChecked = true;

                this.addMitarbeiterViewModels.Clear();
                foreach (var addMitar in this.kolonne.AdditionalMitarbeiter.GetAll())
                    this.addMitarbeiterViewModels.Add(new MitarbeiterViewModel(addMitar));
            }
        }

        private MitarbeiterViewModel GetMitarbeiterViewModel(Mitarbeiter mitarbeiter, IEnumerable<MitarbeiterViewModel> viewModels)
        {
            MitarbeiterViewModel viewModel = null;
            foreach (var item in viewModels)
            {
                if (item.GetModel() == mitarbeiter)
                {
                    viewModel = item;
                    break;
                }
            }
            return viewModel;
        }

        private bool IsInputValid()
        {
            bool isValid = this.inputValidator.Validate();
            if (isValid)
            {
                if (this.kolonne.Id == 0) // Nur bei einer neuen Kolonne
                {
                    isValid = !this.kolonneRepository.ContainsNummer(this.window.txtNummer.Text.Trim());
                    if (!isValid)
                        MessageBoxHelper.ShowInformation("Eine Kolonne mit dieser Nummer gibt es bereits.");
                }
            }
            return isValid;
        }

        private void UpdateKolonne()
        {
            if (this.kolonne != null)
            {
                Mitarbeiter fuehrerBefore = this.kolonne.Fuehrer;
                this.kolonne.Nummer = (string)this.inputValidator.GetValue(this.window.txtNummer);
                this.kolonne.Bezeichnung = (string)this.inputValidator.GetValue(this.window.txtBezeichnung);
                this.kolonne.Fuehrer = ((MitarbeiterViewModel)this.window.comboKolonnenFuehrer.SelectedItem).GetModel();
                this.kolonne.KolonneColorBrush = (SolidColorBrush) this.window.colorDisplay.Background;

                KolonnenDbTable kolonnenTable = new KolonnenDbTable();
                KolonnenMitarbeiterDbTable kolMitTable = new KolonnenMitarbeiterDbTable();
                if (this.kolonne.Id == 0)
                {
                    // Neue Kolonne
                    this.kolonneRepository.Add(this.kolonne);
                    kolonnenTable.Insert(this.kolonne);
                    kolMitTable.Insert(new KolonnenMitarbeiterItem(this.kolonne.Fuehrer.Id, this.kolonne.Id, true));
                    if(this.addMitarbeiterViewModelsFromChooseWin.Count > 0)
                        this.UpdateKolonneAdditionalMitarbeiter(this.addMitarbeiterViewModelsFromChooseWin);
                }
                else
                {
                    // Kolonne gab es schon
                    kolonnenTable.Update(this.kolonne);
                    if (fuehrerBefore != this.kolonne.Fuehrer)
                    {
                        kolMitTable.Delete(fuehrerBefore.Id, this.kolonne.Id);
                        kolMitTable.Insert(new KolonnenMitarbeiterItem(this.kolonne.Fuehrer.Id, this.kolonne.Id, true));
                    }
                    if (this.addMitarbeiterViewModelsFromChooseWin.Count > 0)
                        this.UpdateKolonneAdditionalMitarbeiter(this.addMitarbeiterViewModelsFromChooseWin);
                }
            }
        }

        private void UpdateKolonneAdditionalMitarbeiter(IEnumerable<MitarbeiterChooseViewModel> viewModels)
        {
            // Überprüfen, ob Mitarbeiter aus der Kolonne entfernt wurden und/oder hinzugefügt
            KolonnenMitarbeiterDbTable table = new KolonnenMitarbeiterDbTable();
            foreach (var vmItem in viewModels)
            {
                if (vmItem.HasChanges())
                {
                    Mitarbeiter mitarbeiter = vmItem.GetModel();
                    if (vmItem.IsChecked)
                    {
                        // Neu
                        table.Insert(new KolonnenMitarbeiterItem(mitarbeiter.Id, this.kolonne.Id, false));
                        this.kolonne.AdditionalMitarbeiter.Add(mitarbeiter);
                    }
                    else
                    {
                        // Entfernt
                        table.Delete(mitarbeiter.Id, this.kolonne.Id);
                        this.kolonne.AdditionalMitarbeiter.Remove(mitarbeiter);
                    }
                }
            }
        }

        // Zusätzliche Mitarbeiter verwalten

        void MitarbeiterListViewSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ////this.ValidateToolbarButtons();
        }

        ////private void ValidateToolbarButtons()
        ////{
        ////    if (this.window.mitarbeiterListView.SelectedItems.Count > 0)
        ////    {
        ////        this.window.mainToolbar.SetEnabledDeleteButton(true);
        ////        this.window.mainToolbar.SetEnabledEditButton(true);
        ////    }
        ////    else
        ////    {
        ////        this.window.mainToolbar.SetEnabledDeleteButton(false);
        ////        this.window.mainToolbar.SetEnabledEditButton(false);
        ////    }
        ////}

        // EventHandler

        void btnSubmit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bool isValid = this.IsInputValid();
            if (isValid)
            {
                this.UpdateKolonne();
                this.window.DialogResult = true;
            }
        }

        void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.window.DialogResult = false;
        }

        void editButton_Click(object sender, RoutedEventArgs e)
        {
            MitarbeiterChooseWindow chooseWindow = new MitarbeiterChooseWindow();
            chooseWindow.Owner = this.window;
            MitarbeiterChoosePresenter choosePresenter = new MitarbeiterChoosePresenter(
                chooseWindow, this.kolonne.Id, this.kolonne.AdditionalMitarbeiter, this.repositories);
            choosePresenter.ShowDialog();
            if (choosePresenter.GetDialogResult())
            {
                this.addMitarbeiterViewModelsFromChooseWin = new List<MitarbeiterChooseViewModel>(choosePresenter.GetViewModels());

                this.addMitarbeiterViewModels.Clear();
                foreach (var mitarVm in this.addMitarbeiterViewModelsFromChooseWin)
                {
                    if (mitarVm.IsChecked)
                        this.addMitarbeiterViewModels.Add(new MitarbeiterViewModel(mitarVm.GetModel()));
                }
            }
        }

    }
}
