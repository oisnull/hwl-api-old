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
        /// <summary>
        /// 如果有值，则获取比这个值小的数据列表
        /// </summary>
        public int MinCircleId { get; set; }

        public int Count { get; set; }

        public List<CircleMatchInfo> CircleMatchInfos { get; set; }
    }
    public class GetUserCircleInfosResponseBody
    {
        public int ViewUserId { get; set; }
        public string ViewUserName { get; set; }
        public string ViewUserImage { get; set; }
        //public string CircleBackImage { get; set; }
        //public string LifeNotes { get; set; }

        public List<CircleInfo> CircleInfos { get; set; }
    }
}
