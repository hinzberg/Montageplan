using Montageplan.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.ViewModel
{
    public class ProjektViewModelEventArgs : EventArgs
    {
        public ProjektViewModelEventArgs(ProjektViewModel projekViewModel)
        {
            this.ProjektViewModel = projekViewModel;
        }

        public ProjektViewModel ProjektViewModel { get; set; }

    }
}
