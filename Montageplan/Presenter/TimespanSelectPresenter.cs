using System;
using System.Windows;
using Montageplan.Model;
using Montageplan.View.Windows;

namespace Montageplan.Presenter
{
    public class TimespanSelectPresenter
    {
        private readonly ControlInputValidator inputValidator;
        private readonly TimespanSelectWindow window;

        public TimespanSelectPresenter(TimespanSelectWindow win)
        {
            this.window = win;
            this.inputValidator = new ControlInputValidator();
            this.InitTextBoxValidator();
            this.StartDatum = DateTime.MinValue;
            this.EndeDatum = DateTime.MinValue;
            this.window.btnCancel.Click += BtnCancelClick;
            this.window.btnSubmit.Click += BtnSubmitClick;
            this.window.btnDate.Click += BtnDateClick;
        }

        public DateTime StartDatum { get; set; }
        public DateTime EndeDatum { get; set; }

        private void BtnDateClick(object sender, RoutedEventArgs e)
        {
            this.OpenDatePickerWindow();
        }

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


        private void BtnSubmitClick(object sender, RoutedEventArgs e)
        {
            bool isValid = this.IsInputValid();
            if (isValid)
            {
                this.UpdateModel();
                this.window.DialogResult = true;
            }
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            this.window.DialogResult = false;
        }

        public void ShowDialog()
        {
            this.UpdateView();
            this.window.ShowDialog();
        }

        public bool GetDialogResult()
        {
            return this.window.DialogResult.Value;
        }

        private void UpdateView()
        {
            this.window.txtStartDatum.Text = PropertyFormatter.FormatDate(this.StartDatum);
            this.window.txtEnddatum.Text = PropertyFormatter.FormatDate(this.EndeDatum);
        }

        private void UpdateModel()
        {
            this.StartDatum = (DateTime) this.inputValidator.GetValue(this.window.txtStartDatum);
            this.EndeDatum = (DateTime) this.inputValidator.GetValue(this.window.txtEnddatum);
        }

        private void InitTextBoxValidator()
        {
            //this.textBoxValidator.AddInputValidator(new TextBoxInputValidator.ValidatorItem(
            //    new TextBoxInputValidator.NotEmptyInputValidator(), this.window.txtBezeichnung, "Bitte geben Sie eine Bezeichnung an!"));

            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                                                      new ControlInputValidator.DateTimeInputValidator(),
                                                      this.window.txtStartDatum,
                                                      "Das Startdatum ist kein gültiges Datum!"));

            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                                                      new ControlInputValidator.DateTimeInputValidator(),
                                                      this.window.txtEnddatum, "Das Endedatum ist kein gültiges Datum!"));
        }

        private bool IsInputValid()
        {
            bool isValid = this.inputValidator.Validate();
            return isValid;
        }

        /// <summary>
        /// Öffnet das DatePickerWindow um ein Datum auszuwählen. Das ausgewählte Datum wird zurückgegeben.
        /// </summary>
        /// <param name="selectedDate">Das im DatePickerWindow ausgewählte Datum beim Öffnen des Fensters.
        /// Wird ein DateTime.MinValue übergeben, wird das Datum von Heute angezeigt.</param>
        /// <returns></returns>
        private DateTime OpenDatePickerWindow(DateTime selectedDate)
        {
            DateTime date = DateTime.MinValue;

            DatePickerWindow datePickerWindow = new DatePickerWindow();
            datePickerWindow.Owner = this.window;
            if (selectedDate == DateTime.MinValue)
                datePickerWindow.SelectedDate = DateTime.Today;
            else
                datePickerWindow.SelectedDate = selectedDate;

            if (datePickerWindow.ShowDialog().Value)
                date = datePickerWindow.SelectedDate;

            return date;
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
    }
}