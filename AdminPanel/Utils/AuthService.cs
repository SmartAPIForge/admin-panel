using System.Diagnostics;

namespace AdminPanel.Utils;

public static class AuthService
{
    private const string AccessTokenKey = "access_token";
    private const string RefreshTokenKey = "refresh_token";

    public static async Task SaveTokensAsync(string accessToken, string refreshToken)
    {
        try
        {
            await SecureStorage.SetAsync(AccessTokenKey, accessToken);
            await SecureStorage.SetAsync(RefreshTokenKey, refreshToken);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SecureStorage error: {ex.Message}");
        }
    }
    
    public static async Task<string> GetAccessTokenAsync() => await SecureStorage.GetAsync(AccessTokenKey);
    
    public static async Task<string> GetRefreshTokenAsync() => await SecureStorage.GetAsync(RefreshTokenKey);

    public static bool IsAuthenticated() => false;
    
    public static void ClearTokens()
    {
        SecureStorage.Remove(AccessTokenKey);
        SecureStorage.Remove(RefreshTokenKey);
    }
}