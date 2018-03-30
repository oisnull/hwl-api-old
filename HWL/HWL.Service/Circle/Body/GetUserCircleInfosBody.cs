using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Body
{
    public class GetUserCircleInfosRequestBody
    {
        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 查看的用户id
        /// </summary>
        public int ViewUserId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
    public class GetUserCircleInfosResponseBody
    {
        public int ViewUserId { get; set; }
        public string ViewUserName { get; set; }
        public string ViewUserImage { get; set; }

        public List<CircleInfo> CircleInfos { get; set; }
    }
}
