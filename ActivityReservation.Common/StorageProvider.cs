namespace ActivityReservation.Common
{
    /// <summary>
    /// 文件存储标准接口
    /// </summary>
    public interface IStorageProvider
    {
    }

    /// <summary>
    /// 本地存储
    /// </summary>
    public class LocalStorageProvider : IStorageProvider
    {
    }

    /// <summary>
    /// 码云存储
    /// </summary>
    public class GiteeStorageProvider : IStorageProvider
    {
    }

    /// <summary>
    /// Github 存储
    /// </summary>
    public class GithubStorageProvider : IStorageProvider
    {
    }
}
