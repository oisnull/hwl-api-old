using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Body
{
    public class GetCircleDetailRequestBody
    {
        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int UserId { get; set; }
        public int CircleId { get; set; }
        public string UpdateTime { get; set; }
    }
    public class GetCircleDetailResponseBody
    {
        public CircleInfo CircleInfo { get; set; }
    }
}
