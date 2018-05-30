using System.Collections.Generic;
using Newtonsoft.Json;

namespace CodeRecordHelpers
{
    public class ResqueMessage
    {

        public List<string> args = new List<string>() { };

        [JsonProperty("class")]
        public string Class;
    }
}
