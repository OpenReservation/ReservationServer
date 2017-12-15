namespace ActivityReservation.WechatAPI.Model
{
    public interface IWechatMsg
    {
        /// <summary>
        /// 消息id
        /// </summary>
        long MsgId { get; set; }

        /// <summary>
        /// 发送消息用户id
        /// </summary>
        string FromUserName { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        string MsgType { get; }

        /// <summary>
        /// 返回微信消息
        /// </summary>
        //IWechatReply ReplyMsgReply { get; set; }
    }

    /// <summary>
    /// 文本消息模型
    /// </summary>
    public class WechatRequestTextMsgModel : IWechatMsg
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType
        {
            get { return "text"; }
        }

        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }

    /// <summary>
    /// 图片消息模型
    /// </summary>
    public class WechatRequestImageMsgModel
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType
        {
            get { return "image"; }
        }

        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }

    /// <summary>
    /// 语音消息模型
    /// </summary>
    public class WechatRequestVoiceMsgModel
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType
        {
            get { return "voice"; }
        }

        /// <summary>
        /// 语音格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 语音识别结果
        /// </summary>
        public string Recognition { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }

    /// <summary>
    /// 视频消息模型
    /// </summary>
    public class WechatRequestVideoMsgModel
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType
        {
            get { return "video"; }
        }

        /// <summary>
        /// 视频消息缩略图媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }

    /// <summary>
    /// 位置信息模型
    /// </summary>
    public class WechatRequestLocationMsgModel
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType
        {
            get { return "location"; }
        }

        /// <summary>
        /// 地理位置维度
        /// </summary>
        public float Location_X { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public float Location_Y { get; set; }

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }

    /// <summary>
    /// 链接消息模型
    /// </summary>
    public class WechatRequestLinkMsgModel
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType
        {
            get { return "link"; }
        }

        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }
}