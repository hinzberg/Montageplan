using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class MetaDataItem
    {
        public MetaDataItem()
        {
            this.Key = "";
            this.Value = "";
        }

        public string Key { get; set; }
        public string Value { get; set; }

    }
}
