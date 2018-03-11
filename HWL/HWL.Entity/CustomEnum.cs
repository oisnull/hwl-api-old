
namespace HWL.Entity
{
    /// <summary>
    /// 结果状态: 1成功 2失败
    /// </summary>
    public enum ResultStatus
    {
        Success = 1,
        Failed = 2,
        None = 3,
    }

    /// <summary>
    /// 公告类型：1注册公告 2升级公告 3其它
    /// </summary>
    public enum NoticeType
    {
        Register = 1,
        Update = 2,
        Other = 3,
    }

    /// <summary>
    /// 用户状态：1正常 2禁用
    /// </summary>
    public enum UserStatus
    {
        Normal = 1,
        Disable = 2,
    }

    /// <summary>
    /// 性别: 0女 1男 2未知
    /// </summary>
    public enum UserSex
    {
        Women = 0,
        Man = 1,
        Unknow = 2,
    }

    /// <summary>
    /// 验证码类型：1注册 2其它
    /// </summary>
    public enum CodeType
    {
        Register = 1,
        Other = 2,
    }

    /// <summary>
    /// 消息类型: 1,文字 2,图片 3,语音 4,视频 5,链接 6,其它
    /// </summary>
    public enum MsgType
    {
        Word = 1,
        Image = 2,
        Sound = 3,

        Video = 4,
        Link = 5,
        Other = 6,
    }

    /// <summary>
    /// 消息记录类型：1,个人消息 2,群组消息 3,添加好友 4,接受好友请求消息 5,获取会话id(初始连接时使用) 6,服务号消息 7,其它（评论,回复等等,后面根据实际情况再改）
    /// </summary>
    public enum MsgRecordType
    {
        SingleChatMsg = 1,
        GroupChatMsg = 2,
        AddFriendMsg = 3,
        AddFriendAccepted = 4,
        GetSessionId = 5,
        ServiceNO = 6,
        Other = 7,
    }

    /// <summary>
    /// 消息状态 1,已读 2,未读 3,删除
    /// </summary>
    public enum MsgStatus
    {
        Read = 1,
        Unread = 2,
        Delete = 3,
    }

    /// <summary>
    /// 消息发送人 1,自己 2,好友 3,其它
    /// </summary>
    public enum MsgSender
    {
        Self = 1,
        Friend = 2,
        Other = 3,
    }

    /// <summary>
    /// 状态:1在线 2断开 3其它
    /// </summary>
    public enum SessionStatus
    {
        Online = 1,
        Disconnect = 2,
        Other = 3,
    }

    /// <summary>
    /// 添加好友请求的消息处理状态 1,未处理 2,已接受 3,已拒绝 4,已删除 5,其它
    /// </summary>
    public enum MsgFriendActionStatus
    {
        Untreated = 1,
        Accepted = 2,
        Refused = 3,
        Deleted = 4,
        Other = 5,
    }

    /// <summary>
    /// 好友请求消息类型：1添加好友请求消息  2请求通过的消息 3其它
    /// </summary>
    public enum MsgFriendActionType
    {
        AddRequest = 1,
        AllowSuccess = 2,
        Other = 3,
    }

    /// <summary>
    /// 内容类型：1纯文字 2图文 3纯图片 4链接
    /// </summary>
    public enum CircleContentType
    {
        Text = 1,
        TextImage = 2,
        Image = 3,
        Link = 4,
        Other = 5,
    }

    /// <summary>
    /// 圈子类型：1我的 2好友 3附近
    /// </summary>
    public enum CircleType
    {
        Mine = 1,
        Friend = 2,
        Near = 3,
    }

    /// <summary>
    /// 资源类型：1聊天图片 2圈子背景 3圈子发布的动态图片  4录音文件 5用户头像 6聊天视频文件 7其他 
    /// </summary>
    public enum ResxType
    {
        ChatImage = 1,
        CircleBackImage = 2,
        CirclePostImage = 3,
        ChatSound = 4,
        UserHeadImage = 5,
        ChatVideo = 6,
        Other = 7,
    }
}
