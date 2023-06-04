using Montageplan.Model;
using Montageplan.View.Windows;
using Montageplan.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Presenter
{
    public class MitarbeiterEditPresenter
    {
        private const string WITHOUT_FUNCTION_TEXT = "(Ohne Funktion)";

        private readonly MitarbeiterEditWindow window;
        private readonly FunktionRepository funktionen;
        private readonly Repository<Fehlzeitart> fehlzeitarten;
        private readonly ControlInputValidator inputValidator;
        private Mitarbeiter mitarbeiter;

        public MitarbeiterEditPresenter(MitarbeiterEditWindow window, FunktionRepository funktionen, Repository<Fehlzeitart> fehlzeitarten)
        {
            this.window = window;
            this.funktionen = funktionen;
            this.fehlzeitarten = fehlzeitarten;
            this.inputValidator = new ControlInputValidator();
            this.InitTextBoxValidator();

            this.window.btnSubmit.Click += BtnSubmitClick;
            this.window.btnCancel.Click += BtnCancelClick;
            this.window.btnNew.Click += BtnNewHolidayClick;
        }

        public void ShowDialog()
        {
            MitarbeiterFunktion empty = new MitarbeiterFunktion();
            empty.Name = WITHOUT_FUNCTION_TEXT;
            List<MitarbeiterFunktion> funktions = new List<MitarbeiterFunktion> { empty };
            funktions.AddRange(this.funktionen.GetAll());

            this.window.txtName.MaxLength = Mitarbeiter.MAX_LENGHT_NAME;
            this.window.txtVorname.MaxLength = Mitarbeiter.MAX_LENGHT_VORNAME;
            this.window.txtBezeichnung.MaxLength = Mitarbeiter.MAX_LENGHT_BEZEICHNUNG;

            this.window.comboFunktion.ItemsSource = funktions;
            this.UpdateView();
            this.window.txtVorname.Focus();
            this.window.ShowDialog();
        }

        public bool GetDialogResult()
        {
            return this.window.DialogResult.Value;
        }

        public Mitarbeiter GetMitarbeiter()
        {
            return this.mitarbeiter;
        }

        public void CreateNewMitarbeiter()
        {
            this.mitarbeiter = new Mitarbeiter();

            this.window.Title = "Neuer Mitarbeiter";
            this.UpdateView();
        }

        public void EditMitarbeiter(Mitarbeiter mitarbeiter)
        {
            this.mitarbeiter = mitarbeiter;

            this.window.Title = "Mitarbeiter bearbeiten";
            this.UpdateView();
        }

        private void InitTextBoxValidator()
        {
            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.NotEmptyInputValidator(), this.window.txtVorname, "Bitte geben Sie einen Vornamen an!"));

            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.NotEmptyInputValidator(), this.window.txtName, "Bitte geben Sie einen Nachnamen an!"));

            ////this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
            ////    new ControlInputValidator.NotEmptyInputValidator(), this.window.comboFunktion, "Bitte wählen Sie eine Funktion aus!"));
        }

        private void UpdateView()
        {
            if (this.mitarbeiter != null)
            {
                this.window.txtVorname.Text = this.mitarbeiter.Vorname;
                this.window.txtName.Text = this.mitarbeiter.Name;
                this.window.txtBezeichnung.Text = this.mitarbeiter.Bezeichnung;
                
                if (this.mitarbeiter.Funktion == null)
                {
                    this.window.comboFunktion.SelectedIndex = 0;
                }
                else
                {
                    this.window.comboFunktion.SelectedItem = this.mitarbeiter.Funktion;
                }

                this.window.checkKannFuehrerSein.IsChecked = this.mitarbeiter.KannFuehrerSein;
                this.window.checkAktiv.IsChecked = true;
                this.window.FehltageCrtl.SetDays(0); // ToDo-Muss noch
            }
        }

        private bool IsInputValid()
        {
            bool isValid = this.inputValidator.Validate();
            return isValid;
        }

        private void UpdateMitarbeiter()
        {
            if (this.mitarbeiter != null)
            {
                MitarbeiterFunktion funktion = this.window.comboFunktion.SelectedItem as MitarbeiterFunktion;
                if (funktion != null && funktion.Name == WITHOUT_FUNCTION_TEXT)
                {
                    this.mitarbeiter.Funktion = null;
                }
                else
                {
                    this.mitarbeiter.Funktion = funktion;
                }

                this.mitarbeiter.Vorname = (string)this.inputValidator.GetValue(this.window.txtVorname);
                this.mitarbeiter.Name = (string)this.inputValidator.GetValue(this.window.txtName);
                this.mitarbeiter.Bezeichnung = this.window.txtBezeichnung.Text.Trim();
                
                this.mitarbeiter.KannFuehrerSein = this.window.checkKannFuehrerSein.IsChecked.Value;

                // Wert muss Mitarbeiter zugewiesen werden.
                int value = this.window.FehltageCrtl.GetDays();

            }
        }


        // EventHandler //

        void BtnNewHolidayClick(object sender, System.Windows.RoutedEventArgs e)
        {
            HolidayEditWindow holidayEditWindow = new HolidayEditWindow();
            holidayEditWindow.Owner = this.window;
            MitarbeiterFehlzeit holiday = new MitarbeiterFehlzeit();
            // Hier fehlt noch die Mitarbeiternummer oder eine andere Refrenz 
            HolidayEditPresenter holidayEditPresenter = new HolidayEditPresenter(holidayEditWindow, holiday);
            holidayEditPresenter.ShowDialog();

            if (holidayEditPresenter.GetDialogResult())
            {
                // Neuer Urlaub/Krankheit wurde angelegt
                this.mitarbeiter.Fehlzeiten.Add(holiday);
            }
        }

        void BtnSubmitClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.IsInputValid())
            {
                this.UpdateMitarbeiter();
                this.window.DialogResult = true;
            }
        }

        void BtnCancelClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.window.DialogResult = false;
        }


    }
}
