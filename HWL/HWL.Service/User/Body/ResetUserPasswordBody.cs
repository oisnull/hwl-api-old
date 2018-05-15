using HWL.Entity;

namespace HWL.Service.User.Body
{
    public class ResetUserPasswordRequestBody
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        /// <summary>
        /// 终端需要进行加密后再传过来
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 终端需要进行加密后再传过来
        /// </summary>
        public string PasswordOK { get; set; }
    }

    public class ResetUserPasswordResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
