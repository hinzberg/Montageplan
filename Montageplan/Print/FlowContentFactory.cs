using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Montageplan.Print.Container;
using Montageplan.Print.Pages;

namespace Montageplan.Print
{
    /// <summary>
    /// Die ContentFactory erzeugt den tatsächlichen Inhalt der Dokumente.
    /// Eine Seite entspricht dabei immer einem Canvas.
    /// </summary>
    public class FlowContentFactory
    {
        private const double PAGE_HEIGHT = 728;
        private const double PAGE_WIDTH = 1024;
        private const double BORDEROFFSET = 0;

        public Canvas CreateDemoPageContent(DemoPageContainer pageContainer)
        {
            DemoPageFlowTemplate demoPageFlowTemplate = new DemoPageFlowTemplate(pageContainer);
            Canvas page = this.CombineWithBaseTemplate(demoPageFlowTemplate, pageContainer.FooterInfo);
            return page;
        }

        public Canvas CreateSingleDayPageContent(ProjektDetailsContainer pageContainer)
        {
            ProjektDetailsFlowTemplate flowTemplate = new ProjektDetailsFlowTemplate(pageContainer);
            Canvas page = this.CombineWithBaseTemplate(flowTemplate, pageContainer.FooterInfo);
            return page;
        }

        private Canvas CombineWithBaseTemplate(UserControl control, BaseTemplateFooterInfo footerInfo)
        {
            Canvas canvas = new Canvas();
            canvas.Width = PAGE_WIDTH;
            canvas.Height = PAGE_HEIGHT;
            Point origin = new Point(0, 0);

            BaseFlowPageTemplate baseTemplate = new BaseFlowPageTemplate();
            this.SetFooterInfo(baseTemplate, footerInfo);

            double contentHeight = PAGE_HEIGHT - (2 * BORDEROFFSET) - baseTemplate.TopRow.Height.Value -
                                   baseTemplate.BottomRow.Height.Value;
            Size contentSize = new Size(baseTemplate.Width, contentHeight);
            Grid.SetRow(control, 1);

            control.Measure(contentSize);
            control.Arrange(new Rect(origin, contentSize));
            control.UpdateLayout();

            baseTemplate.MainGrid.Children.Add(control);

            canvas.Children.Add(baseTemplate);
            return canvas;
        }

        private void SetFooterInfo(BaseFlowPageTemplate baseTemplate, BaseTemplateFooterInfo footerInfo)
        {
            if (!footerInfo.ShowHeaderWithGrayBackground)
            {
                baseTemplate.HeaderStack.Background = Brushes.Transparent;
            }
            baseTemplate.headerLeftLabel.Content = footerInfo.HeaderLeft;
            baseTemplate.headerCenterLabel.Content = footerInfo.HeaderCenter;
            baseTemplate.headerRightLabel.Content = footerInfo.HeaderRight;
            baseTemplate.footerLeftLowerLabel.Content = footerInfo.FooterLeftLower;
            baseTemplate.footerLeftUpperLabel.Content = footerInfo.FooterLeftUpper;
            baseTemplate.footerMiddleLowerLabel.Content = footerInfo.FooterMiddleLower;
            baseTemplate.footerMiddleUpperLabel.Content = footerInfo.FooterMiddleUpper;
            baseTemplate.footerRightLowerLabel.Content = footerInfo.FooterRightLower;
            baseTemplate.footerRightUpperLabel.Content = footerInfo.FooterRightUpper;
            baseTemplate.footerLeftMiddleLowerLabel.Content = footerInfo.FooterLeftMiddleLower;
            baseTemplate.footerLeftMiddleUpperLabel.Content = footerInfo.FooterLeftMiddleUpper;
            baseTemplate.footerRightMiddleLowerLabel.Content = footerInfo.FooterRightMiddleLower;
            baseTemplate.footerRightMiddleUpperLabel.Content = footerInfo.FooterRightMiddleUpper;
            baseTemplate.headerLeftMiddleLowerLabel.Content = footerInfo.HeaderLeftMiddleLower;
            baseTemplate.headerLeftMiddleUpperLabel.Content = footerInfo.HeaderLeftMiddleUpper;
            baseTemplate.headerRightMiddleLowerLabel.Content = footerInfo.HeaderRightMiddleLower;
            baseTemplate.headerRightMiddleUpperLabel.Content = footerInfo.HeaderRightMiddleUpper;
            baseTemplate.ImageFooterCenter.Visibility = Visibility.Visible;
        }
    }
}