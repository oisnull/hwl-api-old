using HWL.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.MQGroupDistribute.message
{
    public class UserSource
    {
        GroupAction groupAction;
        public UserSource()
        {
            groupAction = new GroupAction();
        }


        public List<string> GetUserQueueSymbolList(string groupGuid)
        {
            List<int> users = groupAction.GetGroupUserIds(groupGuid);
            if (users == null || users.Count <= 0) return null;

            return users.ConvertAll(u => this.GetUserQueueName(u));
        }

        private string GetUserQueueName(int userId)
        {
            if (userId <= 0)
            {
                return "user-none-queue";
            }
            return string.Format("user-{0}-queue", userId);
        }
    }
    public class GroupActionUnit
    {
        public static void AddGroupUser()
        {
            int groupCount = 5;
            int groupUserCount = 500;//每个组里面的人数

            GroupAction act = new GroupAction();
            for (int i = 1; i <= groupCount; i++)
            {
                List<int> users = new List<int>();
                for (int j = 1; j <= groupUserCount; j++)
                {
                    users.Add(j);
                }
                act.SaveGroupUser("group-guid-" + i, users.ToArray());
            }
        }
    }
}
