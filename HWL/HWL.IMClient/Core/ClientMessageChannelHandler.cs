using DotNetty.Transport.Channels;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Core
{
    public class ClientMessageChannelHandler : ChannelHandlerAdapter
    {
        private IClientChannelListener _channelListener;
        private Action disConnected;

        public ClientMessageChannelHandler(IClientChannelListener channelListener, Action disConnected)
        {
            this._channelListener = channelListener;
        }

        public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        {
            //Console.WriteLine("Client listen : {0}", msg.ToString());
            //this._messageOperator.listen(msg as ImMessageContext);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
            _channelListener.onConnected(context.Channel.LocalAddress.ToString(), context.Channel.RemoteAddress.ToString());
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            disConnected();
            _channelListener.onDisconnected(context.Channel.LocalAddress.ToString());
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            base.ExceptionCaught(context, exception);
            disConnected();
            _channelListener.onError(context.Channel.LocalAddress.ToString(), exception.Message);
        }
    }
    //public class ClientMessageChannelHandler : SimpleChannelInboundHandler<ImMessageContext>
    //{
    //    private IClientChannelListener _channelListener;
    //    private ClientMessageOperator _messageOperator;

    //    public ClientMessageChannelHandler(ClientMessageOperator messageOperator, IClientChannelListener channelListener)
    //    {
    //        this._channelListener = channelListener;
    //        this._messageOperator = messageOperator;
    //    }

    //    protected override void ChannelRead0(IChannelHandlerContext ctx, ImMessageContext msg)
    //    {
    //        Console.WriteLine("Client listen : {0}", msg.ToString());
    //        this._messageOperator.listen(msg);
    //    }

    //    public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

    //    public override void ChannelActive(IChannelHandlerContext context)
    //    {
    //        _channelListener.onConnected(context.Channel.LocalAddress.ToString(), context.Channel.RemoteAddress.ToString());
    //    }

    //    public override void ChannelInactive(IChannelHandlerContext context)
    //    {
    //        base.ChannelInactive(context);
    //        _channelListener.onDisconnected(context.Channel.LocalAddress.ToString());
    //    }

    //    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    //    {
    //        base.ExceptionCaught(context, exception);
    //        _channelListener.onError(context.Channel.LocalAddress.ToString(), exception.Message);
    //    }
    //}
}
