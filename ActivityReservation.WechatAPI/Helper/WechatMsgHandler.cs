using System;
using System.Xml;
using ActivityReservation.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;
using WeihanLi.Extensions;
using WeihanLi.Redis;

namespace ActivityReservation.WechatAPI.Helper
{
    /// <summary>
    /// 微信消息处理帮助类
    /// </summary>
    internal class WechatMsgHandler
    {
        private static readonly ILogHelperLogger Logger = LogHelper.GetLogger(typeof(WechatMsgHandler));

        public static string ReturnMessage(string postStr)
        {
            var responseContent = "";
            try
            {
                var xmldoc = new XmlDocument();
                xmldoc.LoadXml(postStr);

                var msgId = xmldoc.SelectSingleNode("/xml/MsgId")?.InnerText;
                if (msgId.IsNullOrEmpty())
                {
                    var firewall = RedisManager.GetFirewallClient($"wechatMsgFirewall-{msgId}", TimeSpan.FromSeconds(2));
                    if (!firewall.Hit())
                    {
                        return string.Empty;
                    }
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
                            responseContent = TextMsgHandle(xmldoc); //接受文本消息处理
                            break;

                        case "image":
                            responseContent = ImageMsgHandle(xmldoc); //图片消息
                            break;

                        case "voice":
                            responseContent = VoiceMsgHandleAsync(xmldoc); //语音消息
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("发生异常，异常信息：" + ex.Message + ex.StackTrace);
            }
            return responseContent;
        }

        private static string VoiceMsgHandleAsync(XmlDocument xmldoc)
        {
            string responseContent = "", reply = null;
            var ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            var FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            var Content = xmldoc.SelectSingleNode("/xml/Recognition");
            if (Content != null)
            {
                //设置回复消息
                reply = ChatRobotHelper.GetBotReply(Content.InnerText.UrlEncode());
                if (reply == "error")
                {
                    reply = Content.InnerText;
                }
                responseContent = string.Format(ReplyMessageType.MessageText,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    String.IsNullOrEmpty(reply) ? "Sorry,I can not follow you." : reply);
            }
            Logger.Debug("接受的消息：" + Content.InnerText + "\r\n 发送的消息：" + reply);
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
                    DateTime.Now.Ticks,
                    //"您发送的消息是："+Content.InnerText+"\r\n 我的回复："+reply + "\r\n<a href=\"http://private.chinacloudsites.cn/\">点击进入我们官网</a>"
                    MediaId.InnerText
                );
            }
            return responseContent;
        }

        private static string TextMsgHandle(XmlDocument xmldoc)
        {
            string responseContent = "", reply = "";
            var ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            var FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            var Content = xmldoc.SelectSingleNode("/xml/Content");
            if (Content != null)
            {
                //设置回复消息
                reply = ChatRobotHelper.GetBotReply(Content.InnerText.UrlEncode());
                if (reply == "error")
                {
                    reply = Content.InnerText;
                }
                responseContent = string.Format(ReplyMessageType.MessageText,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    reply);
            }
            Logger.Info("接受的消息：" + Content.InnerText + "\r\n 发送的消息：" + reply);
            return responseContent;
        }

        private static string EventHandle(XmlDocument xmldoc)
        {
            var responseContent = "";
            var Event = xmldoc.SelectSingleNode("/xml/Event");
            var EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
            var ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            var FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            if (Event != null)
            {
                //菜单单击事件
                if (Event.InnerText.Equals("CLICK"))
                {
                    if (EventKey.InnerText.Equals("click_one")) //click_one
                    {
                        responseContent = string.Format(ReplyMessageType.MessageText,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "你点击的是click_one");
                    }
                    else if (EventKey.InnerText.Equals("click_two")) //click_two
                    {
                        responseContent = string.Format(ReplyMessageType.MessageNewsMain,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "2",
                            string.Format(ReplyMessageType.MessageNewsItem, "我要寄件", "",
                                "http://www.soso.com/orderPlace.jpg",
                                "http://www.soso.com/") +
                            string.Format(ReplyMessageType.MessageNewsItem, "订单管理", "",
                                "http://www.soso.com/orderManage.jpg",
                                "http://www.soso.com/"));
                    }
                    else if (EventKey.InnerText.Equals("click_three")) //click_three
                    {
                        responseContent = string.Format(ReplyMessageType.MessageNewsMain,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "1",
                            string.Format(ReplyMessageType.MessageNewsItem, "标题", "摘要",
                                "http://www.soso.com/jieshao.jpg",
                                "http://www.soso.com/"));
                    }
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
        public static string MessageText => @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                          </xml>";

        /// <summary>
        /// 图文消息主体
        /// </summary>
        public static string MessageNewsMain => @"<xml>
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
        public static string MessageNewsItem => @"<item>
                            <Title><![CDATA[{0}]]></Title>
                            <Description><![CDATA[{1}]]></Description>
                            <PicUrl><![CDATA[{2}]]></PicUrl>
                            <Url><![CDATA[{3}]]></Url>
                         </item>";

        public static string MessageImage => @"<xml>
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
