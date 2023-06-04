using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Montageplan.Model
{
    /// <summary>
    /// Überprüft TextBox- oder ComboBox-Eingaben auf ihre Richtigkeit. Die Richtigkeit der Eingaben wird in
    /// übergebenen 'InputValidators' überprüft. Wenn Falscheingaben vorliegen, wird der Rahmen der jeweiligen 'InputBox'
    /// rot gefärbt. Zusätzlich kann eine Meldung angezeigt werden, indem darauf hingewiesen wird, wo ein Eingabefehler vorliegt.
    /// </summary>
    public class ControlInputValidator
    {
        /// <summary>
        /// Interface für ein Eingabe-Validator.
        /// </summary>
        public interface InputValidator
        {
            bool IsValid(string text);
            object GetValue(string text);
        }
        /// <summary>
        /// DateTime Eingabe-Validator. Die Eingabe ist ungültig, wenn sich der Text nicht in ein DateTime-Objekt parsen lässt.
        /// Optional muss auch kein Text angegeben werden.
        /// </summary>
        public class DateTimeInputValidator : InputValidator
        {
            public DateTimeInputValidator()
                : this(false)
            {
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="allowEmptyText">Darf in der TextBox auch kein Text stehen ?</param>
            public DateTimeInputValidator(bool allowEmptyText)
            {
                this.AllowEmptyText = allowEmptyText;
            }

            /// <summary>
            /// Darf in der InputBox auch kein Text stehen ?
            /// </summary>
            public bool AllowEmptyText { get; set; }

            public bool IsValid(string text)
            {
                bool isValid = false;
                if (this.AllowEmptyText && string.IsNullOrEmpty(text.Trim()))
                    isValid = true;
                else
                    isValid = ((DateTime)this.GetValue(text) != DateTime.MinValue);

                return isValid;
            }

            public object GetValue(string text)
            {
                DateTime value = DateTime.MinValue;
                DateTime dateTemp;
                if (DateTime.TryParse(text, out dateTemp))
                    value = dateTemp;
                return value;
            }
        }
        /// <summary>
        /// Nullable DateTime Eingabe-Validator. Die Eingabe ist ungültig, wenn sich der Text nicht in ein DateTime-Objekt parsen lässt.
        /// </summary>
        public class NullableDateTimeInputValidator : InputValidator
        {
            public NullableDateTimeInputValidator()
            {
            }

            public bool IsValid(string text)
            {
                bool isValid = false;
                if (string.IsNullOrEmpty(text.Trim()))
                    isValid = true;
                else
                    isValid = ((DateTime?)this.GetValue(text)).HasValue;

                return isValid;
            }

            public object GetValue(string text)
            {
                DateTime? value = null;
                DateTime dateTemp;
                if (DateTime.TryParse(text, out dateTemp))
                    value = dateTemp;
                return value;
            }
        }
        /// <summary>
        /// Text Eingabe-Validator. Die Eingabe ist ungültig, wenn kein Text vorhanden ist.
        /// </summary>
        public class NotEmptyInputValidator : InputValidator
        {
            public bool IsValid(string text)
            {
                bool isValid = !string.IsNullOrEmpty(this.GetValue(text).ToString());
                return isValid;
            }

            public object GetValue(string text)
            {
                return text.Trim();
            }
        }
        /// <summary>
        /// Der Validator übernimmt das Überprüfen und das Parsen der TextBox-Eingaben.
        /// Hier wird zusätzlich die Färbung der InputBox-Rahmen bei Falscheingaben übernommen.
        /// </summary>
        public class ValidatorItem
        {
            private Brush invalidBorderBrush = Brushes.Red;

            private readonly InputValidator validator;
            private readonly Brush defaultBorderBrush;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="validator">InputValidator</param>
            /// <param name="inputBox">Entweder eine TextBox oder ComboBox</param>
            public ValidatorItem(InputValidator validator, Control inputBox)
                : this(validator, inputBox, "")
            {
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="validator">InputValidator</param>
            /// <param name="inputBox">Entweder eine TextBox oder ComboBox</param>
            /// <param name="invalidText">Meldungstext, wenn die Eingabe ungültig ist</param>
            public ValidatorItem(InputValidator validator, Control inputBox, string invalidText)
            {
                this.validator = validator;
                this.defaultBorderBrush = inputBox.BorderBrush;
                this.InputBox = inputBox;
                this.InvalidText = invalidText;
            }

            /// <summary>
            /// Entweder eine TextBox oder ComboBox
            /// </summary>
            public Control InputBox { get; set; }
            public string InvalidText { get; set; }

            /// <summary>
            /// Färbt den InputBox-Rahmen rot, falls der Rahmen die Ursprungsfarbe hat.
            /// </summary>
            public void NotifyInvalid()
            {
                if (this.InputBox.BorderBrush != invalidBorderBrush)
                    this.InputBox.BorderBrush = invalidBorderBrush;
            }

            /// <summary>
            /// Der InputBox-Rahmen bekommt seine Ursprungsfarbe, falls der Rahmen aktuell rot ist.
            /// </summary>
            public void NotifyValid()
            {
                if (this.InputBox.BorderBrush == invalidBorderBrush)
                    this.InputBox.BorderBrush = defaultBorderBrush;
            }

            /// <summary>
            /// Ist die Eingabe gültig ? Die Eingabe wird durch einen Validator überprüft.
            /// </summary>
            /// <returns></returns>
            public bool IsInputValid()
            {
                bool isValid = this.validator.IsValid(this.GetInput());
                return isValid;
            }

            /// <summary>
            /// Parst die Eingabe mit einem Validator.
            /// </summary>
            /// <returns></returns>
            public object ParseInput()
            {
                return this.validator.GetValue(this.GetInput());
            }

            /// <summary>
            /// Holt den Text aus der TextBox oder der ComboBox.
            /// </summary>
            /// <returns></returns>
            private string GetInput()
            {
                string input = "";
                if(this.InputBox is TextBox)
                    input = (this.InputBox as TextBox).Text;
                else if (this.InputBox is ComboBox)
                    input = (this.InputBox as ComboBox).Text;

                return input;
            }
        }


        private readonly List<ValidatorItem> items;

        public ControlInputValidator()
        {
            this.items = new List<ValidatorItem>();
            this.InvalidMessageTitle = "Ungültige Eingabe";
            this.ShowMessageBox = true;
        }

        /// <summary>
        /// Titel der MessageBox bei mind. einer Falscheingabe.
        /// </summary>
        public string InvalidMessageTitle { get; set; }
        /// <summary>
        /// Soll die MessageBox bei einer Falscheingabe angezeigt werden ? Die MessageBox wird nur einmalig angezeigt,
        /// bei dem ersten gefundenen Eingabefehler.
        /// </summary>
        public bool ShowMessageBox { get; set; }

        /// <summary>
        /// Fügt einen Validator für die InputBox-Eingabe hinzu.
        /// </summary>
        /// <param name="item">Validator</param>
        public void AddInputValidator(ValidatorItem item)
        {
            this.items.Add(item);
        }

        /// <summary>
        /// Überprüft alle InputBox-Eingaben. Falls Falscheinhaben gefunden werden, werden diese InputBox-Rahmen rot gefärbt.
        /// Zusätzlich kann eine MessageBox mit der zuerst gefunden Falscheingabe ausgegeben werden. Die Information,
        /// ob alle Eingaben gültig sind oder nicht, wird zurückgegeben.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool isAllValid = true;
            string messageText = "";
            foreach (var item in this.items)
            {
                bool isValid = item.IsInputValid();
                if (isValid)
                    item.NotifyValid();
                else
                {
                    isAllValid = false;
                    // Den Rahmen von allen InputBoxen mit ungültigen Eingaben rot färben.
                    // Die Meldung aber nur vom ersten Fehlerhaften ausgeben ("Meldungensturm" vermeiden)
                    item.NotifyInvalid();
                    if (this.ShowMessageBox && messageText == "")
                    {
                        // Die MessageBox wird hier noch nicht aufgerufen. Zuerst sollen alle Rahmen rot gefärbt werden
                        // und zum Schluss die Meldung angezeigt werden
                        messageText = item.InvalidText;
                    }
                }
            }
            if (messageText != "")
                MessageBox.Show(messageText, this.InvalidMessageTitle, MessageBoxButton.OK, MessageBoxImage.Warning);

            return isAllValid;
        }

        /// <summary>
        /// Parst die Eingabe der übergebenen TextBox mit Hilfe des Validators und gibt die geparste Eingabe zurück.
        /// </summary>
        /// <param name="inputBox">TextBox oder ComboBox</param>
        /// <returns></returns>
        public object GetValue(Control inputBox)
        {
            object value = null;
            ValidatorItem item = this.GetValidator(inputBox);
            if (item != null)
                value = item.ParseInput();
            return value;
        }

        /// <summary>
        /// Parst die Eingabe und überprüft mit Hilfe des Validators, ob die Eingabe gültig ist. Das Ergebnis wird zurückgegeben.
        /// </summary>
        /// <param name="inputBox">TextBox oder ComboBox</param>
        /// <returns></returns>
        public bool IsValid(Control inputBox)
        {
            bool isValid = false;
            ValidatorItem item = this.GetValidator(inputBox);
            if (item != null)
                isValid = item.IsInputValid();
            return isValid;
        }

        /// <summary>
        /// Holt den Validator der InputBox und gibt ihn zurück.
        /// </summary>
        /// <param name="inputBox">TextBox oder ComboBox</param>
        /// <returns></returns>
        private ValidatorItem GetValidator(Control inputBox)
        {
            ValidatorItem item = null;
            foreach (var itemTemp in this.items)
            {
                if (itemTemp.InputBox == inputBox)
                {
                    item = itemTemp;
                    break;
                }
            }
            return item;
        }

    }
}
