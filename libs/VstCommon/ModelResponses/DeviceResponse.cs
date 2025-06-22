using Newtonsoft.Json;

namespace VstCommon.ModelResponses
{
    public class DeviceResponse
    {
        [JsonProperty("_id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("addr")]
        public string? Addr { get; set; }

        [JsonProperty("model")]
        public string? Model { get; set; }

        [JsonProperty("version")]
        public string? Version { get; set; }
    }

}