using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace Montageplan.Print
{
    public class FlowDocumentPreviewContainer
    {
        /// <summary>
        /// Diese Container werden an die Vorschau übergeben
        /// </summary>
        /// <param name="flowDoc">Das Flowdocument</param>
        /// <param name="displayText"> </param>
        /// <param name="fileWithoutExtent"> </param>
        public FlowDocumentPreviewContainer(FlowDocument flowDoc, string displayText, string fileWithoutExtent)
        {
            this.Document = flowDoc;
            this.DisplayText = displayText;
            this.FilenameWithoutExtention = fileWithoutExtent;
        }

        public FlowDocumentPreviewContainer()
        {
            this.Document = new FlowDocument();
            this.FilenameWithoutExtention = "";
            this.DisplayText = "";
        }

        public FlowDocument Document { get; set; }
        public string FilenameWithoutExtention { get; set; }
        public string DisplayText { get; set; }
    }
}
