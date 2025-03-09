using AdminPanel.Models;

namespace AdminPanel.Interactors;

public static class ProjectsInteractor
{
    public static async Task<List<Project>> LoadProjectsAsync(string name, string owner, string status, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        return new();
    }

    public static async Task DeleteProjectAsync(string owner, string name)
    {
        
    }

    public static async Task StopProjectAsync(string owner, string name)
    {
        
    }
}