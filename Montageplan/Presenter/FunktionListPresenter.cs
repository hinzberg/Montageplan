using Montageplan.Model;
using Montageplan.View.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using Montageplan.Model.DAL.Database;
using System.Windows.Data;
using System.ComponentModel;
using Montageplan.ViewModel;

namespace Montageplan.Presenter
{
    public class FunktionListPresenter
    {
        private readonly FunktionListWindow window;
        private readonly FunktionRepository funktionen;
        private readonly MitarbeiterRepository mitarbeier;
        private ObservableCollection<MitarbeiterFunktionViewModel> funktionViewModels;

        public FunktionListPresenter(FunktionListWindow win, FunktionRepository funktionen, MitarbeiterRepository mitarbeier)
        {
            this.window = win;
            this.window.mainToolbar.NewButtonText = "Neue Funktion";
            this.funktionen = funktionen;
            this.mitarbeier = mitarbeier;
            this.funktionViewModels = new ObservableCollection<MitarbeiterFunktionViewModel>();
            (this.window.funktionenListView.Items as CollectionView).SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            this.window.mainToolbar.NewButtonClicked += MainToolbarNewButtonClicked;
            this.window.mainToolbar.EditButtonClicked += MainToolbarEditButtonClicked;
            this.window.mainToolbar.DeleteButtonClicked += MainToolbarDeleteButtonClicked;

            this.window.btnOk.Click += btnOk_Click;
            this.window.newFunktionMenuItem.Click += NewFunktionMenuItemClick;
            this.window.deleteFunktionMenuItem.Click += DeleteFunktionMenuItemClick;
            this.window.editFunktionMenuItem.Click += editFunktionMenuItem_Click;
            this.window.funktionenListView.SelectionChanged += FunktionenListViewSelectionChanged;
            this.window.funktionenListView.ContextMenuOpening += FunktionenListViewContextMenuOpening;
            this.window.funktionenListView.AddHandler(
                ListViewItem.MouseDoubleClickEvent, new MouseButtonEventHandler(this.ListViewItem_MouseDoubleClick), true);

            this.ValidateToolbarButtons();
        }

        public void ShowDialog()
        {
            this.UpdateView();
            this.window.ShowDialog();
        }


        void FunktionenListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ValidateToolbarButtons();
        }

        private void ValidateToolbarButtons()
        {
            if (this.window.funktionenListView.SelectedItems.Count > 0)
            {
                this.window.mainToolbar.SetEnabledDeleteButton(true);
                this.window.mainToolbar.SetEnabledEditButton(true);
            }
            else
            {
                this.window.mainToolbar.SetEnabledDeleteButton(false);
                this.window.mainToolbar.SetEnabledEditButton(false);
            }
        }

        void MainToolbarNewButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenNewFunktionWindow();
        }

        void MainToolbarEditButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenEditFunktionWindow();
        }

        void MainToolbarDeleteButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DeleteSelectedFunktionen();
        }

        public bool GetDialogResult()
        {
            return this.window.DialogResult.Value;
        }

        private void UpdateView()
        {
            this.funktionViewModels = new ObservableCollection<MitarbeiterFunktionViewModel>(
                MitarbeiterFunktionViewModel.CreateViewModels(this.funktionen.GetAll()));

            this.window.funktionenListView.ItemsSource = this.funktionViewModels;
        }

        private void OpenEditFunktionWindow()
        {
            MitarbeiterFunktionViewModel selected = this.GetFirstSelected();
            if (selected != null)
            {
                FunktionEditWindow editWin = new FunktionEditWindow();
                editWin.Owner = this.window;
                FunktionEditPresenter presenter = new FunktionEditPresenter(editWin);
                presenter.EditFunktion(selected.GetModel());
                presenter.ShowDialog();
                if (presenter.GetDialogResult())
                {
                    selected.NotifyPropertyChanged();
                    FunktionenDbTable funktionenTable = new FunktionenDbTable();
                    funktionenTable.Update(selected.GetModel());
                }   
            }
        }

        private void OpenNewFunktionWindow()
        {
            FunktionEditWindow window = new FunktionEditWindow();
            window.Owner = this.window;
            FunktionEditPresenter presenter = new FunktionEditPresenter(window);
            presenter.CreateNewFunktion();
            presenter.ShowDialog();
            if (presenter.GetDialogResult())
            {
                MitarbeiterFunktion funktion = presenter.GetFunktion();
                // Neue Funktion hinzufügen wenn es den Namen noch nicht gibt.
                if (!this.funktionen.Contains(funktion))
                {
                    this.funktionen.Add(funktion);
                    this.funktionViewModels.Add(new MitarbeiterFunktionViewModel(funktion));
                    FunktionenDbTable funktionenTable = new FunktionenDbTable();
                    funktionenTable.Insert(funktion);
                }
                else
                {
                    MessageBoxHelper.ShowInformation("Eine Funktion mit diesem Namen gibt es bereits.");
                }
            }
        }

        private void DeleteSelectedFunktionen()
        {
            List<MitarbeiterFunktionViewModel> selection = this.GetSelection();
            if (selection.Count > 0)
            {
                FunktionenDbTable funktionenTable = new FunktionenDbTable();
                foreach (var item in selection)
                {
                    MitarbeiterFunktion deletedFunktion = item.GetModel();

                    this.funktionViewModels.Remove(item);
                    this.funktionen.Remove(deletedFunktion);
                    funktionenTable.Delete(deletedFunktion);

                    foreach (var mitar in this.mitarbeier.GetWithFunktion(deletedFunktion.Id))
                        mitar.Funktion = null;
                }
            }
        }

        private List<MitarbeiterFunktionViewModel> GetSelection()
        {
            List<MitarbeiterFunktionViewModel> selection = new List<MitarbeiterFunktionViewModel>();
            foreach (var item in this.window.funktionenListView.SelectedItems)
                selection.Add((MitarbeiterFunktionViewModel)item);

            return selection;
        }

        private MitarbeiterFunktionViewModel GetFirstSelected()
        {
            MitarbeiterFunktionViewModel selected = null;
            if (this.window.funktionenListView.SelectedItems.Count > 0)
                selected = (MitarbeiterFunktionViewModel)this.window.funktionenListView.SelectedItem;
            return selected;
        }

        // EventHandler

        void FunktionenListViewContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            bool isItemSelected = (this.window.funktionenListView.SelectedItems.Count == 0);
            this.window.editFunktionMenuItem.IsEnabled = !isItemSelected;
            this.window.deleteFunktionMenuItem.IsEnabled = !isItemSelected;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.OpenEditFunktionWindow();
        }

        void editFunktionMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenEditFunktionWindow();
        }

        void NewFunktionMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenNewFunktionWindow();
        }

        void DeleteFunktionMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DeleteSelectedFunktionen();
        }

        void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.window.Close();
        }

    }
}