using System.Text.Json.Serialization;

namespace AdminPanel.Models;

public class ErrorRecord
{
    private DateTime _hour;
    
    [JsonPropertyName("hour")]
    public DateTime Hour { 
        get => _hour;
        set => _hour = value.ToLocalTime();
    }
    
    [JsonPropertyName("error_percentage")]
    public double ErrorPercentage { get; set; }
}