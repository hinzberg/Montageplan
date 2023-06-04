using Montageplan.Model;
using Montageplan.View;
using Montageplan.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Montageplan.Presenter
{
    public class ProjectsPresenter
    {
        private readonly ProjectsView view;
        private ObservableCollection<ProjektViewModel> viewModels;
        private ListCollectionView collectionView;

        public ProjectsPresenter(ProjectsView view)
        {
            this.view = view;
            this.viewModels = null;
            this.collectionView = null;
            this.ProjectSelectionChanged = null;

            ListBoxDragHelper dragHelper = new ListBoxDragHelper(view.projectsListBox, this.GetProjektAdornerTemplate());
            ListBoxDragHelper.AddHelper(dragHelper);
        }

        public Action<ProjektViewModelEventArgs> ProjectSelectionChanged { get; set; }

        public ProjektViewModel GetSelected()
        {
            return (this.view.projectsListBox.SelectedItem as ProjektViewModel);
        }

        public void SetProjects(IEnumerable<Projekt> projects)
        {
            this.viewModels = new ObservableCollection<ProjektViewModel>(ProjektViewModel.CreateViewModels(projects));
        }

        public void UpdateView()
        {
            if (this.viewModels != null)
            {
                // 'CurrentChanged' Zuweisung bereinigen
                if(this.collectionView != null)
                    this.collectionView.CurrentChanged -= collectionView_CurrentChanged;

                this.collectionView = new ListCollectionView(this.viewModels);
                this.view.projectsListBox.ItemsSource = this.collectionView;
                this.view.projectsListBox.SelectedItem = null;
                this.collectionView.CurrentChanged += collectionView_CurrentChanged;
            }
        }

        private DataTemplate GetProjektAdornerTemplate()
        {
            DataTemplate template = null;
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri(@"..\View\ItemTemplates\DragAdornerTemplates.xaml", UriKind.Relative);
            template = dictionary["projektAdornerTemplate"] as DataTemplate;
            return template;
        }

        void collectionView_CurrentChanged(object sender, EventArgs e)
        {
            if (this.ProjectSelectionChanged != null)
                this.ProjectSelectionChanged(new ProjektViewModelEventArgs(this.GetSelected()));
        }

    }
}
