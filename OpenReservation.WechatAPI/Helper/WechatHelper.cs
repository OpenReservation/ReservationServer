﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using OpenReservation.WechatAPI.Entities;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;
using WeihanLi.Redis;

namespace OpenReservation.WechatAPI.Helper;

public class WeChatHelper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public WeChatHelper(HttpClient httpClient, ILogger<WeChatHelper> logger)
    {
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
    /// 获取 AccessToken
    /// </summary>
    /// <param name="appId">appId</param>
    /// <param name="appSecret">appSecret</param>
    /// <returns>AccessToken</returns>
    public async Task<string> GetAccessTokenAsync(string appId, string appSecret)
    {
        var tokenCacheKey = $"wechat_access_token:{appId}";
        var token = RedisManager.CacheClient.Get(tokenCacheKey);
        if (string.IsNullOrEmpty(token))
        {
            using (var redLock = RedisManager.GetRedLockClient(tokenCacheKey, 10))
            {
                if (await redLock.TryLockAsync())
                {
                    var tokenEntity = await RetryHelper.TryInvokeAsync(() => _httpClient.GetStringAsync(GetAccessTokenUrlFormat.FormatWith(appId, appSecret))
                            .ContinueWith(r => r.Result.JsonToObject<AccessTokenEntity>()),
                        result => string.IsNullOrEmpty(result?.AccessToken));
                    if (!string.IsNullOrEmpty(tokenEntity?.AccessToken))
                    {
                        token = tokenEntity.AccessToken;
                        RedisManager.CacheClient.Set(tokenCacheKey, tokenEntity.AccessToken, TimeSpan.FromSeconds(tokenEntity.ExpiresIn));
                    }
                }
            }
        }

        return token;
    }

    // https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN
    /// <summary>
    /// 发送消息
    /// 0: AccessToken
    /// </summary>
    private const string SendMsgUrlFormat = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";

    /// <summary>
    /// 发送微信消息
    /// </summary>
    /// <param name="msg">msg body</param>
    /// <param name="appId">appId</param>
    /// <param name="appSecret">appSecret</param>
    /// <returns></returns>
    public async Task<bool> SendWechatMsg(object msg, string appId, string appSecret)
    {
        var accessToken = await GetAccessTokenAsync(appId, appSecret);
        if (accessToken.IsNullOrEmpty())
        {
            _logger.LogWarning($" failed to get access token for [{appId}]");
            return false;
        }
        var url = SendMsgUrlFormat.FormatWith(accessToken);
        using (var response = await _httpClient.PostAsJsonAsync(url, msg))
        {
            var responseText = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"send wechat msg response: {responseText}");
            var result = responseText.JsonToObject<WechatResponseEntity>();
            return result.ErrorCode == 0;
        }
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
        var accessToken = await GetAccessTokenAsync(MpWeChatConsts.AppId, MpWeChatConsts.AppSecret);
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
        var accessToken = await GetAccessTokenAsync(MpWeChatConsts.AppId, MpWeChatConsts.AppSecret);
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