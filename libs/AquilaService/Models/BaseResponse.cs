using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AquilaService.Models;

public class BaseResponse
{
    [JsonProperty("status")]
    [JsonPropertyName("status")]
    public object Status { get; set; }

    [JsonProperty("message")]
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public object? Value { get; set; }

    [JsonProperty("error")]
    [JsonPropertyName("error")]
    public object? Error { get; set; }

    [JsonProperty("warning")]
    [JsonPropertyName("warning")]
    public object? Warning { get; set; }
}

