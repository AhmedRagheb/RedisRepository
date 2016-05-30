using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisRepository
{
    public class RedisClient : IRedisClient
    {
        private readonly IDatabase _db;
        private readonly ConnectionMultiplexer _redis;

        public RedisClient()
        {
            const string configuration = "{0},abortConnect=false,defaultDatabase={1},ssl=false,ConnectTimeout={2},allowAdmin=true,connectRetry={3}";
            _redis = ConnectionMultiplexer
                .Connect(string.Format(configuration, RedisClientConfigurations.Url,
                    RedisClientConfigurations.DefaultDatabase, RedisClientConfigurations.ConnectTimeout,
                    RedisClientConfigurations.ConnectRetry));
            _redis.PreserveAsyncOrder = RedisClientConfigurations.PreserveAsyncOrder;
            _db = _redis.GetDatabase();
        }

        /// <summary>
        /// Remove value from redis
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _db.KeyDelete(key);
        }

        /// <summary>
        /// Remove multiple values from redis
        /// </summary>
        /// <param name="keys"></param>
        public void Remove(RedisKey[] keys)
        {
            _db.KeyDelete(keys);
        }

        /// <summary>
        /// Check if key is exist in redis
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return _db.KeyExists(key);
        }

        /// <summary>
        /// Dispose DB connection
        /// </summary>
        public void Stop()
        {
            _redis.Dispose();
        }

        /// <summary>
        /// Add new record in redis 
        /// </summary>
        /// <typeparam name="T">generic refrence type</typeparam>
        /// <param name="key">unique key of value</param>
        /// <param name="value">value of key of type T</param>
        /// <param name="expiresAt">time span of expiration</param>
        /// <returns>true or false</returns>
        public bool Add<T>(string key, T value, TimeSpan expiresAt) where T : class
        {
            var stringContent = SerializeContent(value);
            return _db.StringSet(key, stringContent, expiresAt);
        }

        /// <summary>
        /// Add new record in redis 
        /// </summary>
        /// <typeparam name="T">generic refrence type</typeparam>
        /// <param name="key">unique key of value</param>
        /// <param name="value">value of key of type object</param>
        /// <param name="expiresAt">time span of expiration</param>
        /// <returns>true or false</returns>
        public bool Add<T>(string key, object value, TimeSpan expiresAt) where T : class
        {
            var stringContent = SerializeContent(value);
            return _db.StringSet(key, stringContent, expiresAt);
        }

        /// <summary>
        /// Add new record in redis 
        /// </summary>
        /// <typeparam name="T">generic refrence type</typeparam>
        /// <param name="key">unique key of value</param>
        /// <param name="value">value of key of type T</param>
        /// <returns>true or false</returns>
        public bool Update<T>(string key, T value) where T : class
        {
            var stringContent = SerializeContent(value);
            return _db.StringSet(key, stringContent);
        }

        /// <summary>
        /// Get value of key, return one object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            try
            {
                RedisValue myString = _db.StringGet(key);
                if (myString.HasValue && !myString.IsNullOrEmpty)
                {
                    return DeserializeContent<T>(myString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                // Log Exception
                return null;
            }
        }

        /// <summary>
        /// Get all values of key, return list as you can send key in pattern format 
        /// (article:*) get all articles.  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string key) where T : class
        {
            try
            {
                var server = _redis.GetServer(host: RedisClientConfigurations.Url,
                                              port: RedisClientConfigurations.Port);
                var keys = server.Keys(_db.Database, key);
                var keyValues = _db.StringGet(keys.ToArray());

                var result = new List<T>();
                foreach (var redisValue in keyValues)
                {
                    if (redisValue.HasValue && !redisValue.IsNullOrEmpty)
                    {
                        var item = DeserializeContent<T>(redisValue);
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception)
            {
                // Log Exception
                return null;
            }
        }


        #region private

        // serialize and Deserialize content in separate functions as redis can save value as array of binary. 
        // so, any time you need to change the way of handling value, do it here.

        private string SerializeContent(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        private T DeserializeContent<T>(RedisValue myString)
        {
            return JsonConvert.DeserializeObject<T>(myString);
        }


        #endregion
    }
}
