using HWL.Entity;
using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Body
{
    public class AddCircleInfoRequestBody
    {
        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int UserId { get; set; }
        //public CircleContentType ContentType { get; set; }
        public string Content { get; set; }
        public List<ImageInfo> Images { get; set; }
        public int PosId { get; set; }
        public string PosDesc { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string LinkUrl { get; set; }
        public string LinkTitle { get; set; }
        public string LinkImage { get; set; }
    }

    public class AddCircleInfoResponseBody
    {
        public int CircleId { get; set; }
        public CircleContentType ContentType { get; set; }
        public DateTime PublishTime { get; set; }
    }
}
