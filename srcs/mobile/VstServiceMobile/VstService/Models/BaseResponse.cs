using Newtonsoft.Json;

namespace VstService.Models
{
    public class BaseResponse
    {
        [JsonProperty("status")]
        public object Status { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("value")]
        public object? Value { get; set; }

        [JsonProperty("error")]
        public object? Error { get; set; }

        [JsonProperty("warning")]
        public object? Warning { get; set; }
    }

}
