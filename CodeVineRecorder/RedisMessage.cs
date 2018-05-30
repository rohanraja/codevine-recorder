using System;
namespace CodeRecordHelpers
{
    public class RedisMessage
    {
		private string key;
		private string message;

		public RedisMessage(string key, string message)
		{
			this.key = key;
			this.message = message;
		}

		public string GetKey()
		{
			return key;
		}

		public string GetMessage()
		{
			return message;
		}
	}
}
