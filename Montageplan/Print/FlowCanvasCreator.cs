using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Montageplan.Model;
using Montageplan.Print.Container;

namespace Montageplan.Print
{
    /// <summary>
    /// Klasse erzeugt Canvas-Listen aus den sich Druck-Berichte
    /// zusammensetzten lassen.
    /// </summary>
    public class FlowCanvasCreator
    {
        private readonly List<Canvas> canvasList;
        private readonly FlowContentFactory flowContentFactory;
        private readonly BaseTemplateFooterFactory footerFactory;
        private readonly RepositoryContainer repositories;

        public FlowCanvasCreator(RepositoryContainer repo)
        {
            this.repositories = repo;
            this.canvasList = new List<Canvas>();
            this.flowContentFactory = new FlowContentFactory();
            this.footerFactory = new BaseTemplateFooterFactory();
        }

        public int PageNumber
        {
            get { return this.canvasList.Count + 1; }
        }

        public List<Canvas> CreateDemoReport()
        {
            this.CreateDemoPage();
            this.CreateDemoPage();
            this.CreateDemoPage();
            return this.canvasList;
        }

        private void CreateDemoPage()
        {
            DemoPageContainer demoPageContainer = new DemoPageContainer();
            demoPageContainer.DemoDescription = "Das ist ein Test";
            demoPageContainer.FooterInfo = this.footerFactory.GetFooterInfoWithDate();
            demoPageContainer.FooterInfo.HeaderLeft = "Montageplan";
            demoPageContainer.FooterInfo.PageNumber = this.PageNumber;

            Canvas canvas = this.flowContentFactory.CreateDemoPageContent(demoPageContainer);
            this.canvasList.Add(canvas);
        }

        //-Projekt Report-----------------------------------------------------------------------------------

        /// <summary>
        /// Erzeugt einen Bericht mit den Projekten eines Tages.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<Canvas> CreateProjektReport(DateTime startDate, DateTime endDate)
        {
            if (startDate <= endDate)
            {
                DateTime singleDate = startDate;
                do
                {
                    this.CreateProjektReportSingleDay(singleDate);
                    singleDate = singleDate.AddDays(1);
                } while (singleDate <= endDate);
            }
            return this.canvasList;
        }

        /// <summary>
        /// Report für einen Tag.
        /// </summary>
        /// <param name="date"></param>
        private void CreateProjektReportSingleDay(DateTime date)
        {
            if (this.repositories.Days.Contains(date))
            {
                CalendarDay createdDay = this.repositories.Days.Get(date);
                List<ProjektDetails> projekte = createdDay.GetExistingProjektDetails();
                if (projekte.Count > 0)
                {
                    int printedProjektCounter = 0;
                    List<ProjektDetails> projekteForPage = new List<ProjektDetails>();
                    foreach (ProjektDetails details in projekte)
                    {
                        projekteForPage.Add(details);
                        printedProjektCounter++;
                        // Es passen immer nur 5 Projekte auf ein Blatt.
                        // Oder bereits alle anderen gedruckt.
                        if (projekteForPage.Count == 5 || printedProjektCounter == projekte.Count)
                        {
                            // Seite voll.
                            this.CreateProjektReportPage(projekteForPage,date);
                            // Liste jetzt leeren für nächste Seite.
                            projekteForPage.Clear();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Erzeugt ein Canvas mit maximal fünf Projekten.
        /// </summary>
        /// <param name="projekte"></param>
        /// <param name="date"></param>
        private void CreateProjektReportPage(List<ProjektDetails> projekte, DateTime date)
        {
            ProjektDetailsContainer container = new ProjektDetailsContainer();
            container.Details = projekte;
            container.FooterInfo = this.footerFactory.GetEmpty();
            container.FooterInfo.HeaderRight = date.ToLongDateString();
            container.FooterInfo.HeaderLeft = "Projektplanung";
            container.FooterInfo.PageNumber = this.PageNumber;
            Canvas canvas = this.flowContentFactory.CreateSingleDayPageContent(container);
            this.canvasList.Add(canvas);
        }

        //-----------------------------------------------------------------------------------------------------
    }
}