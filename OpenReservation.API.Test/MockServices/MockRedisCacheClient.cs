using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using WeihanLi.Redis;

namespace OpenReservation.API.Test.MockServices;

internal class MockRedisCacheClient : ICacheClient
{
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
        throw new NotImplementedException();
    }

    public bool Set<T>(string key, T value, TimeSpan? expiresIn, When when = When.Always, CommandFlags commandFlags = CommandFlags.None)
    {
        throw new NotImplementedException();
    }

    public bool Set<T>(string key, Func<T> func, TimeSpan? expiresIn, When when = When.Always, CommandFlags commandFlags = CommandFlags.None)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetAsync<T>(string key, T value)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiresIn, When when = When.Always, CommandFlags commandFlags = CommandFlags.None)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetAsync<T>(string key, Func<T> func, TimeSpan? expiresIn, When when = When.Always, CommandFlags commandFlags = CommandFlags.None)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetAsync<T>(string key, Func<Task<T>> func, TimeSpan? expiresIn, When when = When.Always, CommandFlags commandFlags = CommandFlags.None)
    {
        throw new NotImplementedException();
    }

    public T GetOrSet<T>(string key, Func<T> func, TimeSpan? expiresIn = null, CommandFlags flags = CommandFlags.None)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> func, TimeSpan? expiresIn = null, CommandFlags flags = CommandFlags.None)
    {
        return func();
    }

    public bool Remove(string key, CommandFlags commandFlags = CommandFlags.None)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(string key, CommandFlags commandFlags = CommandFlags.None)
    {
        throw new NotImplementedException();
    }
}