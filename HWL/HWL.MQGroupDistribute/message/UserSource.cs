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


        public List<string> GetUserQueueSymbolList(int fromUserId, string groupGuid)
        {
            List<int> users = groupAction.GetGroupUserIds(groupGuid);
            if (users == null || users.Count <= 0) return null;

            if (fromUserId > 0)
            {
                users.Remove(fromUserId);
            }

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
}
