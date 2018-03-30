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

        public string Content { get; set; }
        public List<string> Images { get; set; }
        public int PosId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LinkUrl { get; set; }
        public string LinkTitle { get; set; }
        public string LinkImage { get; set; }
    }

    public class AddCircleInfoResponseBody
    {
        public int Id { get; set; }
    }
}
