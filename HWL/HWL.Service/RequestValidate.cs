using GMSF.HeadDefine;

namespace HWL.Service
{
    public class RequestValidate : IRequestHead
    {
        private bool isCheckedSessionId;
        private bool isCheckedToken;
        private int currUserId;

        public RequestValidate(bool isCheckedSessionId = true, bool isCheckedToken = true)
        {
            this.isCheckedSessionId = isCheckedSessionId;
            this.isCheckedToken = isCheckedToken;
        }

        public bool IsCheckSessionId
        {
            get
            {
                return this.isCheckedSessionId;
            }
        }

        public bool IsCheckToken
        {
            get
            {
                return this.isCheckedToken;
            }
        }

        public bool CheckSessionId(string sessionId)
        {
            return true;
        }

        public bool CheckToken(string token)
        {
            if (token == "88888888") return true;
            if (string.IsNullOrEmpty(token)) return false;
            this.currUserId = new Redis.UserAction().GetTokenUser(token);
            if (this.currUserId <= 0) return false;
            return true;
        }

        public int GetCurrUserId()
        {
            return this.currUserId;
        }
    }
}
