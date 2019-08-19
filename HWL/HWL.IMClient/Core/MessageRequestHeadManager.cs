using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient
{
    public class MessageRequestHeadManager
    {
        private readonly static ImMessageRequestHead requestHead = new ImMessageRequestHead()
        {
            Client = "",
            Language = "ch-cn",
            Sessionid = "",
            Timestamp = 0,
            Version = "1.0.0",
        };

        public static ImMessageRequestHead getRequestHead()
        {
            return requestHead;
        }

        public static void setSessionId(string sessionId)
        {
            requestHead.Sessionid = sessionId;
        }
    }
}
