using Newtonsoft.Json;

namespace VstCommon.Models
{
    public class LoginRequest
    {
        [JsonProperty("userName")]
        public string? Username { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; }
    }

}

