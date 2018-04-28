using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Generic
{
    public class GenericUtility
    {
        public static String formatDate(DateTime? dt)
        {
            if (dt == null) return null;
            return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static String formatDate2(DateTime? dt)
        {
            if (dt == null) return null;
            return dt.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
        }
    }
}
