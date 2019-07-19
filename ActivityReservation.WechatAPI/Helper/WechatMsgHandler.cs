using System;
using System.Threading.Tasks;
using System.Xml;
using ActivityReservation.Common;
using Microsoft.Extensions.Configuration;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;
using WeihanLi.Extensions;

namespace ActivityReservation.WechatAPI.Helper
{
    /// <summary>
    /// 微信消息处理帮助类
    /// </summary>
    internal class WechatMsgHandler
    {
        private static readonly ILogHelperLogger Logger = LogHelper.GetLogger(typeof(WechatMsgHandler));

        public static async Task<string> ReturnMessageAsync(string postStr)
        {
            var responseContent = "";
            try
            {
                var xmldoc = new XmlDocument();
                xmldoc.LoadXml(postStr);

                var msgId = xmldoc.SelectSingleNode("/xml/MsgId")?.InnerText;
                if (msgId.IsNotNullOrEmpty())
                {
                    //var firewall = RedisManager.GetFirewallClient($"wechatMsgFirewall-{msgId}", TimeSpan.FromSeconds(2));
                    //if (!await firewall.HitAsync())
                    //{
                    //    Logger.Info($"duplicate msg blocked, msg id: {msgId}");
                    //    return string.Empty;
                    //}
                }
                var msgType = xmldoc.SelectSingleNode("/xml/MsgType");

                if (msgType != null)
                {
                    switch (msgType.InnerText)
                    {
                        case "event":
                            responseContent = EventHandle(xmldoc); //事件处理
                            break;

                        case "text":
                            responseContent = await TextMsgHandleAsync(xmldoc); //接受文本消息处理
                            break;

                        case "image":
                            responseContent = ImageMsgHandle(xmldoc); //图片消息
                            break;

                        case "voice":
                            responseContent = await VoiceMsgHandleAsync(xmldoc); //语音消息
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "回复消息发生异常，异常信息：" + ex.Message);
            }
            return responseContent;
        }

        private static async Task<string> VoiceMsgHandleAsync(XmlDocument xmldoc)
        {
            string responseContent = "", reply = null;
            var ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            var FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            var Content = xmldoc.SelectSingleNode("/xml/Recognition");
            if (Content != null)
            {
                //设置回复消息
                reply = await DependencyResolver.Current.ResolveService<ChatBotHelper>()
                    .GetBotReplyAsync(Content.InnerText);
                if (reply == "error")
                {
                    reply = Content.InnerText;
                }
                responseContent = string.Format(ReplyMessageType.MessageText,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.UtcNow.Ticks,
                    String.IsNullOrEmpty(reply) ? "Sorry,I can not follow you." : reply);
            }
            //Logger.Debug("接受的消息：" + Content.InnerText + "\r\n 发送的消息：" + reply);
            return responseContent;
        }

        private static string ImageMsgHandle(XmlDocument xmldoc)
        {
            var responseContent = "";
            var ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            var FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            var MediaId = xmldoc.SelectSingleNode("/xml/MediaId");
            if (MediaId != null)
            {
                //reply = "这是回复";
                responseContent = string.Format(ReplyMessageType.MessageImage,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.UtcNow.Ticks,
                    //"您发送的消息是："+Content.InnerText+"\r\n 我的回复："+reply + "\r\n<a href=\"http://private.chinacloudsites.cn/\">点击进入我们官网</a>"
                    MediaId.InnerText
                );
            }
            return responseContent;
        }

        private static async Task<string> TextMsgHandleAsync(XmlDocument xmldoc)
        {
            string responseContent = "", reply = "";
            var ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            var FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            var Content = xmldoc.SelectSingleNode("/xml/Content");
            if (Content != null)
            {
                //设置回复消息
                reply = await DependencyResolver.Current.ResolveService<ChatBotHelper>()
                    .GetBotReplyAsync(Content.InnerText);
                if (reply == "error")
                {
                    reply = Content.InnerText;
                }
                responseContent = string.Format(ReplyMessageType.MessageText,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.UtcNow.Ticks,
                    reply);
            }
            return responseContent;
        }

        private static string EventHandle(XmlDocument xmldoc)
        {
            var responseContent = "";
            var @event = xmldoc.SelectSingleNode("/xml/Event")?.InnerText;
            var eventKey = xmldoc.SelectSingleNode("/xml/EventKey")?.InnerText;
            var ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            var FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            if (@event != null)
            {
                if ("subscribe".EqualsIgnoreCase(@event))
                {
                    // 关注
                    var reply = DependencyResolver.Current.ResolveService<IConfiguration>()
                                .GetAppSetting("WeChatSubscribeReply");
                    if (string.IsNullOrEmpty(reply))
                    {
                        responseContent = "";
                    }
                    else
                    {
                        responseContent = string.Format(ReplyMessageType.MessageText,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.UtcNow.Ticks,
                            reply
                            );
                    }
                }
                else if ("unsubscribe".EqualsIgnoreCase(@event))
                {
                    //  取消关注
                }
                //菜单单击事件
                else if (@event.Equals("CLICK"))
                {
                    //if (eventKey == "click_one") //click_one
                    //{
                    //    responseContent = string.Format(ReplyMessageType.MessageText,
                    //        FromUserName.InnerText,
                    //        ToUserName.InnerText,
                    //        DateTime.UtcNow.Ticks,
                    //        $"你点击的是 {eventKey}");
                    //}
                }
            }
            return responseContent;
        }
    }

    //回复消息类型
    internal static class ReplyMessageType
    {
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public const string MessageText = @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                          </xml>";

        /// <summary>
        /// 图文消息主体
        /// </summary>
        public const string MessageNewsMain = @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[news]]></MsgType>
                            <ArticleCount>{3}</ArticleCount>
                            <Articles>
                            {4}
                            </Articles>
                            </xml> ";

        /// <summary>
        /// 图文消息项
        /// </summary>
        public const string MessageNewsItem = @"<item>
                            <Title><![CDATA[{0}]]></Title>
                            <Description><![CDATA[{1}]]></Description>
                            <PicUrl><![CDATA[{2}]]></PicUrl>
                            <Url><![CDATA[{3}]]></Url>
                         </item>";

        public const string MessageImage = @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[image]]></MsgType>
                            <Image>
                                <MediaId><![CDATA[{3}]]></MediaId>
                            </Image>
                          </xml>";
    }
}
