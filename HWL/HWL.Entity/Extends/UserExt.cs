using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.Entity.Extends
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class UserBaseInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户标识号（用户自己去设置的）
        /// </summary>
        public string Symbol { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 登录成功后生成的加密票据
        /// </summary>
        public string Token { get; set; }
        public string Name { get; set; }
        public string HeadImage { get; set; }
        public string CircleBackImage { get; set; }
        public UserSex UserSex { get; set; }
        public string LifeNotes { get; set; }

        /// <summary>
        /// 概念不能为注册地址，应该为家乡地址
        /// </summary>
        public List<int> RegisterPosIdList { get; set; }
        public List<string> RegisterPosList { get; set; }
    }
    public class UserFriendInfo
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string FirstSpell { get; set; }
        public string NameRemark { get; set; }
        public string HeadImage { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }

    }
    /// <summary>
    /// 用户详情
    /// </summary>
    public class UserDetailsInfo
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string HeadImage { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        /// <summary>
        /// 是否是好友
        /// </summary>
        public bool IsFriend { get; set; }
        /// <summary>
        /// 好友备注
        /// </summary>
        public string NameRemark { get; set; }
        public string FirstSpell { get; set; }
    }

}
