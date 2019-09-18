namespace ActivityReservation.WechatAPI.Model
{
    public class WeChatMsgBase
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 发送消息微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 接受消息的微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }
    }

    /// <summary>
    /// 文本消息模型
    /// </summary>
    public class WeChatTextMsgModel : WeChatMsgBase
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// 图片消息模型
    /// </summary>
    public class WeChatImageMsgModel : WeChatMsgBase
    {
        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId { get; set; }
    }

    /// <summary>
    /// 语音消息模型
    /// </summary>
    public class WechatVoiceMsgModel : WeChatMsgBase
    {
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
    }

    /// <summary>
    /// 视频消息模型
    /// </summary>
    public class WeChatVideoMsgModel : WeChatMsgBase
    {
        /// <summary>
        /// 视频消息缩略图媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId { get; set; }
    }

    /// <summary>
    /// 位置信息模型
    /// </summary>
    public class WeChatLocationMsgModel : WeChatMsgBase
    {
        //public string MsgType
        //{
        //    get { return "location"; }
        //}

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
    }

    /// <summary>
    /// 链接消息模型
    /// </summary>
    public class WeChatLinkMsgModel : WeChatMsgBase
    {
        ///// <summary>
        ///// 消息类型
        //public string MsgType
        //{
        //    get { return "link"; }
        //}

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
    }
}
