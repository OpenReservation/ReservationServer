using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Redis 帮助类
    /// </summary>
    public static class RedisHelper
    {
        /// <summary>
        /// logger
        /// </summary>
        private static LogHelper logger = new LogHelper(typeof(RedisHelper));

        private static readonly string redisConn;
        private static ConnectionMultiplexer connection;
        private static readonly int dataBaseIndex = 0;
        private static IDatabase db = null;
        private static object asyncState = new object();
        private static ISubscriber subscriber;

        static RedisHelper()
        {
            redisConn = ConfigurationHelper.AppSetting("redisConf");
            connection = ConnectionMultiplexer.Connect(redisConn);
            connection.ConnectionFailed += (sender, e) => { logger.Error("redis 连接失败"); };
            connection.ConnectionRestored += (sender, e) => { logger.Info("redis 连接恢复"); };
            connection.ErrorMessage += (sender, e) => logger.Error(e.Message);
            connection.InternalError += (sender, e) => logger.Error(e.Origin, e.Exception);
            db = connection.GetDatabase(dataBaseIndex, asyncState);
            subscriber = connection.GetSubscriber(asyncState);
        }

        #region Cache
        #region Exists
        public static bool Exists(string key, CommandFlags flags = CommandFlags.None)
        {
            return db.KeyExists(key, flags);
        }

        public static async Task<bool> ExistsAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            return await db.KeyExistsAsync(key, flags);
        }
        #endregion

        #region Get
        public static string Get(string key, CommandFlags flags = CommandFlags.None)
        {
            return db.StringGet(key, flags);
        }

        public static async Task<string> GetAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            return await db.StringGetAsync(key, flags);
        }

        public static T Get<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            return ConverterHelper.JsonToObject<T>(Get(key, flags));
        }

        public static async Task<T> GetAsync<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            return ConverterHelper.JsonToObject<T>(await GetAsync(key, flags));
        }
        #endregion

        #region Set
        public static bool Set<T>(string key, T value) =>
            Set(key, value, null);
        public static bool Set<T>(string key, T value, TimeSpan? expiration) =>
            Set(key, ConverterHelper.ObjectToJson(value), expiration);

        public static bool Set(string key, string value) =>
            Set(key, value, null);
        public static bool Set(string key, string value, TimeSpan? expiration) =>
            Set(key, value, expiration, When.Always);
        public static bool Set(string key, string value, TimeSpan? expiration, When when) =>
            Set(key, value, expiration, when, CommandFlags.None);
        public static bool Set(string key, string value, TimeSpan? expiration, When when, CommandFlags flags) =>
            db.StringSet(key, value, expiration ?? TimeSpan.FromDays(7), when, flags);

        public static Task<bool> SetAsync<T>(string key, T value) =>
            SetAsync(key, value, null);
        public static Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration) =>
            SetAsync(key, ConverterHelper.ObjectToJson(value), expiration);

        public static Task<bool> SetAsync(string key, RedisValue value) =>
            SetAsync(key, value, null);
        public static Task<bool> SetAsync(string key, RedisValue value, TimeSpan? expiration) =>
            SetAsync(key, value, expiration, When.Always);
        public static Task<bool> SetAsync(string key, RedisValue value, TimeSpan? expiration, When when) =>
            SetAsync(key, value, expiration, when, CommandFlags.None);
        public static async Task<bool> SetAsync(string key, RedisValue value, TimeSpan? expiration, When when, CommandFlags flags) =>
            await db.StringSetAsync(key, value, expiration ?? TimeSpan.FromDays(7), when, flags);
        #endregion

        #region Increment
        public static long Increment(string key) =>
            Increment(key, 1);
        public static long Increment(string key, long value) =>
            Increment(key, value, CommandFlags.None);
        public static long Increment(string key, long value, CommandFlags flags) =>
            db.StringIncrement(key, value, flags);
        public static Task<long> IncrementAsync(string key) =>
            IncrementAsync(key, 1);
        public static Task<long> IncrementAsync(string key, long value) =>
            IncrementAsync(key, value, CommandFlags.None);
        public static Task<long> IncrementAsync(string key, long value, CommandFlags flags) =>
            db.StringIncrementAsync(key, value, flags);

        #endregion

        #region Decrement
        public static long Decrement(string key) =>
            Decrement(key, 1);
        public static long Decrement(string key, long value) =>
            Decrement(key, value, CommandFlags.None);
        public static long Decrement(string key, long value, CommandFlags flags) =>
            db.StringDecrement(key, value, flags);
        public static Task<long> DecrementAsync(string key) =>
            DecrementAsync(key, 1);
        public static Task<long> DecrementAsync(string key, long value) =>
            DecrementAsync(key, value, CommandFlags.None);
        public static Task<long> DecrementAsync(string key, long value, CommandFlags flags) =>
            db.StringDecrementAsync(key, value, flags);

        #endregion

        #region Remove
        public static bool Remove(string key) =>
            Remove(key, CommandFlags.None);
        public static bool Remove(string key, CommandFlags flags) =>
            db.KeyDelete(key, flags);

        public static Task<bool> RemoveAsync(string key) =>
            RemoveAsync(key, CommandFlags.None);
        public static Task<bool> RemoveAsync(string key, CommandFlags flags) =>
            db.KeyDeleteAsync(key, flags);
        #endregion
        #endregion

        #region PubSub
        public static void Publish(string channel, string message)
        {
            Publish(channel, message, CommandFlags.None);
        }

        public static void Publish(string channel, string message, CommandFlags flag)
        {
            subscriber.Publish(channel, message, flag);
        }

        public static async Task PublishAsync(string channel, string message)
        {
            await PublishAsync(channel, message, CommandFlags.None);
        }

        public static async Task PublishAsync(string channel, string message, CommandFlags flag)
        {
            await subscriber.PublishAsync(channel, message, flag);
        }

        public static void Subscribe(string channel, string type, Action<RedisSubscribeMessageModel> action)
        {
            Subscribe(channel, type, action, CommandFlags.None);
        }

        public static void Subscribe(string channel, string type, Action<RedisSubscribeMessageModel> action, CommandFlags flag)
        {
            subscriber.Subscribe(channel, (channelName, msg) =>
            {
                var model = new RedisSubscribeMessageModel
                {
                    Channnel = channelName,
                    SubscribeMessage = msg,
                    SubscribeType = type
                };
                action(model);
            }, flag);
        }

        public static async Task SubscribeAsync(string channel, string type, Action<RedisSubscribeMessageModel> action)
        {
            await SubscribeAsync(channel, type, action, CommandFlags.None);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="channel">channel</param>
        /// <param name="type">type</param>
        /// <param name="action">回掉委托</param>
        /// <param name="flag"></param>
        public static async Task SubscribeAsync(string channel, string type, Action<RedisSubscribeMessageModel> action, CommandFlags flag)
        {
            await subscriber.SubscribeAsync(channel, (channelName, msg) =>
            {
                var model = new RedisSubscribeMessageModel
                {
                    Channnel = channelName,
                    SubscribeMessage = msg,
                    SubscribeType = type
                };
                action(model);
            }, flag);
        }

        public static void Unsubscribe(string channel, CommandFlags flag = CommandFlags.None)
        {
            subscriber.Unsubscribe(channel, flags: flag);
        }

        public static void Unsubscribe(string channel, string type, Action<RedisSubscribeMessageModel> action, CommandFlags flag = CommandFlags.None)
        {
            subscriber.Unsubscribe(channel, (channelName, msg) =>
            {
                var model = new RedisSubscribeMessageModel
                {
                    Channnel = channelName,
                    SubscribeMessage = msg,
                    SubscribeType = type
                };
                action(model);
            }, flag);
        }

        public static async Task UnsubscribeAsync(string channel, CommandFlags flag = CommandFlags.None)
        {
            await subscriber.UnsubscribeAsync(channel, flags: flag);
        }

        public static async Task UnsubscribeAsync(string channel, string type, Action<RedisSubscribeMessageModel> action, CommandFlags flag = CommandFlags.None)
        {
            await subscriber.UnsubscribeAsync(channel, (channelName, msg) =>
            {
                var model = new RedisSubscribeMessageModel
                {
                    Channnel = channelName,
                    SubscribeMessage = msg,
                    SubscribeType = type
                };
                action(model);
            }, flag);
        }

        public static void UnsubscribeAll(CommandFlags flag = CommandFlags.None)
        {
            subscriber.UnsubscribeAll(flag);
        }

        public static async Task UnsubscribeAllAsync(CommandFlags flag = CommandFlags.None)
        {
            await subscriber.UnsubscribeAllAsync(flag);
        }
        #endregion
    }

    /// <summary>
    /// Redis订阅消息Model
    /// </summary>
    public class RedisSubscribeMessageModel
    {
        /// <summary>
        /// channel
        /// </summary>
        public string Channnel { get; set; }

        /// <summary>
        /// 订阅类型
        /// </summary>
        public string SubscribeType { get; set; }

        /// <summary>
        /// 订阅消息
        /// </summary>
        public string SubscribeMessage { get; set; }
    }
}