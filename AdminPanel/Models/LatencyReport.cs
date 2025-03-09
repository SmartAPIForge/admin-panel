using System.Text.Json.Serialization;

namespace AdminPanel.Models;

public class LatencyReport
{
    [JsonPropertyName("service")]
    public string Service { get; set; }
    
    [JsonPropertyName("data")]
    public List<LatencyRecord> Data { get; set; }
}