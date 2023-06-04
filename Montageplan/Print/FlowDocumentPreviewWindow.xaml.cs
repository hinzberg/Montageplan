using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using Montageplan.Classes;
using Montageplan.Model;

namespace Montageplan.Print
{
    /// <summary>
    /// Vorschaufenster für FlowDocumente.
    /// Von hier kann man dann wirklich drucken.
    /// </summary>
    public partial class FlowDocumentPreviewWindow : Window
    {
        private readonly DispatcherTimer dTimer;
        private readonly List<FlowDocumentPreviewContainer> previewContainers;
        private FlowDocument selectedDocument;
        private int lastMaxPages;

        public FlowDocumentPreviewWindow(IEnumerable<FlowDocumentPreviewContainer> containers)
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
            this.Closing += FlowDocumentPreviewWindowClosing;
            this.IsPrintChosen = false;
            this.previewContainers = new List<FlowDocumentPreviewContainer>(containers);

            List<string> names = new List<string>();
            foreach (FlowDocumentPreviewContainer container in containers)
            {
                names.Add(container.DisplayText);
            }

            this.ButtonPrint.IsEnabled = false;
            this.lastMaxPages = 1;

            this.comboReports.ItemsSource = names;
            this.comboReports.SelectedIndex = 0;
            if (previewContainers.Count == 1)
                this.comboReports.Visibility = Visibility.Hidden;

            dTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dTimer.Interval = TimeSpan.FromSeconds(1);
            dTimer.Tick += DTimerTick;
            dTimer.Start();
        }

        public bool IsPrintChosen { get; set; }

        private void DTimerTick(object sender, EventArgs e)
        {
            RefreshPageNumberDisplay();
            this.ValidateNavigationButtons();
        }

        private void ComboReports_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.comboReports.SelectedIndex;
            this.selectedDocument = this.previewContainers[index].Document;
            this.DocumentViewer.Document = selectedDocument;
            this.DocumentViewer.FirstPage();

            if (dTimer != null)
                dTimer.Start();
        }

        /// <summary>
        /// Button für Druck
        /// Hier wir es aufs Papier gebracht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonPrintClick(object sender, RoutedEventArgs e)
        {
            const double PAGE_HEIGHT = 728;
            const double PAGE_WIDTH = 1024;

            double edgeOffsetX = AppConfig.Settings.Print.PageOffsetX;
            double edgeOffsetY = AppConfig.Settings.Print.PageOffsetY;

            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
                if (capabilities.PageImageableArea != null)
                {
                    double width = capabilities.PageImageableArea.OriginWidth + edgeOffsetX;
                    double height = capabilities.PageImageableArea.OriginHeight + edgeOffsetY;
                    Thickness padding = new Thickness(width, height, 0, 0);

                    foreach (PageMediaSize mediaSize in capabilities.PageMediaSizeCapability)
                    {
                        if (mediaSize.PageMediaSizeName == PageMediaSizeName.ISOA4)
                        {
                            if (mediaSize.Width != null && mediaSize.Height != null)
                            {
                                width = (mediaSize.Width.Value - capabilities.PageImageableArea.ExtentWidth) / 2;
                                height = (mediaSize.Height.Value - capabilities.PageImageableArea.ExtentHeight) / 2;
                                width += edgeOffsetX;
                                height += edgeOffsetY;
                                padding = new Thickness(width, height, 0, 0);
                            }
                            break;
                        }
                    }

                    Thickness oldPadding = this.selectedDocument.PagePadding;

                    this.selectedDocument.PagePadding = padding;
                    var paginator = ((IDocumentPaginatorSource)this.selectedDocument).DocumentPaginator;

                    paginator.PageSize = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                    printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
                    printDialog.PrintDocument(paginator, "Montageplan");

                    this.selectedDocument.PagePadding = oldPadding;
                }
            }

            IsPrintChosen = true;
        }

        private void RefreshPageNumberDisplay()
        {
            int page = this.DocumentViewer.MasterPageNumber;
            int maxPage = this.DocumentViewer.PageCount;

            // Passiert beim init
            if (page == 0)
                page = 1;

            if (maxPage == 0)
                maxPage = 1;

            this.CurrentPageNumber.Text = page.ToString("N0");
            this.MaximumPageNumber.Content = "von " + maxPage.ToString("N0");

            this.ValidateActionButtons();
        }

        private void ValidateActionButtons()
        {
            int maxPage = this.DocumentViewer.PageCount;
            if (this.lastMaxPages == maxPage)
            {
                this.ButtonPrint.IsEnabled = true;
                //this.btnPdf.IsEnabled = true;
                //this.btnMail.IsEnabled = true;
            }
            this.lastMaxPages = maxPage;
        }


        private void ValidateNavigationButtons()
        {
            int current = this.DocumentViewer.MasterPageNumber;
            int max = this.DocumentViewer.Document.DocumentPaginator.PageCount;

            if (current == max)
            {
                ButtonLastPage.IsEnabled = false;
                ButtonNextPage.IsEnabled = false;
            }
            else
            {
                ButtonLastPage.IsEnabled = true;
                ButtonNextPage.IsEnabled = true;
            }

            if (current > 1)
            {
                ButtonFirstPage.IsEnabled = true;
                ButtonPreviousPage.IsEnabled = true;
            }
            else
            {
                ButtonFirstPage.IsEnabled = false;
                ButtonPreviousPage.IsEnabled = false;
            }

            int index = this.comboReports.SelectedIndex;
            FlowDocumentPreviewContainer selectedContainer = this.previewContainers[index];
        }

        private void ButtonFirstPageClick(object sender, RoutedEventArgs e)
        {
            this.DocumentViewer.FirstPage();
            this.RefreshPageNumberDisplay();
            this.ValidateNavigationButtons();
        }

        private void ButtonLastPageClick(object sender, RoutedEventArgs e)
        {
            this.DocumentViewer.LastPage();
            this.RefreshPageNumberDisplay();
            this.ValidateNavigationButtons();
        }

        private void ButtonNextPageClick(object sender, RoutedEventArgs e)
        {
            this.DocumentViewer.NextPage();
            this.RefreshPageNumberDisplay();
            this.ValidateNavigationButtons();
        }

        private void ButtonPreviousPageClick(object sender, RoutedEventArgs e)
        {
            this.DocumentViewer.PreviousPage();
            this.RefreshPageNumberDisplay();
            this.ValidateNavigationButtons();
        }

        private void CurrentPageNumberKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CurrentPageNumberLostFocus(sender, e);
                CurrentPageNumber.SelectAll();
            }
        }

        private void CurrentPageNumberLostFocus(object sender, RoutedEventArgs e)
        {
            int i = 0;
            if (int.TryParse(CurrentPageNumber.Text, out i))
            {
                if (i >= 1 && i <= this.DocumentViewer.PageCount)
                {
                    this.DocumentViewer.GoToPage(i);
                    this.ValidateNavigationButtons();
                }
            }
            CurrentPageNumber.SelectAll();
        }

        void FlowDocumentPreviewWindowClosing(object sender, CancelEventArgs e)
        {
            dTimer.Stop();
        }
    }
}