using Newtonsoft.Json;
using System.Collections.Generic;

namespace VstCommon.ModelResponses
{
    public partial class LoginResponse : VstBaseResponse
    {
        [JsonProperty("role")]
        public string? Role { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("timeout")]
        public int Timeout { get; set; } = 0;

        [JsonProperty("token")]
        public string? Token { get; set; }
    }

    public partial class LoginResponse : VstBaseResponse
    {
        [JsonProperty("staff")]
        public Dictionary<string, object>? Staffs { get; set; }

        [JsonProperty("device")]
        public Dictionary<string, object>? Devices { get; set; }
    }
}

