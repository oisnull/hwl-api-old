using System;

namespace HWL.Entity
{
    public class CustomerEnumDesc
    {
        public static string GetNoticeType(NoticeType nt)
        {
            string desc = "";
            switch (nt)
            {
                case NoticeType.Register:
                    desc = "注册公告";
                    break;
                case NoticeType.Update:
                    desc = "升级公告";
                    break;
                case NoticeType.Other:
                    desc = "其它";
                    break;
                default:
                    break;
            }
            return desc;
        }

        /// <summary>
        /// 获取消息记录标识
        /// </summary>
        public static string GetRecordIdentity(int fromUserId, string groupGuid, MsgRecordType msgRecordType)
        {
            int recordType = (int)msgRecordType;
            switch (msgRecordType)
            {
                case MsgRecordType.SingleChatMsg:
                    return string.Format("{0}-{1}-{2}", fromUserId, "", recordType);
                case MsgRecordType.GroupChatMsg:
                    return string.Format("{0}-{1}-{2}", 0, groupGuid, recordType);
                case MsgRecordType.AddFriendMsg:
                case MsgRecordType.AddFriendAccepted:
                case MsgRecordType.GetSessionId:
                case MsgRecordType.ServiceNO:
                case MsgRecordType.Other:
                default:
                    return string.Format("{0}-{1}-{2}", 0, "", recordType);
            }
        }

        /// <summary>
        /// 获取资源存储的目录配置信息
        /// </summary>
        /// <returns></returns>
        public static string GetResxTypePath(ResxType resxType, int userId = 0)
        {
            string path = "";
            switch (resxType)
            {
                case ResxType.UserHeadImage:
                    path = string.Format("user-head/{0}/", DateTime.Now.ToString("yyyy"));
                    break;
                case ResxType.ChatImage:
                    path = string.Format("chat-image/{0}/{1}/", DateTime.Now.ToString("yyyyMMdd"), userId);
                    break;
                case ResxType.ChatSound:
                    path = string.Format("chat-sound/{0}/{1}/", DateTime.Now.ToString("yyyyMMdd"), userId);
                    break;
                case ResxType.ChatVideo:
                    path = string.Format("chat-video/{0}/{1}/", DateTime.Now.ToString("yyyyMMdd"), userId);
                    break;
                case ResxType.CircleBackImage:
                    path = string.Format("circle-back/{0}/", DateTime.Now.ToString("yyyy"));
                    break;
                case ResxType.CirclePostImage:
                    path = string.Format("circle-post/{0}/{1}/", DateTime.Now.ToString("yyyyMMdd"), userId);
                    break;
                case ResxType.Other:
                default:
                    path = string.Format("other/{0}/{1}/", DateTime.Now.ToString("yyyyMMdd"), userId);
                    break;
            }
            return path;
        }
    }
}
