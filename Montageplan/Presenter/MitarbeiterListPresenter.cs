using Montageplan.Model;
using Montageplan.Model.DAL.Database;
using Montageplan.ViewModel;
using Montageplan.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Montageplan.Presenter
{
    public class MitarbeiterListPresenter
    {
        private readonly MitarbeiterListWindow window;
        private readonly MitarbeiterRepository mitarbeiter;
        private readonly FunktionRepository funktionen;
        private readonly Repository<Fehlzeitart> fehlzeitarten;
        private ObservableCollection<MitarbeiterViewModel> viewModels;

        public MitarbeiterListPresenter(
            MitarbeiterListWindow window, MitarbeiterRepository mitarbeiter, FunktionRepository funktionen, Repository<Fehlzeitart> fehlzeitarten)
        {
            this.window = window;
            this.mitarbeiter = mitarbeiter;
            this.funktionen = funktionen;
            this.fehlzeitarten = fehlzeitarten;
            this.window.mainToolbar.NewButtonText = "Neuer Mitarbeiter";


            (this.window.mitarbeiterListView.Items as CollectionView).SortDescriptions.Add(new SortDescription("NameToSort", ListSortDirection.Ascending));
            this.viewModels = new ObservableCollection<MitarbeiterViewModel>();

            this.window.mainToolbar.NewButtonClicked += MainToolbarNewButtonClicked;
            this.window.mainToolbar.DeleteButtonClicked += MainToolbarDeleteButtonClicked;
            this.window.mainToolbar.EditButtonClicked += MainToolbarEditButtonClicked;


            this.window.NewMitarbeiterMenuItem.Click += NewMitarbeiterMenuItemClick;
            this.window.EditMitarbeiterMenuItem.Click += EditMitarbeiterMenuItemClick;
            this.window.DeleteMitarbeitertMenuItem.Click += DeleteMitarbeiterMenuItemClick;
            this.window.btnOk.Click += btnOk_Click;
            this.window.mitarbeiterListView.ContextMenuOpening += MitarbeiterListViewContextMenuOpening;
            this.window.mitarbeiterListView.SelectionChanged += MitarbeiterListViewSelectionChanged;
            this.window.mitarbeiterListView.AddHandler(
                ListViewItem.MouseDoubleClickEvent, new MouseButtonEventHandler(this.ListViewItem_MouseDoubleClick), true);

            this.ValidateToolbarButtons();
        }

        void MitarbeiterListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ValidateToolbarButtons();
        }

        public void ShowDialog()
        {
            this.window.deleteMitarbeiterMenuItem.IsEnabled = false;
            this.window.mainToolbar.deleteButton.IsEnabled = false;

            this.UpdateView();
            this.window.ShowDialog();
        }

        private void ValidateToolbarButtons()
        {
            if (this.window.mitarbeiterListView.SelectedItems.Count > 0)
            {
                //this.window.mainToolbar.SetEnabledDeleteButton(true);
                this.window.mainToolbar.SetEnabledEditButton(true);
            }
            else
            {
                //this.window.mainToolbar.SetEnabledDeleteButton(false);
                this.window.mainToolbar.SetEnabledEditButton(false);
            }
        }

        void MainToolbarNewButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenNewMitarbeiterWindow();
        }

        void MainToolbarEditButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenEditMitarbeiterWindow();
        }

        void MainToolbarDeleteButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DeleteSelectedMitarbeiter();
        }

        private void UpdateView()
        {
            this.viewModels = new ObservableCollection<MitarbeiterViewModel>(MitarbeiterViewModel.CreateViewModels(this.mitarbeiter.GetAll()));
            this.window.mitarbeiterListView.ItemsSource = this.viewModels;
        }

        private void OpenEditMitarbeiterWindow()
        {
            MitarbeiterViewModel selected = this.GetFirstSelected();
            if (selected != null)
            {
                MitarbeiterEditPresenter presenter = new MitarbeiterEditPresenter(new MitarbeiterEditWindow(), this.funktionen, this.fehlzeitarten);
                presenter.EditMitarbeiter(selected.GetModel());
                presenter.ShowDialog();
                if (presenter.GetDialogResult())
                {
                    selected.NotifyPropertyChanged();
                    MitarbeiterDbTable mitarbeiterTable = new MitarbeiterDbTable();
                    mitarbeiterTable.Update(selected.GetModel());
                }
            }
        }

        private void OpenNewMitarbeiterWindow()
        {
            MitarbeiterEditPresenter presenter = new MitarbeiterEditPresenter(new MitarbeiterEditWindow(), this.funktionen, this.fehlzeitarten);
            presenter.CreateNewMitarbeiter();
            presenter.ShowDialog();
            if (presenter.GetDialogResult())
            {
                Mitarbeiter newMitarbeiter = presenter.GetMitarbeiter();
                this.mitarbeiter.Add(newMitarbeiter);
                this.viewModels.Add(new MitarbeiterViewModel(newMitarbeiter));
                MitarbeiterDbTable mitarbeiterTable = new MitarbeiterDbTable();
                mitarbeiterTable.Insert(newMitarbeiter);
            }
        }

        private void DeleteSelectedMitarbeiter()
        {
            List<MitarbeiterViewModel> selection = this.GetSelection();
            if (selection.Count > 0)
            {
                MitarbeiterDbTable mitarbeiterTable = new MitarbeiterDbTable();
                foreach (var item in selection)
                {
                    Mitarbeiter deletedMitarbeiter = item.GetModel();

                    this.viewModels.Remove(item);
                    this.mitarbeiter.Remove(deletedMitarbeiter);
                    mitarbeiterTable.Delete(deletedMitarbeiter);
                }
            }
        }

        private List<MitarbeiterViewModel> GetSelection()
        {
            List<MitarbeiterViewModel> selection = new List<MitarbeiterViewModel>();
            foreach (var item in this.window.mitarbeiterListView.SelectedItems)
                selection.Add((MitarbeiterViewModel)item);

            return selection;
        }

        private MitarbeiterViewModel GetFirstSelected()
        {
            MitarbeiterViewModel selected = null;
            if (this.window.mitarbeiterListView.SelectedItems.Count > 0)
                selected = (MitarbeiterViewModel)this.window.mitarbeiterListView.SelectedItem;
            return selected;
        }



        /// <summary>
        /// Contextmenü, Neu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NewMitarbeiterMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenNewMitarbeiterWindow();
        }

        /// <summary>
        /// Contextmenü, Bearbeiten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EditMitarbeiterMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenEditMitarbeiterWindow();
        }

        /// <summary>
        /// Contextmenü, Löschen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteMitarbeiterMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DeleteSelectedMitarbeiter();
        }

        void MitarbeiterListViewContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            bool isItemSelected = (this.window.mitarbeiterListView.SelectedItems.Count == 0);
            this.window.EditMitarbeiterMenuItem.IsEnabled = !isItemSelected;
            //this.window.DeleteMitarbeitertMenuItem.IsEnabled = !isItemSelected;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.OpenEditMitarbeiterWindow();
        }

        void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.window.Close();
        }


    }
}
