namespace AquilaService.Models;

using System.Text.Json.Serialization;
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
    [JsonPropertyName("status")]
    public object? Status { get; set; }

    /// <summary>
    /// Gets or sets Value
    /// </summary>
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public object? Value { get; set; }
}
