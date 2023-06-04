using System.Windows;
using System.Windows.Controls;
using Montageplan.Classes;
using Montageplan.Model;

namespace Montageplan.Windows
{
    public partial class MitarbeiterListWindow : Window
    {
        public MitarbeiterListWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }

        public MenuItem NewMitarbeiterMenuItem
        {
            get { return (MenuItem) SubElementGetter.Get("newMitarbeiterMenuItem", this.mitarbeiterListView); }
        }

        public MenuItem EditMitarbeiterMenuItem
        {
            get { return (MenuItem) SubElementGetter.Get("editMitarbeiterMenuItem", this.mitarbeiterListView); }
        }

        public MenuItem DeleteMitarbeitertMenuItem
        {
            get { return (MenuItem) SubElementGetter.Get("deleteMitarbeiterMenuItem", this.mitarbeiterListView); }
        }
    }
}