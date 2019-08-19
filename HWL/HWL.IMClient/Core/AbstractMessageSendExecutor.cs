using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Core
{
    public abstract class AbstractMessageSendExecutor
    {
        public ImMessageContext getMessageContext()
        {
            ImMessageRequest request = getMessageRequest();
            if (request.RequestHead == null)
            {
                request.RequestHead = MessageRequestHeadManager.getRequestHead();
            }

            return new ImMessageContext()
            {
                Request = request,
                Type = getMessageType()
            };
        }

        public abstract ImMessageType getMessageType();

        public abstract ImMessageRequest getMessageRequest();

        public virtual void success()
        {
            Console.WriteLine("Client send {0} message to server success", getMessageType().ToString());
        }

        public virtual void failure(string message)
        {
            Console.WriteLine("Client send {0} message to server failure.{1}", getMessageType().ToString(), message);
        }
    }
}
