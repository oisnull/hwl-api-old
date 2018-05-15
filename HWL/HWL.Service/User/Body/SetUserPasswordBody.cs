using HWL.Entity;

namespace HWL.Service.User.Body
{
    public class SetUserPasswordRequestBody
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 终端需要进行加密后再传过来
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 终端需要进行加密后再传过来
        /// </summary>
        public string PasswordOK { get; set; }
        public string CheckCode { get; set; }
    }

    public class SetUserPasswordResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
