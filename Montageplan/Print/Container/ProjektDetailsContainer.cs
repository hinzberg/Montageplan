using System.Collections.Generic;
using Montageplan.Model;

namespace Montageplan.Print.Container
{
    public class ProjektDetailsContainer : ContentContainerBase
    {
        public ProjektDetailsContainer()
        {
        }

        public List<ProjektDetails> Details { get; set; }
    }
}