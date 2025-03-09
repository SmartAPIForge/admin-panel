using CommunityToolkit.Mvvm.Messaging;
using AdminPanel.Messaging;
using AdminPanel.Utils;

namespace AdminPanel;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<AuthenticationMessage>(
            this,
            (r, m) =>
            {
                if (!m.Response)
                    MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"//LoginPage"));
            }
        );
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        return new Window(AuthService.IsAuthenticated() ? new AppShell() : new LoginPage());
    }
}