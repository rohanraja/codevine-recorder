using System;
using System.Threading;

namespace CodeRecordHelpers
{

    public class MessageDispatcher : IMessageDispatcher
	{
		RedisHelper redisHelper;

        public static void WaitToFinish()
        {
            Thread.Sleep(500);
        }

        public MessageDispatcher()
        {
			redisHelper = RedisHelper.GetConnectedRedisHelper();
            ThreadPool.SetMinThreads(10, 10);
        }

		public void DispatchMessage(RedisMessage msg)
		{
            //ThreadPool.QueueUserWorkItem(AsyncQueue, msg);
            AsyncQueue(msg);
		}

        private void AsyncQueue(object state)
        {
            var msg = state as RedisMessage;
            if (msg == null)
                return;

			string key = msg.GetKey();

			redisHelper.AddToQueue(key, msg.GetMessage());
        }
    }
}
