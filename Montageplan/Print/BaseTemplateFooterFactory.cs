using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Print
{
    public class BaseTemplateFooterFactory
    {
        public BaseTemplateFooterInfo GetFooterInfoWithDate()
        {
            BaseTemplateFooterInfo info = new BaseTemplateFooterInfo
            {
                HeaderRight = this.GetFormattedDate(),
                ShowFooterCenterLogo = false
            };
            return info;
        }

        public BaseTemplateFooterInfo GetEmpty()
        {
            return new BaseTemplateFooterInfo();
        }

        private string GetFormattedDate()
        {
            return DateTime.Now.ToLongDateString();
        }
    }
}
