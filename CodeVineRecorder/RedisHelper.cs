using System;
using StackExchange.Redis;

namespace CodeRecordHelpers
{
	public class RedisHelper
    {
		private ConnectionMultiplexer redis;

		public RedisHelper()
        {
        }

		public static RedisHelper GetConnectedRedisHelper()
		{
			var redisHelper = new RedisHelper();
            redisHelper.Connect();
			return redisHelper;
		}

		public void AddToQueue(string key, string jsonVal)
		{
			var db = redis.GetDatabase();
			db.ListRightPushAsync(key, jsonVal);
		}

		public void Connect()
		{

			redis = ConnectionMultiplexer.Connect("algomuse.com:6379");
    	}

        public long ListCount(string key)
        {
            return redis.GetDatabase().ListLength(key);
        }

        public void DelKey(string key)
        {
            redis.GetDatabase().KeyDelete(key);
        }

        public void Dispose()
		{
			redis.Dispose();
		}

		public bool IsConnected()
		{
			return redis.IsConnected;
		}
	}
}
