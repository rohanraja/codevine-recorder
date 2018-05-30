using System;
namespace CodeRecordHelpers
{

    public class MessageDispatcher : IMessageDispatcher
	{
		RedisHelper redisHelper;

        public MessageDispatcher()
        {
			redisHelper = RedisHelper.GetConnectedRedisHelper();
        }

		public void DispatchMessage(RedisMessage msg)
		{
			string key = msg.GetKey();

			redisHelper.AddToQueue(key, msg.GetMessage());
		}

    }
}
