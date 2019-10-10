using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReX = System.Text.RegularExpressions.Regex;

namespace APP.Commanding
{
    static class H
    {
        /// <summary>
        /// This will remove the date_version-indicator like  _001 or _075
        /// </summary>
        public static System.Text.RegularExpressions.Regex RexIterator = new ReX(@"_\d{3}$", System.Text.RegularExpressions.RegexOptions.Compiled);
        /// <summary> This gets these dates : 20180220 </summary>
        public static System.Text.RegularExpressions.Regex RexDat0 = new ReX(@"(20|19)\d{2}(0[1-9]|1[0-2])(0[1-9]|[1-2]\d|3[0-2])");
        /// <summary> this gets these dates : 2018-02-20 or 2018_02_20</summary>
        public static System.Text.RegularExpressions.Regex RexDat1 = new ReX(@"(20|19)\d{2}[-_](0[1-9]|1[0-2])[-_](0[1-9]|[1-2]\d|3[0-2])");
        /// <summary> this gets these dates : 2018 02 20</summary>
        public static System.Text.RegularExpressions.Regex RexDat2 = new ReX(@"(20|19)\d{2}\s+(0[1-9]|1[0-2])\s+(0[1-9]|[1-2]\d|3[0-2])");
    }
}
