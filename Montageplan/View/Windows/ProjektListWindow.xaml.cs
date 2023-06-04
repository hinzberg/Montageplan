using System.Windows;
using System.Windows.Controls;
using Montageplan.Classes;
using Montageplan.Model;

namespace Montageplan.Windows
{
    public partial class ProjektListWindow : Window
    {
        public ProjektListWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }

        public MenuItem NewProjektMenuItem
        {
            get { return (MenuItem) SubElementGetter.Get("newProjektMenuItem", this.projekteListView); }
        }

        public MenuItem EditProjektMenuItem
        {
            get { return (MenuItem) SubElementGetter.Get("editProjektMenuItem", this.projekteListView); }
        }

        public MenuItem DeleteProjektMenuItem
        {
            get { return (MenuItem) SubElementGetter.Get("deleteProjektMenuItem", this.projekteListView); }
        }
    }
}