using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Print.Container
{
    public class DemoPageContainer : ContentContainerBase
    {
        public string DemoDescription { get; set; }

        public DemoPageContainer()
        {
            this.DemoDescription = "";
        }
    }
}
