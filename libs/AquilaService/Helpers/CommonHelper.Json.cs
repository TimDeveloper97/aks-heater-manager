using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;

namespace AquilaService;

public static class JsonHelper
{
    /// <summary>
    /// check type json is excel or sheet
    /// </summary>
    /// <param name="json">json</param>
    public static object? DetermineJsonType(Dictionary<string, Type> dictTypes, string json)
    {
        try
        {
            // Parse the JSON with JsonDocument to inspect its properties

            using (System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(json))
            {
                System.Text.Json.JsonElement root = document.RootElement;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    object? obj = null;
                    // Check for a property that distinguishes class A or class B
                    foreach (var (prop, type) in dictTypes)
                    {
                        if (root.TryGetProperty(prop, out _))
                        {
                            obj = JsonConvert.DeserializeObject(json, type);
                            break;
                        }
                    }

                    return obj;
                }
                else if (root.ValueKind == JsonValueKind.Array)
                {
                    // If JSON root is an array, iterate over each element
                    List<object>? objs = new List<object>();
                    foreach (var element in root.EnumerateArray())
                    {
                        string elementJson = element.GetRawText(); // Serialize the element back to JSON string
                        var obj = TryDeserialize(element, elementJson, dictTypes);
                        objs?.Add(obj);
                    }

                    return objs;
                }
                else
                {
                    throw new Exception("Unsupported JSON root type: " + root.ValueKind);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Invalid JSON. " + ex.Message);
        }
    }

    static object TryDeserialize(JsonElement element, string json, Dictionary<string, Type> dictTypes)
    {
        // Ensure the element is an object before checking for properties
        if (element.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        // Iterate through the property/type mappings
        foreach (var kvp in dictTypes)
        {
            if (element.TryGetProperty(kvp.Key, out _))
            {
                // Deserialize using Newtonsoft.Json
                return JsonConvert.DeserializeObject(json, kvp.Value);
            }
        }
        return null;
    }
}
