namespace ActivityReservation.WechatAPI.Model
{
    /// <summary>
    /// 微信响应消息
    /// </summary>
    public interface IWechatReply
    {
        /// <summary>
        /// 消息接收人
        /// </summary>
        string ToUserName { get; set; }

        /// <summary>
        /// 消息发送人【公共号id】
        /// </summary>
        string FromUserName { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        string MsgType { get; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        long CreateTime { get; set; }
    }

    /// <summary>
    /// 文本消息模型
    /// </summary>
    public class WechatResponseTextMsgModel : IWechatReply
    {
        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType => "text";

        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// 图片消息模型
    /// </summary>
    public class WechatResponseImageMsgModel : IWechatReply
    {
        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType => "image";

        /// <summary>
        /// 通过上传多媒体文件得到的id
        /// </summary>
        public string MediaId { get; set; }
    }

    /// <summary>
    /// 语音消息模型
    /// </summary>
    public class WechatResponseVoiceMsgModel : IWechatReply
    {
        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType => "voice";

        /// <summary>
        /// 通过上传多媒体文件得到的id
        /// </summary>
        public string MediaId { get; set; }
    }

    /// <summary>
    /// 音乐消息模型
    /// </summary>
    public class WechatReponseMusicMsgModel : IWechatReply
    {
        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType => "music";

        /// <summary>
        /// 缩略图媒体id，通过上传多媒体文件得到的id
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// 音乐标题（可选）
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 音乐描述（可选）
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 音乐链接（可选）
        /// </summary>
        public string MusicUrl { get; set; }

        /// <summary>
        /// 高清音乐链接，WIFI环境下优先使用该链接（可选）
        /// </summary>
        public string HQMusicUrl { get; set; }
    }

    /// <summary>
    /// 视频消息模型
    /// </summary>
    public class WechatReponseVideoMsgModel : IWechatReply
    {
        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType => "video";

        /// <summary>
        /// 通过上传多媒体文件得到的id
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 视频消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 视频消息描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 图文消息模型
    /// </summary>
    public class WechatResponseNewsMsgModel : IWechatReply
    {
        /// <summary>
        /// 发送者账号（一个OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，整型
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        public string MsgType => "news";

        /// <summary>
        /// 通过上传多媒体文件得到的id
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 图文消息个数，限制为10条以内
        /// </summary>
        public int ArticleCount { get; set; }

        /// <summary>
        /// 多条图文消息，默认第一条为大图，不能超过10条
        /// </summary>
        public WechatResponseSingleArticleModel[] Articles { get; set; }
    }

    /// <summary>
    /// 单个Article模型
    /// </summary>
    public class WechatResponseSingleArticleModel
    {
        /// <summary>
        /// 图文消息标题（可选）
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图文消息描述（可选）
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图片链接，jpg、png，大图360*200，小图200*200
        /// </summary>
        public string PicUrl { get; set; }
    }
}