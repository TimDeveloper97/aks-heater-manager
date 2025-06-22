using Newtonsoft.Json;

namespace VstCommon
{
    public class VstResponse
    {
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("value")]
        public object? Value { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }
    }

    public class VstBaseResponse
    {
        [JsonProperty("_id")]
        public string? Id { get; set; }
    }
}
