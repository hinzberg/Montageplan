using System.Windows;
using Montageplan.Model;
using Montageplan.View.Windows;

namespace Montageplan.Presenter
{
    public class FunktionEditPresenter
    {
        private readonly FunktionEditWindow window;
        private readonly ControlInputValidator inputValidator;    
        private MitarbeiterFunktion funktion;

        public FunktionEditPresenter(FunktionEditWindow win)
        {
            this.window = win;
            this.inputValidator = new ControlInputValidator();
            this.funktion = null;
            this.InitTextBoxValidator();

            this.window.btnCancel.Click += BtnCancelClick;
            this.window.btnSubmit.Click += BtnSubmitClick;
        }

        public void ShowDialog()
        {
            this.window.txtName.MaxLength = MitarbeiterFunktion.MAX_LENGTH_NAME;

            this.UpdateView();
            this.window.txtName.Focus();
            this.window.ShowDialog();
        }

        private void UpdateView()
        {
            if (this.funktion != null)
            {
                this.window.txtName.Text = this.funktion.Name;
            }
        }

        public bool GetDialogResult()
        {
            return this.window.DialogResult.Value;
        }

        public MitarbeiterFunktion GetFunktion()
        {
            return this.funktion;
        }

        public void CreateNewFunktion()
        {
            this.funktion = new MitarbeiterFunktion();

            this.window.Title = "Neue Funktion";
            this.UpdateView();
        }

        public void EditFunktion(MitarbeiterFunktion funktion)
        {
            this.funktion = funktion;

            this.window.Title = "Funktion bearbeiten";
            this.UpdateView();
        }

        private void InitTextBoxValidator()
        {
            this.inputValidator.AddInputValidator(new ControlInputValidator.ValidatorItem(
                new ControlInputValidator.NotEmptyInputValidator(), this.window.txtName, "Bitte geben Sie eine Bezeichnung an!"));
        }

        private bool IsInputValid()
        {
            bool isValid = this.inputValidator.Validate();
            return isValid;
        }

        private void UpdateFunktion()
        {
            if (this.funktion != null)
            {
                this.funktion.Name = (string)this.inputValidator.GetValue(this.window.txtName);
            }
        }

        // EventHandler

        private void BtnSubmitClick(object sender, RoutedEventArgs e)
        {
            if (this.IsInputValid())
            {
                this.UpdateFunktion();
                this.window.DialogResult = true;
            }
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            this.window.DialogResult = false;
        }


    }
}