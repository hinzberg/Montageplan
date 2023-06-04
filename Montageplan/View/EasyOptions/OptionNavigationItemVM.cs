using System.ComponentModel;
using Montageplan.EasyOptions;

namespace PolePosition.EasyOptions
{
    public class OptionNavigationItemVM : INotifyPropertyChanged
    {
        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public OptionNavigationItemVM(IOptionPanel optionPanel)
        {
            this.PanelName = optionPanel.PanelName;
            this.IsEnabled = true;
        }

        public string PanelName { get; set; }
        public bool IsEnabled { get; set; }


        public string GetPanelNameForSort()
        {
            string sortName = this.PanelName;
            if (this.PanelName == "Über CarObserver")
            {
                // Als letztes anzeigen
                sortName = "zzz";
            }
            return sortName;
        }

        public void RefreshIsEnabled()
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs("IsEnabled"));
        }

    }
}
