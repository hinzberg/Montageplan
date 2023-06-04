namespace Montageplan.Print.Container
{
    public class ContentContainerBase
    {
        public ContentContainerBase()
        {
            this.InitialPageNumber = 0;
        }

        public BaseTemplateFooterInfo FooterInfo { get; set; }
        public int InitialPageNumber { get; set; }
    }
}