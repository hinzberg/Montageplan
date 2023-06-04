using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Montageplan.Model;
using Montageplan.Print.Container;

namespace Montageplan.Print.Pages
{
    public partial class ProjektDetailsFlowTemplate : UserControl
    {
        private readonly ProjektDetailsContainer container;
        private List<ProjektDetailsVM> viewModels; 

        public ProjektDetailsFlowTemplate(ProjektDetailsContainer cont)
        {
            this.container = cont;
            InitializeComponent();
            this.CreateViewModels(container.Details);
            this.kolonnenListBox.ItemsSource = this.viewModels;
        }

        private void CreateViewModels(IEnumerable<ProjektDetails> details )
        {
            this.viewModels = new List<ProjektDetailsVM>();

            foreach (ProjektDetails detail in details)
            {
                this.viewModels.Add(new ProjektDetailsVM(detail));
            }
        }



    }
}
