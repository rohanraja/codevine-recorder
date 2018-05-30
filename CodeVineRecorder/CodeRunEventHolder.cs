using System;
namespace CodeRecordHelpers
{
    public class CodeRunEventHolder
    {
        public CodeRunEventHolder()
        {
        }

		public Guid CodeRunID;

		public string EventType; // LineExec, LocalVarDec

		public string PayLoad;
    }
}
