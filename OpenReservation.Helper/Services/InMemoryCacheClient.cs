using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using WeihanLi.Redis;

namespace OpenReservation.Services
{
    public class InMemoryCacheClient : ICacheClient
    {
        private readonly IMemoryCache _cache;

        public InMemoryCacheClient(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool Expire(string key, TimeSpan? expiresIn, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExpireAsync(string key, TimeSpan? expiresIn, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public string Get(string key, CommandFlags commandFlags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAsync(string key, CommandFlags commandFlags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key, CommandFlags commandFlags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(string key, CommandFlags commandFlags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public bool Set<T>(string key, T value)
        {
            return false;
        }

        public bool Set<T>(string key, T value, TimeSpan? expiresIn, When when = When.Always, CommandFlags commandFlags = CommandFlags.None)
        {
            return false;
        }

        public bool Set<T>(string key, Func<T> func, TimeSpan? expiresIn, When when = When.Always, CommandFlags commandFlags = CommandFlags.None)
        {
            return false;
        }

        public Task<bool> SetAsync<T>(string key, T value)
        {
            return Task.FromResult(false);
        }

        public Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiresIn, When when = When.Always, CommandFlags commandFlags = CommandFlags.None)
        {
            return Task.FromResult(false);
        }

        public Task<bool> SetAsync<T>(string key, Func<T> func, TimeSpan? expiresIn, When when = When.Always, CommandFlags commandFlags = CommandFlags.None)
        {
            return Task.FromResult(false);
        }

        public Task<bool> SetAsync<T>(string key, Func<Task<T>> func, TimeSpan? expiresIn, When when = When.Always, CommandFlags commandFlags = CommandFlags.None)
        {
            return Task.FromResult(false);
        }

        public T GetOrSet<T>(string key, Func<T> func, TimeSpan? expiresIn = null, CommandFlags flags = CommandFlags.None)
        {
            return func();
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> func, TimeSpan? expiresIn = null, CommandFlags flags = CommandFlags.None)
        {
            return func();
        }

        public bool Remove(string key, CommandFlags commandFlags = CommandFlags.None)
        {
            return true;
        }

        public Task<bool> RemoveAsync(string key, CommandFlags commandFlags = CommandFlags.None)
        {
            return Task.FromResult(true);
        }
    }
}
