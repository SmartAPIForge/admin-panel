using System.Text.Json.Serialization;

namespace AdminPanel.Models;

public class LatencyRecord
{
    private DateTime _hour;
    
    [JsonPropertyName("hour")]
    public DateTime Hour { 
        get => _hour;
        set => _hour = value.ToLocalTime();
    }
    
    [JsonPropertyName("average_response_time")]
    public double AverageResponseTime { get; set; }
}