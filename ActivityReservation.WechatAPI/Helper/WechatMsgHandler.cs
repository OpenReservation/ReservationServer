using Common;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Xml;
using Senparc.Weixin.MP.Entities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Entities.Request;
using System.Xml.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.Context;
using System.Web;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.WechatAPI.Helper
{
    /// <summary>
    /// 微信消息处理帮助类
    /// </summary>
    public class WechatMsgHandler: MessageHandler<WechatContext>
    {

        private static LogHelper logger = new LogHelper(typeof(WechatMsgHandler));
        #region OldMethod

        public static string ReturnMessage(string postStr)
        {
            string responseContent = "";
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(postStr);
                XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
                XmlNode formUser = xmldoc.SelectSingleNode("/xml/FromUserName");
                if (MsgType != null)
                {
                    switch (MsgType.InnerText)
                    {
                        case "event":
                            responseContent = EventHandle(xmldoc);//事件处理
                            break;

                        case "text":
                            responseContent = TextMsgHandle(xmldoc);//接受文本消息处理
                            break;

                        case "image":
                            responseContent = ImageMsgHandle(xmldoc);//图片消息
                            break;

                        case "voice":
                            responseContent = VoiceMsgHandleAsync(xmldoc);//语音消息
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("发生异常，异常信息：" + ex.Message + ex.StackTrace);
            }
            return responseContent;
        }

        private static string VoiceMsgHandleAsync(XmlDocument xmldoc)
        {
            string responseContent = "", reply = null;
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Recognition");
            if (Content != null)
            {
                //设置回复消息
                reply = ChatRobotHelper.GetBotReply(HttpContext.Current.Server.UrlEncode(Content.InnerText));
                if (reply == "error")
                {
                    reply = Content.InnerText;
                }
                responseContent = string.Format(ReplyMessageType.Message_Text,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    String.IsNullOrEmpty(reply) ? "Sorry,I can not follow you." : reply);
            }
            logger.Debug("接受的消息：" + Content.InnerText + "\r\n 发送的消息：" + reply);
            return responseContent;
        }

        private static string ImageMsgHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode MediaId = xmldoc.SelectSingleNode("/xml/MediaId");
            if (MediaId != null)
            {
                //reply = "这是回复";
                responseContent = string.Format(ReplyMessageType.Message_Image,
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
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            if (Content != null)
            {
                //设置回复消息
                reply = ChatRobotHelper.GetBotReply(HttpContext.Current.Server.UrlEncode(Content.InnerText));
                if (reply == "error")
                {
                    reply = Content.InnerText;
                }
                responseContent = string.Format(ReplyMessageType.Message_Text,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    reply);
            }
            logger.Info("接受的消息：" + Content.InnerText + "\r\n 发送的消息：" + reply);
            return responseContent;
        }

        private static string EventHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode Event = xmldoc.SelectSingleNode("/xml/Event");
            XmlNode EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            if (Event != null)
            {
                //菜单单击事件
                if (Event.InnerText.Equals("CLICK"))
                {
                    if (EventKey.InnerText.Equals("click_one"))//click_one
                    {
                        responseContent = string.Format(ReplyMessageType.Message_Text,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "你点击的是click_one");
                    }
                    else if (EventKey.InnerText.Equals("click_two"))//click_two
                    {
                        responseContent = string.Format(ReplyMessageType.Message_News_Main,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "2",
                             string.Format(ReplyMessageType.Message_News_Item, "我要寄件", "",
                             "http://www.soso.com/orderPlace.jpg",
                             "http://www.soso.com/") +
                             string.Format(ReplyMessageType.Message_News_Item, "订单管理", "",
                             "http://www.soso.com/orderManage.jpg",
                             "http://www.soso.com/"));
                    }
                    else if (EventKey.InnerText.Equals("click_three"))//click_three
                    {
                        responseContent = string.Format(ReplyMessageType.Message_News_Main,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "1",
                             string.Format(ReplyMessageType.Message_News_Item, "标题", "摘要",
                             "http://www.soso.com/jieshao.jpg",
                             "http://www.soso.com/"));
                    }
                }
            }
            return responseContent;
        } 
        #endregion

        /// <summary>
        /// 模板消息集合（Key：checkCode，Value：OpenId）
        /// </summary>
        public static Dictionary<string, string> TemplateMessageCollection = new Dictionary<string, string>();

        public WechatMsgHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            WeixinContext.ExpireMinutes = 3;
            //在指定条件下，不使用消息去重
            base.OmitRepeatedMessageFunc = requestMessage =>
            {
                var textRequestMessage = requestMessage as RequestMessageText;
                if (textRequestMessage != null && textRequestMessage.Content == "容错")
                {
                    return false;
                }
                return true;
            };
        }        

        public WechatMsgHandler(XDocument doc, PostModel postModel, int maxRecordCount = 0)
            : base(doc, postModel, maxRecordCount)
        {
        }

        public override void OnExecuting()
        {
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 执行微信请求
        /// </summary>
        public override void Execute()
        {
            if (CancelExcute)
            {
                return;
            }

            OnExecuting();

            if (CancelExcute)
            {
                return;
            }

            try
            {
                if (RequestMessage == null)
                {
                    return;
                }

                switch (RequestMessage.MsgType)
                {
                    case RequestMsgType.Text:
                        {
                            var requestMessage = RequestMessage as RequestMessageText;
                            ResponseMessage = OnTextOrEventRequest(requestMessage) ?? OnTextRequest(requestMessage);
                        }
                        break;
                    case RequestMsgType.Location:
                        ResponseMessage = OnLocationRequest(RequestMessage as RequestMessageLocation);
                        break;
                    case RequestMsgType.Image:
                        ResponseMessage = OnImageRequest(RequestMessage as RequestMessageImage);
                        break;
                    case RequestMsgType.Voice:
                        ResponseMessage = OnVoiceRequest(RequestMessage as RequestMessageVoice);
                        break;
                    case RequestMsgType.Video:
                        ResponseMessage = OnVideoRequest(RequestMessage as RequestMessageVideo);
                        break;
                    case RequestMsgType.Link:
                        ResponseMessage = OnLinkRequest(RequestMessage as RequestMessageLink);
                        break;
                    case RequestMsgType.ShortVideo:
                        ResponseMessage = OnShortVideoRequest(RequestMessage as RequestMessageShortVideo);
                        break;
                    case RequestMsgType.Event:
                        {
                            var requestMessageText = (RequestMessage as IRequestMessageEventBase).ConvertToRequestMessageText();
                            ResponseMessage = OnTextOrEventRequest(requestMessageText)
                                                ?? OnEventRequest(RequestMessage as IRequestMessageEventBase);
                        }
                        break;
                    default:
                        throw new UnknownRequestMsgTypeException("未知的MsgType请求类型", null);
                }

                //记录上下文
                //此处修改
                if (WeixinContextGlobal.UseWeixinContext && ResponseMessage != null && !string.IsNullOrEmpty(ResponseMessage.FromUserName))
                {
                    WeixinContext.InsertMessage(ResponseMessage);
                }
            }
            catch (Exception ex)
            {
                throw new MessageHandlerException("MessageHandler中Execute()过程发生错误：" + ex.Message, ex);
            }
            finally
            {
                OnExecuted();
            }
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
            * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
            * 只需要在这里统一发出委托请求，如：
            * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
            * return responseMessage;
            */

            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }
    }

    //回复消息类型
    public static class ReplyMessageType
    {
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public static string Message_Text
        {
            get { return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                          </xml>"; }
        }

        /// <summary>
        /// 图文消息主体
        /// </summary>
        public static string Message_News_Main
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[news]]></MsgType>
                            <ArticleCount>{3}</ArticleCount>
                            <Articles>
                            {4}
                            </Articles>
                            </xml> ";
            }
        }

        /// <summary>
        /// 图文消息项
        /// </summary>
        public static string Message_News_Item
        {
            get
            {
                return @"<item>
                            <Title><![CDATA[{0}]]></Title>
                            <Description><![CDATA[{1}]]></Description>
                            <PicUrl><![CDATA[{2}]]></PicUrl>
                            <Url><![CDATA[{3}]]></Url>
                         </item>";
            }
        }

        public static string Message_Image
        {
            get { return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[image]]></MsgType>
                            <Image>
                                <MediaId><![CDATA[{3}]]></MediaId>
                            </Image>
                          </xml>"; }
        }
    }
}