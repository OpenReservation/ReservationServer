﻿using Newtonsoft.Json;

namespace OpenReservation.WechatAPI.Entities;

internal class WechatResponseEntity
{
    [JsonProperty("errcode")]
    public int ErrorCode { get; set; }

    [JsonProperty("errmsg")]
    public string ErrorMsg { get; set; }

    public bool Success => ErrorCode == 0;
}