using System.Windows.Controls;
using Montageplan.Print.Container;

namespace Montageplan.Print.Pages
{
    /// <summary>
    /// Interaktionslogik für DemoPageFlowTemplate.xaml
    /// </summary>
    public partial class DemoPageFlowTemplate : UserControl
    {
        private readonly DemoPageContainer container;

        public DemoPageFlowTemplate(DemoPageContainer cont)
        {
            this.container = cont;
            InitializeComponent();
        }

        public string Description
        {
            get { return this.container.DemoDescription; }
        }
    }
}