using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Montageplan.Model;
using Montageplan.View.Windows;

namespace Montageplan.Presenter
{
    public class HolidayEditPresenter
    {
        private readonly HolidayEditWindow window;
        private readonly ControlInputValidator inputValidator;
        private MitarbeiterFehlzeit holiday;

        public HolidayEditPresenter(HolidayEditWindow win, MitarbeiterFehlzeit holi)
        {
            this.window = win;
            this.holiday = holi;
            this.window.btnStartDate.Click += BtnStartDateClick;
            this.window.btnEndDate.Click += BtnEndDateClick;
            this.window.btnSubmit.Click += BtnSubmitClick;
            this.window.btnCancel.Click += BtnCancelClick;
            this.inputValidator = new ControlInputValidator();
            this.InitTextBoxValidator();

            this.window.cmbHolidayDescription.Items.Add("Urlaub");
            this.window.cmbHolidayDescription.Items.Add("Krankheit");
            this.window.cmbHolidayDescription.Items.Add("Sonstiges");
            this.window.cmbHolidayDescription.SelectedIndex = 0;
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
            if (this.holiday != null)
            {
                this.window.cmbHolidayDescription.SelectedItem = this.holiday.Bezeichnung;
                this.window.txtStartDatum.Text = PropertyFormatter.FormatDate(this.holiday.StartDatum);
                this.window.txtEnddatum.Text = PropertyFormatter.FormatDate(this.holiday.EndeDatum);
            }
        }

        private void UpdateModel()
        {
            if (this.holiday != null)
            {
                this.holiday.Bezeichnung = this.window.cmbHolidayDescription.SelectedItem as string;
                this.holiday.StartDatum = (DateTime)this.inputValidator.GetValue(this.window.txtStartDatum);
                this.holiday.EndeDatum = (DateTime)this.inputValidator.GetValue(this.window.txtEnddatum);
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

        void BtnStartDateClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DateTime newStartdatum = this.OpenDatePickerWindow(this.ParseDateTime(this.window.txtStartDatum.Text.Trim()));
            if (newStartdatum != DateTime.MinValue)
                this.window.txtStartDatum.Text = newStartdatum.ToShortDateString();
        }


        void BtnEndDateClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DateTime newEndedatum = this.OpenDatePickerWindow(this.ParseDateTime(this.window.txtEnddatum.Text.Trim()));
            if (newEndedatum != DateTime.MinValue)
                this.window.txtEnddatum.Text = newEndedatum.ToShortDateString();
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

        private void InitTextBoxValidator()
        {
            //this.textBoxValidator.AddInputValidator(new TextBoxInputValidator.ValidatorItem(
            //    new TextBoxInputValidator.NotEmptyInputValidator(), this.window.txtBezeichnung, "Bitte geben Sie eine Bezeichnung an!"));

            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.DateTimeInputValidator(), this.window.txtStartDatum, "Das Startdatum ist kein gültiges Datum!"));

            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.DateTimeInputValidator(), this.window.txtEnddatum, "Das Endedatum ist kein gültiges Datum!"));
        }

        private bool IsInputValid()
        {
            bool isValid = this.inputValidator.Validate();
            return isValid;
        }
    }
}
