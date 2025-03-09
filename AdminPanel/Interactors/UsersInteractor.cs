using AdminPanel.Models;

namespace AdminPanel.Interactors;

public static class UsersInteractor
{
    public static async Task<List<User>> GetUsersAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        return new();
    }

    public static async Task DeleteUserAsync(string username)
    {
        
    }

    public static async Task AddUserAsync(string email, string password)
    {
        
    }
}