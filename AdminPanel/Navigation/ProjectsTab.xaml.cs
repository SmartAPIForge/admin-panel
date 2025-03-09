using AdminPanel.Messaging;
using AdminPanel.Models;
using AdminPanel.Interactors;
using CommunityToolkit.Mvvm.Messaging;

namespace AdminPanel.Navigation;

public partial class ProjectsTab : ContentPage
{
    private CancellationTokenSource _cts = new CancellationTokenSource();
    
    public string OwnerFilter { get; set; }
    
    public ProjectsTab()
    {
        InitializeComponent();
        StatusFilterPicker.SelectedIndex = 0;
        LoadProjects();

        WeakReferenceMessenger.Default.Register<OwnerFilterMessage>(
            this,
            (r, m) =>
            {
                OwnerFilterEntry.Text = m.Owner;
                LoadProjects();
            }
        );
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!string.IsNullOrEmpty(OwnerFilter))
        {
            OwnerFilterEntry.Text = OwnerFilter;
        }
    }

    ~ProjectsTab()
    {
        WeakReferenceMessenger.Default.Unregister<OwnerFilterMessage>(this);
    }

    private async void OnOwnerFilterChanged(object sender, TextChangedEventArgs e)
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            await Task.Delay(500, _cts.Token);
            
            LoadProjects();
        }
        catch (TaskCanceledException) {}
    }

    private async void LoadProjects()
    {
        try
        {
            var data = await ProjectsInteractor.LoadProjectsAsync(
                SearchBar.Text,
                OwnerFilterEntry.Text,
                StatusFilterPicker.SelectedIndex switch
                {
                    1 => "NEW",
                    2 => "GENERATE_PENDING",
                    3 => "GENERATE_SUCCESS",
                    4 => "GENERATE_FAIL",
                    5 => "DEPLOY_PENDING",
                    6 => "DEPLOY_SUCCESS",
                    7 => "DEPLOY_FAIL",
                    8 => "RUNNING",
                    9 => "STOPPED",
                    10 => "FAILED",
                    _ => string.Empty
                },
                _cts.Token
            );

            MainThread.InvokeOnMainThreadAsync(
                () =>
                    ProjectsListView.ItemsSource = data
            );
        }
        catch (TaskCanceledException) {}
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            await Task.Delay(500, _cts.Token);
            
            LoadProjects();
        }
        catch (TaskCanceledException) {}
    }

    private async void OnStatusFilterChanged(object sender, EventArgs e)
    {
        LoadProjects();
    }

    private async void OnStopClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Project project)
            await ProjectsInteractor.StopProjectAsync(project.Owner, project.Name);
    }
    
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Project project)
            await ProjectsInteractor.DeleteProjectAsync(project.Owner, project.Name);
    }
}