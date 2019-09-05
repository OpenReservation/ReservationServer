using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ActivityReservation.Common
{
    /// <summary>
    /// 文件存储标准接口
    /// </summary>
    public interface IStorageProvider
    {
        Task<string> SaveBytes(byte[] bytes, string fileName, string dir);
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

        public Task<string> SaveBytes(byte[] bytes, string fileName, string dir)
        {
            var fullPath = $"{_options.BaseDir}/{dir}/{fileName}";
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
    /// </summary>
    public class GiteeStorageProvider : IStorageProvider
    {
        public Task<string> SaveBytes(byte[] bytes, string fileName, string dir)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// Github 存储
    /// </summary>
    public class GithubStorageProvider : IStorageProvider
    {
        public Task<string> SaveBytes(byte[] bytes, string fileName, string dir)
        {
            throw new System.NotImplementedException();
        }
    }
}
