using System.Text.Json.Serialization;

namespace AdminPanel.Models;

public class ErrorReport
{
    [JsonPropertyName("service")]
    public string Service { get; set; }
    
    [JsonPropertyName("data")]
    public List<ErrorRecord> Data { get; set; }
}