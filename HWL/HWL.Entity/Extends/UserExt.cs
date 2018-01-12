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
}
