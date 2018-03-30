using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Body
{
    public class DeleteCircleInfoRequestBody
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        public int CircleId { get; set; }
    }

    public class DeleteCircleInfoResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
