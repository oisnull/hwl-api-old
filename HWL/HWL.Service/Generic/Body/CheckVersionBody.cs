using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Generic.Body
{
    public class CheckVersionRequestBody
    {
        public int UserId { get; set; }
        public string OldVersion { get; set; }
    }

    public class CheckVersionResponseBody
    {
        public bool IsNewVersion { get; set; }

        public string NewVersion { get; set; }

        public string DownLoadUrl { get; set; }
    }
}
