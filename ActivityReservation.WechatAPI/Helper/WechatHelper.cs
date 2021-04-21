using System;
using System.Net.Http;
using System.Threading.Tasks;
using ActivityReservation.WechatAPI.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatHelper
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public WechatHelper(HttpClient httpClient, ILogger<WechatHelper> logger, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// GetAccessTokenUrlFormat
        /// 0:appid
        /// 1:secret
        /// </summary>
        private const string GetAccessTokenUrlFormat = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 更新公众号菜单
        /// 0:AccessToken
        /// https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141013
        /// </summary>
        private const string UpdateMpWechatMenuUrlFormat = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";

        /// <summary>
        /// 删除公众号菜单
        /// 0:AccessToken
        /// </summary>
        private const string DeleteMpWechatMenuUrlFormat = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";

        /// <summary>
        /// 获取公共号 AccessToken
        /// </summary>
        /// <returns>AccessToken</returns>
        public async Task<string> GetAccessTokenAsync()
        {
            var token = await _memoryCache.GetOrCreateAsync("wechat_access_token",
                 async (entry) =>
                {
                    var tokenRes = await RetryHelper.TryInvokeAsync(() => _httpClient
                            .GetStringAsync(
                                GetAccessTokenUrlFormat.FormatWith(WeChatConsts.AppId, WeChatConsts.AppSecret))
                            .ContinueWith(r => r.Result.JsonToObject<AccessTokenEntity>()),
                        result => result.AccessToken.IsNotNullOrWhiteSpace());
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(7140);
                    return tokenRes;
                });
            return token?.AccessToken;
        }

        /// <summary>
        /// 更新微信公众号菜单
        /// </summary>
        /// <param name="menu">菜单信息
        /// {
        ///  "button":[             //button定义该结构为一个菜单
        ///   {
        ///        "name":"分支主菜单名",
        ///        "sub_button":[　　　　//sub_button定义子菜单
        ///         {
        ///            "type":"click",　　//按钮类型
        ///            "name":"分支子菜单名1",　　//菜单名称
        ///            "key":"loveSuzhou"　　//菜单key值
        ///         },
        ///         {
        ///            "type":"click",
        ///            "name":"分支子菜单名2",
        ///            "key":"liveSuzhou"
        ///         }]
        ///    },
        ///    {
        ///        "type":"click",
        ///        "name":"独立菜单",
        ///        "key":"lianxiUs"
        ///    }]
        /// }
        /// </param>
        /// <returns></returns>
        public async Task<bool> UpdateWechatMenuAsync(object menu)
        {
            var accessToken = await GetAccessTokenAsync();
            if (accessToken.IsNullOrWhiteSpace())
            {
                _logger.Error($"获取 AccessToken 失败");
                return false;
            }
            var url = UpdateMpWechatMenuUrlFormat.FormatWith(accessToken);
            var response = await _httpClient.PostAsJsonAsync(url, menu);
            var result = await response.Content.ReadAsStringAsync()
                .ContinueWith(r => r.Result.JsonToObject<WechatResponseEntity>());
            return result.Success;
        }

        /// <summary>
        /// 删除微信公众号菜单
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteWechatMenuAsync()
        {
            var accessToken = await GetAccessTokenAsync();
            if (accessToken.IsNullOrWhiteSpace())
            {
                _logger.Error($"获取 AccessToken 失败");
                return false;
            }
            var url = DeleteMpWechatMenuUrlFormat.FormatWith(accessToken);
            var result = await _httpClient.GetStringAsync(url)
                .ContinueWith(r => r.Result.JsonToObject<WechatResponseEntity>());
            return result.Success;
        }
    }
}
