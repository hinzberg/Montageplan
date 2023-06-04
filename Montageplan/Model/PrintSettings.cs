namespace Montageplan.Model
{
    public class PrintSettings
    {
        public PrintSettings()
        {
            this.PageOffsetX = 10;
            this.PageOffsetY = 10;
        }

        public int PageOffsetX { get; set; }
        public int PageOffsetY { get; set; }
    }
}