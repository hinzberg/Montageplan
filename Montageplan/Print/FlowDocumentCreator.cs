using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Montageplan.Print
{
    /// <summary>
    /// Diese Klasse erzeugt aus einer Liste von Canvases ein FlowDocument.
    /// </summary>
    public class FlowDocumentCreator
    {
        private const double PAGE_HEIGHT = 728;
        private const double PAGE_WIDTH = 1024;

        private FlowDocument flowDoc;

        public FlowDocumentCreator()
        {
            this.flowDoc = null;
        }

        public FlowDocument Create(List<Canvas> cvList)
        {
            this.flowDoc = new FlowDocument();
            flowDoc.PageHeight = PAGE_HEIGHT;
            flowDoc.PageWidth = PAGE_WIDTH + 10;
            flowDoc.PagePadding = new Thickness(5);

            foreach (Canvas cv in cvList)
            {
                Section sec = new Section();
                Paragraph par = new Paragraph();
                Canvas cv1 = cv;
                par.Inlines.Add(cv1);
                sec.Blocks.Add(par);
                flowDoc.Blocks.Add(sec);
            }

            return flowDoc;
        }
    }
}