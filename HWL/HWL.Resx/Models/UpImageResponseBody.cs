using HWL.Entity;
using System.Collections.Generic;

namespace HWL.Resx.Models
{
    //public class ResponseResult
    //{
    //    public ResultStatus Status { get; set; }
    //    public string Message { get; set; }

    //    public string PreviewUrl { get; set; }
    //    public string AccessUrl { get; set; }
    //    public long FileSize { get; set; }
    //    public string PlayTime { get; set; }
    //    public List<string> AccessUrls { get; set; }
    //}

    public class UpImageResponseBody
    {
        public string PreviewUrl { get; set; }
        public string AccessUrl { get; set; }
        public long FileSize { get; set; }
        public string PlayTime { get; set; }
        public List<string> AccessUrls { get; set; }
    }

}
