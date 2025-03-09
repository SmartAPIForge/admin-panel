using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdminPanel.Models;

public class Server
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("ip")]
    public string Ip { get; set; }
    
    [JsonPropertyName("port")]
    public int Port { get; set; }
    
    [JsonPropertyName("user")]
    public string User { get; set; }
}