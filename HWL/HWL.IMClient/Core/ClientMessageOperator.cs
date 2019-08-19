using DotNetty.Transport.Channels;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HWL.IMClient.Core
{
    public class ClientMessageOperator
    {
        private IChannel currentChannel;
        private Dictionary<ImMessageType, IClientMessageListenExecutor> listenExecutors;
        private Action<string> clientAckSender;

        public ClientMessageOperator()
        {
            listenExecutors = new Dictionary<ImMessageType, IClientMessageListenExecutor>();
        }

        public void setChannel(IChannel channel)
        {
            this.currentChannel = channel;
        }

        public void setClientAckSender(Action<string> clientAckSender)
        {
            this.clientAckSender = clientAckSender;
        }

        public void registerListenExecutor(ImMessageType messageType, IClientMessageListenExecutor executor)
        {
            if (this.listenExecutors.ContainsKey(messageType))
            {
                this.listenExecutors.Remove(messageType);
            }
            this.listenExecutors.Add(messageType, executor);
        }

        public void send(AbstractMessageSendExecutor sendExecutor)
        {
            this.send(sendExecutor, null);
        }

        public void send(AbstractMessageSendExecutor sendExecutor, IClientMessageListenExecutor listenExecutor)
        {
            if (sendExecutor == null) return;

            if (listenExecutor != null)
            {
                this.registerListenExecutor(sendExecutor.getMessageType(), listenExecutor);
            }

            try
            {
                Task task = this.currentChannel.WriteAndFlushAsync(sendExecutor.getMessageContext());
                task.Wait();

                if (task.IsCompleted)
                {
                    sendExecutor.success();
                }
                else
                {
                    sendExecutor.failure("server channel error.");
                }
            }
            catch (Exception ex)
            {
                sendExecutor.failure(ex.Message);
            }
        }

        public void listen(ImMessageContext messageContext)
        {
            if (messageContext == null) return;

            if (this.clientAckSender != null && messageContext.Response?.ResponseHead?.Isack == true)
            {
                this.clientAckSender(messageContext.Response?.ResponseHead?.Messageid);
            }

            IClientMessageListenExecutor listenExecutor = listenExecutors[messageContext.Type];
            if (listenExecutor != null)
            {
                listenExecutor.execute(messageContext);
                if (listenExecutor.executedAndClose())
                {
                    this.currentChannel.CloseAsync();
                }
            }
        }
    }
}
