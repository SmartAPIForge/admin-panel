using AdminPanel.Models;

namespace AdminPanel.Interactors;

public static class AnalyticsDataInteractor
{
    public static async Task<List<LatencyRecord>> GetLatencyRecordsAsync(string serviceName)
    {
        return new();
    }

    public static async Task<List<ErrorRecord>> GetErrorRecordsAsync(string serviceName)
    {
        return new();
    }
}