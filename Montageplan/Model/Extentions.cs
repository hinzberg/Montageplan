using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public static class Extentions
    {
        public static bool CaseInsensitiveContains(this string s, string value)
        {
            string dummy1 = s.ToLower();
            string dummy2 = value.ToLower();
            return dummy1.Contains(dummy2);
        }

        public static bool CaseInsensitiveMatch(this string s, string value)
        {
            string dummy1 = s.ToLower();
            string dummy2 = value.ToLower();
            return dummy1 == dummy2;
        }
    }
}
