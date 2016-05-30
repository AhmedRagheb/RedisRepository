using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace RedisRepository
{
    public interface IRedisClient
    {
        bool Remove(string key);
        void Remove(RedisKey[] keys);
        bool Exists(string key);
        void Stop();
        bool Add<T>(string key, object value, TimeSpan expiresAt) where T : class;
        bool Add<T>(string key, T value, TimeSpan expiresAt) where T : class;
        bool Add<T>(string key, List<T> value, TimeSpan expiresAt) where T : class;
        T Get<T>(string key) where T : class;
        List<T> Search<T>(string key) where T : class;
    }
}
