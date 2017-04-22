using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Common
{
    /// <summary>
    /// HTTP请求帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// HTTP GET请求，返回字符串 
        /// </summary>
        /// <param name="url"> url </param>
        /// <returns></returns>
        public static string HttpGetString(string url)
        {
            Uri uri = new Uri(url, UriKind.Absolute);
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.Method = "GET";
            using (var response = request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// HTTP POST 请求，返回字符串 
        /// </summary>
        /// <param name="url"> url </param>
        /// <param name="parameters"> post数据字典 </param>
        /// <param name="queryStringExist"> 是否存在GET请求参数 </param>
        /// <returns></returns>
        public static string HttpGetString(string url, Dictionary<string, string> parameters, bool queryStringExist = false)
        {
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder sbParameters = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        sbParameters.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        if (queryStringExist)
                        {
                            sbParameters.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            sbParameters.AppendFormat("?{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                }
                if (sbParameters.Length > 0)
                {
                    url = url + sbParameters.ToString();
                }
            }
            Uri uri = new Uri(url, UriKind.Absolute);
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.Method = "GET";
            using (var response = request.GetResponse())
            {
                var responseSream = response.GetResponseStream();
                if (responseSream != null)
                {
                    using (StreamReader reader = new StreamReader(responseSream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// HTTP POST 请求，返回字符串 
        /// </summary>
        /// <param name="url"> url </param>
        /// <param name="parameters"> post数据字典 </param>
        /// <returns></returns>
        public static string HttpPostString(string url, Dictionary<string, string> parameters)
        {
            Uri uri = new Uri(url, UriKind.Absolute);
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.Method = "POST";
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] postData = Encoding.UTF8.GetBytes(buffer.ToString());
                var postStream = request.GetRequestStream();
                postStream.Write(postData, 0, postData.Length);
            }
            using (var response = request.GetResponse())
            {
                var responseSream = response.GetResponseStream();
                if (responseSream != null)
                {
                    using (StreamReader reader = new StreamReader(responseSream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// HTTP POST 请求，返回字符串 
        /// </summary>
        /// <param name="url"> url </param>
        /// <param name="postData"> post数据 </param>
        /// <param name="isJsonFormat"> 是否是json格式数据 </param>
        /// <returns></returns>
        public static string HttpPostString(string url, byte[] postData, bool isJsonFormat = true)
        {
            Uri uri = new Uri(url, UriKind.Absolute);
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.Method = "POST";
            if (isJsonFormat)
            {
                request.ContentType = "application/json;charset=UTF-8";
            }
            var postStream = request.GetRequestStream();
            postStream.Write(postData, 0, postData.Length);
            using (var response = request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream(); ;
                if (responseStream != null)
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}