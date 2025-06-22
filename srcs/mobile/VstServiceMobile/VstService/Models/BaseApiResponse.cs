namespace VstService.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// base response for return when call api
    /// </summary>
    public class BaseApiResponse
    {
        /// <summary>
        /// Gets or sets Status
        /// </summary>
        [JsonProperty("status")]
        public object? Status { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        [JsonProperty("value")]
        public object? Value { get; set; }
    }
}
