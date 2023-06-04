using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using Montageplan.Model;
using Montageplan.Presenter;
using Montageplan.View.Windows;

namespace Montageplan.Print
{
    /// <summary>
    /// Diese Klasse kapselt alle Druckfunktionen inklusive aller 
    /// Auswahlfenster und Vorschau.
    /// </summary>
    public class MainPrinter
    {
        private readonly MainWindow window;
        private readonly RepositoryContainer repositories;

        public MainPrinter(MainWindow win, RepositoryContainer repo)
        {
            this.window = win;
            this.repositories = repo;
        }

        /// <summary>
        /// Tagesbericht, nach Auswahlfenster.
        /// Zunächst nur mit Demoseiten.
        /// </summary>
        public void CreateTimespanReport()
        {
            TimespanSelectWindow timespanSelectWindow = new TimespanSelectWindow();
            timespanSelectWindow.Owner = this.window;

            TimespanSelectPresenter timespanSelectPresenter = new TimespanSelectPresenter(timespanSelectWindow);
            timespanSelectPresenter.StartDatum = AppConfig.Settings.StartDatumDruck;
            timespanSelectPresenter.EndeDatum = AppConfig.Settings.EndeDatumDruck;
            timespanSelectPresenter.ShowDialog();

            bool? result = timespanSelectPresenter.GetDialogResult();
            if (result == true)
            {
                GenericWaitWindow waitWindow = this.CreateWaitWindow();
                waitWindow.Owner = this.window;
                waitWindow.Show();

                AppConfig.Settings.StartDatumDruck = timespanSelectPresenter.StartDatum;
                AppConfig.Settings.EndeDatumDruck = timespanSelectPresenter.EndeDatum;
                AppConfig.Save();

                FlowCanvasCreator flowCanvasCreator = new FlowCanvasCreator(this.repositories);
                List<Canvas> canvases 
                    = flowCanvasCreator.CreateProjektReport(AppConfig.Settings.StartDatumDruck, AppConfig.Settings.EndeDatumDruck);

                if (canvases.Any())
                {
                    FlowDocumentCreator documentCreator = new FlowDocumentCreator();
                    FlowDocument doc = documentCreator.Create(canvases);

                    FlowDocumentPreviewContainer previewContainer = new FlowDocumentPreviewContainer();
                    previewContainer.Document = doc;
                    previewContainer.DisplayText = "Montageplan";
                    previewContainer.FilenameWithoutExtention = previewContainer.DisplayText;

                    waitWindow.Close();

                    FlowDocumentPreviewWindow flowDocumentPreviewWindow
                        = new FlowDocumentPreviewWindow(new[] { previewContainer });
                    flowDocumentPreviewWindow.Owner = this.window;
                    flowDocumentPreviewWindow.ShowDialog();
                }
                else
                {
                    waitWindow.Close();
                    string m = "Für den gewählten Zeitraum gibt es keine Projekte.";
                    string a = "Bitte wählen Sie einen anderen Zeitraum.";
                    MessageBoxHelper.ShowOkBoxWindow(m,a,this.window);
                }
            }
        }

        private GenericWaitWindow CreateWaitWindow()
        {
            return new GenericWaitWindow("Vorschau wird generiert", "Bitte haben Sie einen Moment Geduld");
        }
    }
}