using System;
using Newtonsoft.Json;

namespace CodeRecordHelpers
{
    internal class JsonHelper
    {
		private static JsonHelper _instance;

		public static JsonHelper Instance()
        {
            if (_instance == null)
				_instance = new JsonHelper();

            return _instance;
        }

		internal string ToJSON(object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}
	}
}