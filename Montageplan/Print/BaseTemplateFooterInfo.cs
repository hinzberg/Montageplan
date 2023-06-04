namespace Montageplan.Print
{
    public class BaseTemplateFooterInfo
    {
        // Header

        private object _footerLeftLower;
        private object _footerLeftUpper;
        private object _footerMiddleLower;
        private object _footerMiddleUpper;
        private object _footerRightLower;
        private object _footerRightUpper;
        private object _headerCenter;
        private object _headerLeft;
        private object _headerRight;

        public BaseTemplateFooterInfo()
        {
            this.HeaderLeft = "";
            this.HeaderCenter = "";
            this.HeaderRight = "";
            this.FooterLeftLower = "";
            this.FooterLeftUpper = "";
            this.FooterMiddleLower = "";
            this.FooterMiddleUpper = "";
            this.FooterRightLower = "";
            this.FooterRightUpper = "";

            this.FooterLeftMiddleLower = "";
            this.FooterLeftMiddleUpper = "";
            this.FooterRightMiddleLower = "";
            this.FooterRightMiddleUpper = "";

            this.HeaderLeftMiddleLower = "";
            this.HeaderLeftMiddleUpper = "";
            this.HeaderRightMiddleLower = "";
            this.HeaderRightMiddleUpper = "";

            this.ShowFooterCenterLogo = false;
            this.ShowHeaderWithGrayBackground = true;
        }

        public object HeaderLeft
        {
            get { return _headerLeft; }
            set { _headerLeft = value; }
        }

        public object HeaderCenter
        {
            get { return _headerCenter; }
            set { _headerCenter = value; }
        }

        public object HeaderRight
        {
            get { return _headerRight; }
            set { _headerRight = value; }
        }

        // Footer

        public object FooterLeftLower
        {
            get { return _footerLeftLower; }
            set { _footerLeftLower = value; }
        }

        public object FooterLeftUpper
        {
            get { return _footerLeftUpper; }
            set { _footerLeftUpper = value; }
        }

        public object FooterMiddleLower
        {
            get { return _footerMiddleLower; }
            set { _footerMiddleLower = value; }
        }

        public object FooterMiddleUpper
        {
            get { return _footerMiddleUpper; }
            set { _footerMiddleUpper = value; }
        }

        public object FooterRightLower
        {
            get { return _footerRightLower; }
            set { _footerRightLower = value; }
        }

        public object FooterRightUpper
        {
            get { return _footerRightUpper; }
            set { _footerRightUpper = value; }
        }

        public object FooterLeftMiddleUpper { get; set; }
        public object FooterLeftMiddleLower { get; set; }
        public object FooterRightMiddleUpper { get; set; }
        public object FooterRightMiddleLower { get; set; }

        public object HeaderLeftMiddleUpper { get; set; }
        public object HeaderLeftMiddleLower { get; set; }
        public object HeaderRightMiddleUpper { get; set; }
        public object HeaderRightMiddleLower { get; set; }

        public bool ShowFooterCenterLogo { get; set; }
        public bool ShowHeaderWithGrayBackground { get; set; }

        // Ein Wrapper für leichtere Zuweisungen
        public int PageNumber
        {
            set { this.FooterRightLower = value; }
        }

        public static BaseTemplateFooterInfo Empty()
        {
            return new BaseTemplateFooterInfo();
        }
    }
}