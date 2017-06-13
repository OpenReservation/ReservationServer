namespace ActivityReservation.WechatAPI.Helper
{
    internal class WeChatConsts
    {
        /// <summary>
        /// 定义Token，与微信公共平台上的Token保持一致
        /// </summary>
        public static readonly string Token = Common.ConfigurationHelper.AppSetting("WechatToken");

        /// <summary>
        /// AppId 要与 微信公共平台 上的 AppId 保持一致
        /// </summary>
        public static readonly string AppId = Common.ConfigurationHelper.AppSetting("WechatAppId");

        /// <summary>
        /// EncodingAESKey
        /// </summary>
        public static readonly string AESKey = Common.ConfigurationHelper.AppSetting("WechatAESKey");
    }
}