using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Entity.Extends
{
    public class GroupUserInfo
    {
        public string GroupGuid { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserHeadImage { get; set; }
    }

    public class GroupInfo
    {
        public string GroupGuid { get; set; }
        public string GroupName { get; set; }
        public string GroupNote { get; set; }
        public int BuildUserId { get; set; }
        public int GroupUserCount { get; set; }
        public string BuildDate { get; set; }
        public string UpdateDate { get; set; }
        //public string GroupImage { get; set; }
        public List<GroupUserInfo> GroupUsers { get; set; }
    }

    public class GroupUserEqualityComparer : IEqualityComparer<t_group_user>
    {
        public bool Equals(t_group_user x, t_group_user y)
        {
            return x.group_guid == y.group_guid && x.user_id == y.user_id;
        }

        public int GetHashCode(t_group_user obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
