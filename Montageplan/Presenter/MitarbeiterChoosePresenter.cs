using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Montageplan.View.Windows;
using Montageplan.Model;
using Montageplan.ViewModel;
using Montageplan.Model.DAL.Database;

namespace Montageplan.Presenter
{
    public class MitarbeiterChoosePresenter
    {
        private readonly MitarbeiterChooseWindow window;
        private readonly int kolonneId;
        private readonly MitarbeiterRepository kolonnenMitarbeiter;
        private readonly RepositoryContainer repositories;
        private List<MitarbeiterChooseViewModel> viewModels;

        public MitarbeiterChoosePresenter(
            MitarbeiterChooseWindow win, int kolonneId, MitarbeiterRepository kolonnenMitarbeiter, RepositoryContainer repositories)
        {
            this.window = win;
            this.kolonneId = kolonneId;
            this.repositories = repositories;
            this.kolonnenMitarbeiter = kolonnenMitarbeiter;
            this.viewModels = new List<MitarbeiterChooseViewModel>();

            this.window.btnCancel.Click += BtnCancelClick;
            this.window.btnOk.Click += BtnOkClick;
        }

        public void ShowDialog()
        {
            this.UpdateView();
            this.window.ShowDialog();
        }

        public bool GetDialogResult()
        {
            bool result = this.window.DialogResult.Value;
            return result;
        }

        public List<MitarbeiterChooseViewModel> GetViewModels()
        {
            return new List<MitarbeiterChooseViewModel>(this.viewModels);
        }

        private void UpdateView()
        {
            this.viewModels = new List<MitarbeiterChooseViewModel>();
            List<Mitarbeiter> freeMitarbeiter =
                this.repositories.GetFreeMitarbeiter(this.kolonneId);

            if (freeMitarbeiter.Count > 0)
                this.viewModels.AddRange(MitarbeiterChooseViewModel.CreateChooseViewModels(
                    freeMitarbeiter.OrderBy(item => item.GetFullName()), this.kolonnenMitarbeiter.GetAll()));

            this.window.mitarbeiterListView.ItemsSource = this.viewModels;
        }

        // EventHandler

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            this.window.DialogResult = true;
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            this.window.DialogResult = false;
        }

    }
}