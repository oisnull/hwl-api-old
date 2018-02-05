using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWL.Unit.Models
{
    public class AddFriendBean
    {
        public int userId { get; set; }
        public String userName { get; set; }
        public String userHeadImage { get; set; }
        public int friendId { get; set; }
        public String remark { get; set; }
    }
}