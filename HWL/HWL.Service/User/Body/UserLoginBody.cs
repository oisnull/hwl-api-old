using HWL.Entity.Extends;

namespace HWL.Service.User.Body
{
    public class UserLoginRequestBody
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 终端需要进行加密后再传过来
        /// </summary>
        public string Password { get; set; }
    }

    public class UserLoginResponseBody
    {
        public UserBaseInfo UserInfo { get; set; }
    }
}
