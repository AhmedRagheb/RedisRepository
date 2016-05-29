using System;
using System.Collections.Generic;
using Sarmady.Models;
using StackExchange.Redis;

namespace Sarmady.Rahawan.Engine
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

        CacheEngineResponse<T> RedisRequest<T>(List<int> ids, string key, Func<T, bool?> filterBy = null)
            where T : class, IDocument<T>;

        List<T> Search<T>(string key) where T : class;
    }
}
