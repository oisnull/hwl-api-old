using DotNetty.Codecs.Protobuf;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HWL.IMClient.Core
{
    public class IMClientEngine
    {
        public const int STATUS_DISCONNECT = 0;
        public const int STATUS_CONNECT = 1;

        private string host;
        private int port;
        private int status = STATUS_DISCONNECT;// 0:disconnect 1:connect

        private IEventLoopGroup workGroup;
        private Bootstrap bootstrap;
        private IChannel currentChannel;
        //private ClientMessageOperator _messageOperator;
        private IClientConnectListener _connectListener;

        public IMClientEngine(string host, int port)
        {
            this.host = host;
            this.port = port;

            init();
        }

        //public void setMessageOperator(ClientMessageOperator messageOperator)
        //{
        //    this._messageOperator = messageOperator;
        //    if (this._messageOperator == null)
        //    {
        //        throw new ArgumentNullException("ClientMessageOperator");
        //    }
        //}

        public void setConnectListener(IClientConnectListener connectListener)
        {
            this._connectListener = connectListener;
        }

        private void init()
        {
            workGroup = new MultithreadEventLoopGroup();

            bootstrap = new Bootstrap();
            bootstrap.Group(workGroup);
            bootstrap.Channel<TcpSocketChannel>();
            bootstrap.Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
            {
                IChannelPipeline pipeline = channel.Pipeline;
                pipeline.AddLast(new ProtobufVarint32FrameDecoder());
                pipeline.AddLast(new ProtobufDecoder(ImMessageContext.Parser));
                pipeline.AddLast(new ProtobufVarint32LengthFieldPrepender());
                pipeline.AddLast(new ProtobufEncoder());

                pipeline.AddLast(new ClientMessageChannelHandler(_connectListener, resetStatus));
            }));
        }

        public void connect()
        {
            if (status == STATUS_CONNECT)
                return;

            try
            {
                Task<IChannel> task = bootstrap.ConnectAsync(IPAddress.Parse(host), port);
                //task.Wait();

                this.currentChannel = task.Result;
                status = STATUS_CONNECT;
                //_messageOperator.setChannel(currentChannel);
                _connectListener.onBuildConnectionSuccess(this.currentChannel?.LocalAddress?.ToString(), this.currentChannel?.RemoteAddress?.ToString());
            }
            catch (Exception ex)
            {
                _connectListener.onBuildConnectionFailure(this.currentChannel?.LocalAddress?.ToString(), ex.Message);
                status = STATUS_DISCONNECT;
                stop();
            }
        }

        public void send(AbstractMessageSendExecutor sendExecutor)
        {
            if (sendExecutor == null) return;

            try
            {
                if (this.currentChannel == null)
                {
                    sendExecutor.failure("server channel is empty.");
                    return;
                }

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

        public bool isConnected()
        {
            return status == STATUS_CONNECT;
        }

        private void resetStatus()
        {
            status = STATUS_DISCONNECT;
        }

        public void stop()
        {
            string localAddress = "";
            if (this.currentChannel != null)
            {
                localAddress = this.currentChannel.LocalAddress.ToString();
                currentChannel.CloseAsync();
            }
            if (workGroup != null)
            {
                workGroup.ShutdownGracefullyAsync();
            }
            _connectListener.onClosed(localAddress);
        }
    }
}
