using System.Windows;
using System.Windows.Controls;
using Montageplan.Classes;
using Montageplan.Model;

namespace Montageplan.Windows
{
    public partial class KolonneListWindow : Window
    {
        public KolonneListWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }

        public MenuItem NewKolonneMenuItem
        {
            get { return (MenuItem) SubElementGetter.Get("newKolonneMenuItem", this.kolonnenListView); }
        }

        public MenuItem EditKolonneMenuItem
        {
            get { return (MenuItem) SubElementGetter.Get("editKolonneMenuItem", this.kolonnenListView); }
        }

        public MenuItem DeleteKolonneMenuItem
        {
            get { return (MenuItem) SubElementGetter.Get("deleteKolonneMenuItem", this.kolonnenListView); }
        }
    }
}