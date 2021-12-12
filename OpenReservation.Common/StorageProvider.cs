using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeihanLi.Extensions;

namespace OpenReservation.Common;

/// <summary>
/// 文件存储标准接口
/// </summary>
public interface IStorageProvider
{
    Task<string> SaveBytes(byte[] bytes, string filePath);
}

/// <summary>
/// 本地存储
/// </summary>
public class LocalStorageProvider : IStorageProvider
{
    private readonly LocalStorageProviderOptions _options;

    public LocalStorageProvider(IOptions<LocalStorageProviderOptions> options)
    {
        _options = options.Value;
    }

    public Task<string> SaveBytes(byte[] bytes, string filePath)
    {
        var fullPath = $"{_options.BaseDir}/{filePath}";
        System.IO.File.WriteAllBytes(fullPath, bytes);
        return Task.FromResult(fullPath);
    }
}

public class LocalStorageProviderOptions
{
    public string BaseDir { get; set; }
}

/// <summary>
/// 码云存储
/// https://gitee.com/api/v5/swagger#/postV5ReposOwnerRepoContentsPath
/// </summary>
public class GiteeStorageProvider : IStorageProvider
{
    private const string PostFileApiPathFormat = "/api/v5/repos/{0}/{1}/contents{2}";
    private const string RawFileUrlFormat = "/{0}/{1}/raw/master{2}";

    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly GiteeStorageOptions _options;

    public GiteeStorageProvider(HttpClient httpClient, ILogger<GiteeStorageProvider> logger, IOptions<GiteeStorageOptions> options)
    {
        _logger = logger;
        _httpClient = httpClient;
        _options = options.Value;

        _httpClient.BaseAddress = new Uri(_options.ApiBaseUrl);
    }

    public async Task<string> SaveBytes(byte[] bytes, string filePath)
    {
        var base64Str = Convert.ToBase64String(bytes);

        var contentBytes =
            $"access_token={_options.AccessToken}&message=upload_file&content={base64Str.UrlEncode()}"
                .GetBytes();
        var byteArrayContent = new ByteArrayContent(contentBytes);

        using (var response = await _httpClient.PostAsync(PostFileApiPathFormat.FormatWith(_options.UserName, _options.RepositoryName, filePath),
                   byteArrayContent))
        {
            if (response.IsSuccessStatusCode)
            {
                return $"{_options.ApiBaseUrl}{RawFileUrlFormat.FormatWith(_options.UserName, _options.RepositoryName, filePath)}";
            }

            var result = await response.Content.ReadAsStringAsync();
            _logger.LogWarning($"post file error, response: {result}");

            return null;
        }
    }
}

public class GiteeStorageOptions
{
    private string _apiBaseUrl = "https://gitee.com";

    public string ApiBaseUrl
    {
        get => _apiBaseUrl;
        set
        {
            if (!string.IsNullOrEmpty(value) && Uri.TryCreate(value, UriKind.Absolute, out _))
            {
                _apiBaseUrl = value;
            }
        }
    }

    public string UserName { get; set; }

    public string RepositoryName { get; set; }

    public string AccessToken { get; set; }
}

/// <summary>
/// Github 存储
/// </summary>
public class GithubStorageProvider : IStorageProvider
{
    public Task<string> SaveBytes(byte[] bytes, string filePath)
    {
        throw new System.NotImplementedException();
    }
}