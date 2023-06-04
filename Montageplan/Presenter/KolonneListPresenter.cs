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
    public class KolonneListPresenter
    {
        private readonly KolonneListWindow window;
        private readonly KolonneRepository kolonnen;
        private readonly MitarbeiterRepository mitarbeiter;
        private readonly RepositoryContainer repositories;
        private ObservableCollection<KolonneViewModel> viewModels;

        public KolonneListPresenter(KolonneListWindow window,
            KolonneRepository kolonnen, MitarbeiterRepository mitarbeiter, RepositoryContainer repositories)
        {
            this.window = window;
            this.kolonnen = kolonnen;
            this.mitarbeiter = mitarbeiter;
            this.repositories = repositories;
            this.window.mainToolbar.NewButtonText = "Neue Kolonne";
            (this.window.kolonnenListView.Items as CollectionView).SortDescriptions.Add(new SortDescription("Nummer", ListSortDirection.Ascending));

            this.window.mainToolbar.NewButtonClicked += MainToolbarNewButtonClicked;
            this.window.mainToolbar.DeleteButtonClicked += MainToolbarDeleteButtonClicked;
            this.window.mainToolbar.EditButtonClicked += MainToolbarEditButtonClicked;

            this.window.btnOk.Click += BtnOkClick;
            this.window.NewKolonneMenuItem.Click += NewKolonneMenuItemClick;
            this.window.EditKolonneMenuItem.Click += EditKolonneMenuItemClick;
            this.window.DeleteKolonneMenuItem.Click += DeleteKolonneMenuItemClick;
            this.window.kolonnenListView.ContextMenuOpening += ProjekteListViewContextMenuOpening;
            this.window.kolonnenListView.SelectionChanged += KolonnenListViewSelectionChanged;
            this.window.kolonnenListView.AddHandler(
                ListViewItem.MouseDoubleClickEvent, new MouseButtonEventHandler(this.ListViewItemMouseDoubleClick), true);

            this.ValidateToolbarButtons();
        }

        public void ShowDialog()
        {
            this.window.DeleteKolonneMenuItem.IsEnabled = false;
            this.window.mainToolbar.deleteButton.IsEnabled = false;

            this.UpdateView();
            this.window.ShowDialog();
        }

        private void UpdateView()
        {
            this.viewModels = new ObservableCollection<KolonneViewModel>(KolonneViewModel.CreateViewModels(this.kolonnen.GetAll()));
            this.window.kolonnenListView.ItemsSource = this.viewModels;
        }

        void KolonnenListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ValidateToolbarButtons();
        }

        private void ValidateToolbarButtons()
        {
            if (this.window.kolonnenListView.SelectedItems.Count > 0)
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
            this.OpenNewKolonneWindow();
        }

        void MainToolbarEditButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenEditKolonneWindowForSelected();
        }

        void MainToolbarDeleteButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DeleteSelectedKolonnen();
        }


        private void OpenEditKolonneWindowForSelected()
        {
            KolonneViewModel selected = this.GetFirstSelected();
            if (selected != null)
            {
                KolonneEditPresenter newKolPresenter = new KolonneEditPresenter(
                    new KolonneEditWindow(), this.kolonnen, this.mitarbeiter, this.repositories);
                newKolPresenter.EditKolonne(selected.GetModel());
                newKolPresenter.ShowDialog();
                if (newKolPresenter.GetDialogResult())
                {
                    selected.NotifyPropertyChanged();
                }   
            }
        }

        private void OpenNewKolonneWindow()
        {
            KolonneEditPresenter newProjPresenter = new KolonneEditPresenter(
                new KolonneEditWindow(), this.kolonnen, this.mitarbeiter, this.repositories);
            newProjPresenter.CreateNewKolonne();
            newProjPresenter.ShowDialog();
            if (newProjPresenter.GetDialogResult())
            {
                Kolonne newKolonne = newProjPresenter.GetKolonne();
                this.viewModels.Add(new KolonneViewModel(newKolonne));
            }
        }

        private void DeleteSelectedKolonnen()
        {
            List<KolonneViewModel> selection = this.GetSelection();
            if (selection.Count > 0)
            {
                KolonnenDbTable kolonnenTable = new KolonnenDbTable();
                foreach (var item in selection)
                {
                    Kolonne deletedKolonne = item.GetModel();

                    this.viewModels.Remove(item);
                    this.kolonnen.Remove(deletedKolonne);
                    kolonnenTable.Delete(deletedKolonne);
                }
            }
        }

        private List<KolonneViewModel> GetSelection()
        {
            List<KolonneViewModel> selection = new List<KolonneViewModel>();
            foreach (var item in this.window.kolonnenListView.SelectedItems)
                selection.Add((KolonneViewModel)item);

            return selection;
        }

        private KolonneViewModel GetFirstSelected()
        {
            KolonneViewModel selected = null;
            if (this.window.kolonnenListView.SelectedItems.Count > 0)
                selected = (KolonneViewModel)this.window.kolonnenListView.SelectedItem;
            return selected;
        }

        // EventHandler

        private void ListViewItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.OpenEditKolonneWindowForSelected();
        }

        /// <summary>
        /// Contextmenü, Neu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NewKolonneMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenNewKolonneWindow();
        }

        /// <summary>
        /// Contextmenü, Bearbeiten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EditKolonneMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenEditKolonneWindowForSelected();
        }

        /// <summary>
        /// Contextmenü, Löschen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteKolonneMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DeleteSelectedKolonnen();
        }

        void ProjekteListViewContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            bool isItemSelected = (this.window.kolonnenListView.SelectedItems.Count == 0);
            this.window.EditKolonneMenuItem.IsEnabled = !isItemSelected;
            //this.window.DeleteKolonneMenuItem.IsEnabled = !isItemSelected;
        }

        void BtnNewClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenNewKolonneWindow();
        }

        void BtnOkClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.window.Close();
        }


    }
}
