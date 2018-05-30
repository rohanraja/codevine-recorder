using System;
using System.Collections.Generic;

namespace CodeRecordHelpers
{
    public class HookHelpers
    {
		private IMessageDispatcher messageDispatcher;
        private JsonHelper jsonHelper;

		public HookHelpers(IMessageDispatcher dispatcher)
        {
            messageDispatcher = dispatcher;
            jsonHelper = new JsonHelper();
        }

		public void DispatchCodeRunEvent(string crid, object payload, string eventType)
        {
            string Payload = jsonHelper.ToJSON(payload);

            var strArgs = new List<string>() { };
			strArgs.Add(crid);
            strArgs.Add(eventType);
            strArgs.Add(Payload);

            string key = "CODE_RUN_EVENTS";
            key = string.Format("resque:queue:{0}", key);
            string railsClassName = "CodeRunEventProcessor";

            EnqueueResqueMessage(key, railsClassName, strArgs);
        }

        private void EnqueueResqueMessage(string key, string railsClassName, List<string> strArgs)
        {

            Console.WriteLine("Dispatching Message: " + key);
            ResqueMessage remsg = new ResqueMessage();
            remsg.Class = railsClassName;
            remsg.args = strArgs;
            string message = jsonHelper.ToJSON(remsg);

            RedisMessage msg = new RedisMessage(key, message);
            messageDispatcher.DispatchMessage(msg);
        }    }
}
