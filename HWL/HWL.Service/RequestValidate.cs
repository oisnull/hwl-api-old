using GMSF.HeadDefine;

namespace HWL.Service
{
    public class RequestValidate : IRequestHead
    {
        private bool isCheckedSessionId;
        private bool isCheckedToken;

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
            return true;
        }
    }
}
