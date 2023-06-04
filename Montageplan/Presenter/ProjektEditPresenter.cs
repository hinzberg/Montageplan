using System;
using System.Windows;
using System.Windows.Media;
using Montageplan.Model;
using Montageplan.View.Windows;
using Montageplan.Windows;
using Montageplan.Model.DAL.Database;
using System.Windows.Controls;

namespace Montageplan.Presenter
{
    public class ProjektEditPresenter
    {
        private readonly ProjektEditWindow window;
        private readonly ControlInputValidator inputValidator;
        private Projekt projekt;

        public ProjektEditPresenter(ProjektEditWindow window)
        {
            this.window = window;
            this.projekt = null;
            this.inputValidator = new ControlInputValidator();
            this.InitTextBoxValidator();

            this.window.comboStartzeit.ItemsSource = TimePickerItemViewModel.CreateList(0, 23);
            this.window.comboEndzeit.ItemsSource = TimePickerItemViewModel.CreateList(0, 23);
            this.window.btnSubmit.Click += BtnSubmitClick;
            this.window.btnCancel.Click += BtnCancelClick;
            this.window.btnDate.Click += BtnDateClick;
            this.window.colorDisplay.MouseDown += ShowColorPickerWindow;
        }

        public void ShowDialog()
        {
            this.window.txtBezeichnung.MaxLength = Projekt.MAX_LENGTH_NAME;

            this.UpdateView();
            this.window.txtBezeichnung.Focus();
            this.window.ShowDialog();
        }

        public bool GetDialogResult()
        {
            return this.window.DialogResult.Value;
        }

        public Projekt GetProjekt()
        {
            return this.projekt;
        }

        public void CreateNewProjekt()
        {
            this.projekt = new Projekt();
            this.projekt.Name = "";
            this.projekt.Startdatum = null;
            this.projekt.Endedatum = null;
            this.window.Title = "Neues Projekt";
        }

        public void CreateNewProjektForDate(DateTime date)
        {
            this.projekt = new Projekt();
            this.projekt.IsTemplate = false;
            this.projekt.Name = "";
            this.projekt.Startdatum = date;
            this.projekt.Endedatum = date;

            this.window.Title = "Neues Projekt";
            this.window.stackStartDatum.Visibility = Visibility.Collapsed;
            this.window.stackEndDatum.Visibility = Visibility.Collapsed;
            this.window.btnDate.Visibility = Visibility.Collapsed;
            this.window.checkIsCompleted.Visibility = Visibility.Collapsed;
        }

        public void EditProjekt(Projekt projekt)
        {
            this.projekt = projekt;

            this.window.Title = "Projekt bearbeiten";
            //this.UpdateView();
        }

        private void InitTextBoxValidator()
        {
            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.NotEmptyInputValidator(), this.window.txtBezeichnung, "Bitte geben Sie eine Bezeichnung an!"));

            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.NullableDateTimeInputValidator(), this.window.txtStartDatum, "Das Startdatum ist kein gültiges Datum!"));

            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.NullableDateTimeInputValidator(), this.window.txtEnddatum, "Das Endedatum ist kein gültiges Datum!"));
        }

        private bool IsInputValid()
        {
            bool isValid = this.inputValidator.Validate();
            if (isValid)
            {
                // Zusätzliche Überprüfung
                bool hasStartText = (this.window.txtStartDatum.Text.Trim() != "");
                bool hasEndText = (this.window.txtEnddatum.Text.Trim() != "");
                DateTime? startTime = (this.window.comboStartzeit.SelectedItem as TimePickerItemViewModel).Model;
                DateTime? endTime = (this.window.comboEndzeit.SelectedItem as TimePickerItemViewModel).Model;

                if ((hasStartText && !hasEndText) || (!hasStartText && hasEndText))
                {
                    const string message = "Ein Projekt ist nur gültig, wenn entweder kein Datum angegeben wird oder ein Startdatum mit einem Enddatum.";
                    MessageBoxHelper.ShowOkBoxWindow("", message, this.window);
                    isValid = false;
                }
                else if ((DateTime?)this.inputValidator.GetValue(this.window.txtStartDatum) >
                    (DateTime?)this.inputValidator.GetValue(this.window.txtEnddatum))
                {
                    const string message = "Das Startdatum muss vor dem Endedatum liegen.";
                    MessageBoxHelper.ShowOkBoxWindow("", message, this.window);
                    isValid = false;
                }
                else if ((startTime.HasValue && !endTime.HasValue) || (!startTime.HasValue && endTime.HasValue))
                {
                    const string message = "Ein Projekt ist nur gültig, wenn entweder keine Zeit angegeben wird oder eine Startzeit mit einer Endzeit.";
                    MessageBoxHelper.ShowOkBoxWindow("", message, this.window);
                    isValid = false;
                }
                else if ((startTime.HasValue && endTime.HasValue) && (startTime.Value > endTime.Value))
                {
                    const string message = "Die Startzeit muss vor der Endzeit liegen.";
                    MessageBoxHelper.ShowOkBoxWindow("", message, this.window);
                    isValid = false;
                }
            }

            return isValid;
        }

        private void UpdateView()
        {
            if (this.projekt != null)
            {
                this.window.txtBezeichnung.Text = this.projekt.Name;
                this.window.colorDisplay.Background = this.projekt.ProjectColorBrush;
                this.window.txtStartDatum.Text = PropertyFormatter.FormatDateWithoutPlaceholder(this.projekt.Startdatum);
                this.window.txtEnddatum.Text = PropertyFormatter.FormatDateWithoutPlaceholder(this.projekt.Endedatum);
                this.SelectProjektTime(this.projekt.GetStartTime(), this.window.comboStartzeit);
                this.SelectProjektTime(this.projekt.GetEndTime(), this.window.comboEndzeit);
                this.window.checkIsCompleted.IsChecked = this.projekt.IsCompleted;
            }
        }

        private void SelectProjektTime(DateTime? dateTime, ComboBox comboBox)
        {
            foreach (var item in comboBox.ItemsSource)
            {
                DateTime? dtItem = (item as TimePickerItemViewModel).Model;
                if (!dtItem.HasValue && !dateTime.HasValue)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
                else if (dtItem.HasValue && dateTime.HasValue && 
                    (dtItem.Value.Hour == dateTime.Value.Hour) && (dtItem.Value.Minute == dateTime.Value.Minute))
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void UpdateProjekt()
        {
            if (this.projekt != null)
            {
                DateTime? startTime = (this.window.comboStartzeit.SelectedItem as TimePickerItemViewModel).Model;
                DateTime? endTime = (this.window.comboEndzeit.SelectedItem as TimePickerItemViewModel).Model;

                this.projekt.Name = (string)this.inputValidator.GetValue(this.window.txtBezeichnung);
                this.projekt.ProjectColorBrush = (SolidColorBrush)this.window.colorDisplay.Background;
                this.projekt.Startdatum = (DateTime?)this.inputValidator.GetValue(this.window.txtStartDatum);
                this.projekt.Endedatum = (DateTime?)this.inputValidator.GetValue(this.window.txtEnddatum);

                if (this.projekt.Startdatum.HasValue && startTime.HasValue)
                    this.projekt.Startdatum = new DateTime(
                        this.projekt.Startdatum.Value.Year, this.projekt.Startdatum.Value.Month, this.projekt.Startdatum.Value.Day,
                        startTime.Value.Hour, startTime.Value.Minute, 0);

                if (this.projekt.Endedatum.HasValue && endTime.HasValue)
                    this.projekt.Endedatum = new DateTime(
                        this.projekt.Endedatum.Value.Year, this.projekt.Endedatum.Value.Month, this.projekt.Endedatum.Value.Day,
                        endTime.Value.Hour, endTime.Value.Minute, 0);

                this.projekt.IsCompleted = this.window.checkIsCompleted.IsChecked.Value;
            }
        }

        /// <summary>
        ///  Öffnet DualDatePickerWindow um Start- und Enddatum auswählen.
        ///  Wird ein DateTime.MinValue übergeben, wird das Datum von Heute angezeigt.
        /// </summary>
        private void OpenDatePickerWindow()
        {
            DateTime startDate = this.ParseDateTime(this.window.txtStartDatum.Text.Trim());
            DateTime endDate = this.ParseDateTime(this.window.txtEnddatum.Text.Trim());

            DualDatePickerWindow dualDatePicker = new DualDatePickerWindow();
            dualDatePicker.Owner = this.window;

            DateTime date = DateTime.MinValue;

            // Startdatum setzten
            if (startDate == DateTime.MinValue)
                dualDatePicker.SelectedStartDate = DateTime.Today;
            else
                dualDatePicker.SelectedStartDate = startDate;

            // Endedatum setzten
            if (startDate == DateTime.MinValue)
                dualDatePicker.SelectedEndDate = DateTime.Today;
            else
                dualDatePicker.SelectedEndDate = endDate;

            // Pickerfenster anzeigen
            if (dualDatePicker.ShowDialog().Value)
            {
                // Fenster mit übernehmen beendet.
                if (dualDatePicker.SelectedStartDate != DateTime.MinValue)
                    this.window.txtStartDatum.Text = dualDatePicker.SelectedStartDate.ToShortDateString();

                if (dualDatePicker.SelectedEndDate != DateTime.MinValue)
                    this.window.txtEnddatum.Text = dualDatePicker.SelectedEndDate.ToShortDateString();
            }
        }

        /// <summary>
        /// Formatiert den übergebenen String zu einer DateTime und gibt sie zurück.
        /// </summary>
        /// <param name="dateString">String, der zur Date formatiert wird. Format: dd.mm.yyyy</param>
        /// <returns></returns>
        private DateTime ParseDateTime(string dateString)
        {
            DateTime date = DateTime.MinValue;

            DateTime tempDate;
            if (DateTime.TryParse(dateString, out tempDate))
                date = tempDate;

            return date;
        }

        // EventHandler //

        /// <summary>
        /// ColorPicker anzeigen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ShowColorPickerWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ColorPickerWindow colorPickerWindow = new ColorPickerWindow(this.projekt.ProjectColorBrush.Color);
            colorPickerWindow.Owner = this.window;
            bool? res = colorPickerWindow.ShowDialog();
            if (res != null && res == true)
            {
                Color col = colorPickerWindow.SelectedColor;
                this.window.colorDisplay.Background = new SolidColorBrush(col);
            }
        }

        private void BtnDateClick(object sender, RoutedEventArgs e)
        {
            this.OpenDatePickerWindow();
        }

        private void BtnSubmitClick(object sender, RoutedEventArgs e)
        {
            bool isValid = this.IsInputValid();
            if (isValid)
            {
                this.UpdateProjekt();
                this.window.DialogResult = true;
            }
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            this.window.DialogResult = false;
        }
    }
}