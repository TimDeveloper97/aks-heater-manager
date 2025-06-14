using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

namespace AquilaService.Models;

public class BaseObject
{
    /// <summary>
    /// guid id
    /// </summary>
    /// <example>0ab824a8-8452-40a5-85a2-0cefaf56cbec</example>
    [Required]
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonProperty("createTime")]
    [JsonPropertyName("createTime")]
    public DateTime CreateTime { get; set; }

    [JsonProperty("updateTime")]
    [JsonPropertyName("updateTime")]
    public DateTime UpdateTime { get; set; } = DateTime.Now;

    [JsonProperty("createBy")]
    [JsonPropertyName("createBy")]
    public string? CreateBy { get; set; } = "Anonymous";

    [JsonProperty("updateBy")]
    [JsonPropertyName("updateBy")]
    public string? UpdateBy { get; set; } = "Anonymous";

    public BaseObject()
    {
        Id = Guid.NewGuid().ToString();
        CreateTime = DateTime.Now;
    }
}

public class Common
{
    public static string PATH_DOCUMENT = AppDomain.CurrentDomain.BaseDirectory
        + $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
}
