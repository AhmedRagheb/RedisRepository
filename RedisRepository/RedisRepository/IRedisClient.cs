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
        bool Update<T>(string key, T value) where T : class;
        T Get<T>(string key) where T : class;
        List<T> GetList<T>(string key) where T : class;
    }
}

