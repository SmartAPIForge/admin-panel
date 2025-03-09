using System.Text.Json.Serialization;

namespace AdminPanel.Models;

public class User 
{

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("role_id")] 
    public long Role { get; set; }
    
    [JsonPropertyName("username")]
    public string Username { get; set; }
}