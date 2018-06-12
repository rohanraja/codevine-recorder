using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace CodeRecordHelpers
{

	public class CodeHooks
    {
		private static CodeHooks _instance;

		public Guid CodeRunID = Guid.NewGuid();
		private HookHelpers hookHelpers;
     private JsonHelper jsonHelper;

		static int ticks = 0;
        
		public static string Now()
		{
			ticks++;
			return ticks.ToString();
		}

        private static string getThreadID()
        {
            return Thread.CurrentThread.ManagedThreadId.ToString() ; 
        }

		public static CodeHooks Instance()
		{
			if (_instance == null)
				_instance = new CodeHooks(new MessageDispatcher());
			
			return _instance;
		}

		public CodeHooks(IMessageDispatcher dispatcher)
        {
			hookHelpers = new HookHelpers(dispatcher);
      jsonHelper = new JsonHelper();
        }



		public void AddSourceFile(string relativeFilePath, string code)
        {
            var eventType = "ADD_SOURCE_FILE";


            var payload = new List<string>() { };
			payload.Add(relativeFilePath);
			payload.Add(code);

			hookHelpers.DispatchCodeRunEvent(CodeRunID.ToString(), payload, eventType);
        }

		public Guid OnMethodEnter(string relativeFilePath, string methodName)
        {
            var eventType = "METHOD_ENTER";


			var mrid = Guid.NewGuid();
            var payload = new List<string>() { };
            payload.Add(mrid.ToString());
            payload.Add(relativeFilePath);
            payload.Add(methodName);
			payload.Add(getThreadID());

            hookHelpers.DispatchCodeRunEvent(CodeRunID.ToString(), payload, eventType);

			return mrid;
        }

		public void LogLineRun(Guid mrid, int lineNo, string timeStamp, string methodRunningState="RUNNING")
        {

            //var payload = new LineExecPayloadHolder(mrid, lineNo, timeStamp);
            var eventType = "LINE_EXEC";

			var payload = new List<string>() { };
			payload.Add(mrid.ToString());
			payload.Add(lineNo.ToString());
			payload.Add(timeStamp);
			payload.Add(methodRunningState);


			hookHelpers.DispatchCodeRunEvent(CodeRunID.ToString(), payload, eventType);

        }
        
		public void SendFieldUpdate(int clrId, string varName, string varType, string className, object newVal, string timeStamp)
        {
			SendVarUpdate(clrId.ToString(), varName, false, className, newVal, timeStamp);
        }

		public void LocalVarUpdate(Guid mrid, string varName, string className, object newVal, string timeStamp)
        {
			SendVarUpdate(mrid.ToString(), varName, true, className, newVal, timeStamp);
        }

		public void SendVarUpdate(string contId, string varName, bool isLocal, string className, object newVal, string timeStamp)
		{

			//var payload = new LineExecPayloadHolder(mrid, lineNo, timeStamp);
			var eventType = "SEND_VAR_UPDATE";

			string newValStr = "null";
			string varType = "NULL";

			if (newVal != null)
			{
				if (IsValueType(newVal))
				{
					newValStr = newVal.ToString();
					varType = "VALUE";
				}
				else
				{
					if (IsCodeVineClass(newVal))
					{
						newValStr = newVal.GetHashCode().ToString();
						varType = "INTERNAL_CLASS";
					}
					else
					{
						newValStr = jsonHelper.ToJSON(newVal);
						varType = "EXTERNAL_CLASS";
					}

				}
			}


			var payload = new List<string>() { };
			payload.Add(contId);
			payload.Add(varName);
			payload.Add(varType);
			payload.Add(className);
			payload.Add(newValStr);
			payload.Add(timeStamp);
			payload.Add(isLocal.ToString());


			hookHelpers.DispatchCodeRunEvent(CodeRunID.ToString(), payload, eventType);
		}



        public bool IsCodeVineClass(object target)
		{
			var property = target.GetType().GetProperty("CodeVine_ClrInstanceId");
			return property != null;
		}

        public bool IsValueType(object target)
		{
			var type = target.GetType();

			bool outP = type.IsValueType;
			if (outP)
				return true;

			if (type.Name.ToLower() == "string" && type.Namespace.ToLower() == "system")
				return true;

			return false;
		}


	}
}
