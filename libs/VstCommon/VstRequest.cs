using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VstCommon
{
    public class VstRequest
    {
        [JsonProperty("token")]
        public string? Token { get; set; }

        [JsonProperty("value")]
        public object? Value { get; set; }
    }
}
