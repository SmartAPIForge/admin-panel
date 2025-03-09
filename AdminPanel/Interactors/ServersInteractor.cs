using AdminPanel.Models;

namespace AdminPanel.Interactors;

public static class ServersInteractor
{
    public static async Task<List<Server>> LoadServersAsync()
    {
        return new();
    }
    
    public static async Task AddServerAsync(string ip, int port, string user, string password) {}
    
    public static async Task DeleteServerAsync(string ip, int port, string user) {}
}