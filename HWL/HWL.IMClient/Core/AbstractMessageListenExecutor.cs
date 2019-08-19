using System;
using System.Collections.Generic;
using System.Text;
using HWL.IMCore.Protocol;

namespace HWL.IMClient.Core
{
    public abstract class AbstractMessageListenExecutor<TResponse> : IClientMessageListenExecutor, IClientOperateListener<TResponse>
    {
        private ImMessageResponseHead responseHead = null;
        private TResponse responseBody;

        public void execute(ImMessageContext messageContext)
        {
            responseHead = messageContext.Response.ResponseHead;
            responseBody = this.getResponse(messageContext.Response);

            if(this.checkResponse())
            {
                this.executeCore(messageContext.Type, responseBody);
            }
        }

        private bool checkResponse()
        {
            bool flag = false;
            if (responseHead == null || responseBody == null)
            {
                failure(0, "The response head or body in ImMessageContext is empty.");
            }
            else
            {
                switch (responseHead.Code)
                {
                    case (uint)ImMessageResponseCode.Success:
                        success(responseBody);
                        flag = true;
                        break;
                    case (uint)ImMessageResponseCode.SessionidInvalid:
                        sessionidInvalid();
                        break;
                    case (uint)ImMessageResponseCode.Failed:
                    default:
                        failure(responseHead.Code, responseHead.Message);
                        break;
                }
            }

            return flag;
        }

        public bool executedAndClose()
        {
            return false;
        }

        public ImMessageResponseHead getResponseHead()
        {
            return responseHead;
        }

        public abstract TResponse getResponse(ImMessageResponse response);

        public abstract void executeCore(ImMessageType messageType, TResponse response);

        public virtual void sessionidInvalid()
        {
            Console.WriteLine("Client message listen : session id invalid.");
        }

        public virtual void success(TResponse response)
        {
        }

        public virtual void failure(uint code, string message)
        {
            Console.WriteLine("Client message listen : {0}:{1}.", code, message);
        }
    }
}
