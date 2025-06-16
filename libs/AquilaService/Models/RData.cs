using Newtonsoft.Json;

namespace AquilaService.Models;

/// <summary>
/// Request data
/// </summary>
public class RData
{
    /// <summary>
    /// Gets or sets endpoint
    /// </summary>
    public string? Endpoint { get; set; }

    /// <summary>
    /// Gets or sets Data
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Gets or sets  media type
    /// </summary>
    public string MediaType { get; set; } = "application/json";

    /// <summary>
    /// Gets or sets  cancellation token
    /// </summary>
    public CancellationToken? Token { get; set; } = null;

    /// <summary>
    /// Gets payload
    /// </summary>
    public string Payload => JsonConvert.SerializeObject(Data);
}
