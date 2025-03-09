using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdminPanel.Models;

public class Project 
{
    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("data")]
    public JsonDocument Data { get; set; }
}