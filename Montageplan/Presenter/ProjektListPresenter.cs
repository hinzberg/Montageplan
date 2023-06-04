using Montageplan.Model;
using Montageplan.Model.DAL.Database;
using Montageplan.ViewModel;
using Montageplan.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Windows.Data;
using System.ComponentModel;

namespace Montageplan.Presenter
{
    public class ProjektListPresenter
    {
        private readonly ProjektListWindow window;
        private readonly ProjektRepository projekte;
        private ObservableCollection<ProjektViewModel> viewModels;

        public ProjektListPresenter(ProjektListWindow window, ProjektRepository projekte)
        {
            this.window = window;
            this.projekte = projekte;
            this.viewModels = new ObservableCollection<ProjektViewModel>();
            (this.window.projekteListView.Items as CollectionView).SortDescriptions.Add(new SortDescription("DateToSort", ListSortDirection.Ascending));

            this.window.mainToolbar.NewButtonText = "Neues Projekt";
            this.window.mainToolbar.NewButtonClicked += MainToolbarNewButtonClicked;
            this.window.mainToolbar.DeleteButtonClicked += MainToolbarDeleteButtonClicked;
            this.window.mainToolbar.EditButtonClicked += MainToolbarEditButtonClicked;

            this.window.btnOk.Click += BtnOkClick;
            this.window.NewProjektMenuItem.Click += NewProjektMenuItemClick;
            this.window.EditProjektMenuItem.Click += EditProjektMenuItemClick;
            this.window.DeleteProjektMenuItem.Click += DeleteProjektMenuItemClick;
            this.window.projekteListView.ContextMenuOpening += ProjekteListViewContextMenuOpening;
            this.window.projekteListView.SelectionChanged += ProjekteListViewSelectionChanged;
            this.window.projekteListView.AddHandler(
                ListViewItem.MouseDoubleClickEvent, new MouseButtonEventHandler(this.ListViewItemMouseDoubleClick), true);

            this.ValidateToolbarButtons();
        }

      


        public void ShowDialog()
        {
            this.window.DeleteProjektMenuItem.IsEnabled = false;
            this.window.mainToolbar.deleteButton.IsEnabled = false;

            this.UpdateView();
            this.window.ShowDialog();
        }

        private void UpdateView()
        {
            this.viewModels = new ObservableCollection<ProjektViewModel>(ProjektViewModel.CreateViewModels(this.projekte.GetTemplateProjects()));
            this.window.projekteListView.ItemsSource = this.viewModels;
        }

        void ProjekteListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ValidateToolbarButtons();
        }

        private void ValidateToolbarButtons()
        {
            if (this.window.projekteListView.SelectedItems.Count > 0)
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
            this.OpenNewProjektWindow();
        }

        void MainToolbarEditButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenEditProjektWindowForSelected();
        }

        void MainToolbarDeleteButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DeleteSelectedProjekte();
        }



        private void OpenEditProjektWindowForSelected()
        {
            ProjektViewModel selected = this.GetFirstSelected();
            if (selected != null)
            {
                ProjektEditPresenter newProjPresenter = new ProjektEditPresenter(new ProjektEditWindow());
                newProjPresenter.EditProjekt(selected.GetModel());
                newProjPresenter.ShowDialog();
                if (newProjPresenter.GetDialogResult())
                {
                    selected.NotifyPropertyChanged();
                    ProjekteDbTable projekteTable = new ProjekteDbTable();
                    projekteTable.Update(selected.GetModel());
                }
            }
        }

        private void OpenNewProjektWindow()
        {
            ProjektEditPresenter newProjPresenter = new ProjektEditPresenter(new ProjektEditWindow());
            newProjPresenter.CreateNewProjekt();
            newProjPresenter.ShowDialog();
            if (newProjPresenter.GetDialogResult())
            {
                Projekt newProjekt = newProjPresenter.GetProjekt();

                this.projekte.Add(newProjekt);
                this.viewModels.Add(new ProjektViewModel(newProjekt));
                ProjekteDbTable projekteTable = new ProjekteDbTable();
                projekteTable.Insert(newProjekt);
            }
        }

        private void DeleteSelectedProjekte()
        {
            List<ProjektViewModel> selection = this.GetSelection();
            if (selection.Count > 0)
            {
                ProjekteDbTable projekteTable = new ProjekteDbTable();
                foreach (var item in selection)
                {
                    Projekt deletedProjekt = item.GetModel();

                    this.viewModels.Remove(item);
                    this.projekte.Remove(deletedProjekt);
                    projekteTable.Delete(deletedProjekt);
                }
            }
        }

        private List<ProjektViewModel> GetSelection()
        {
            List<ProjektViewModel> selection = new List<ProjektViewModel>();
            foreach (var item in this.window.projekteListView.SelectedItems)
                selection.Add((ProjektViewModel)item);

            return selection;
        }

        private ProjektViewModel GetFirstSelected()
        {
            ProjektViewModel selected = null;
            if (this.window.projekteListView.SelectedItems.Count > 0)
                selected = (ProjektViewModel)this.window.projekteListView.SelectedItem;
            return selected;
        }

        // EventHandler //

        private void ListViewItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.OpenEditProjektWindowForSelected();
        }

        /// <summary>
        /// Contextmenü, Neu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NewProjektMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenNewProjektWindow();
        }

        /// <summary>
        /// Contextmenü, Bearbeiten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EditProjektMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenEditProjektWindowForSelected();
        }

        /// <summary>
        /// Contextmenü, Löschen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteProjektMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DeleteSelectedProjekte();
        }

        void ProjekteListViewContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            bool isItemSelected = (this.window.projekteListView.SelectedItems.Count == 0);
            this.window.EditProjektMenuItem.IsEnabled = !isItemSelected;
            //this.window.DeleteProjektMenuItem.IsEnabled = !isItemSelected;
        }

        void BtnOkClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.window.Close();
        }

    }
}
