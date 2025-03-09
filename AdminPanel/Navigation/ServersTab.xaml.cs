using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.Interactors;
using AdminPanel.Models;
using AdminPanel.Popups;
using CommunityToolkit.Maui.Views;

namespace AdminPanel.Navigation;

public partial class ServersTab : ContentPage
{
    public ServersTab()
    {
        InitializeComponent();
        LoadServers();
    }

    private async void LoadServers()
    {
        var data = await ServersInteractor.LoadServersAsync();

        MainThread.InvokeOnMainThreadAsync(() => ServersListView.ItemsSource = data);
    }

    private async void OnAddServerClicked(object sender, EventArgs e)
    {
        var popup = new CreateServerPopup();
        var res = await Shell.Current.ShowPopupAsync(popup);
        
        if (res is bool confirmed && confirmed)
            await ServersInteractor.AddServerAsync(popup.IpEntry.Text, int.Parse(popup.PortEntry.Text), popup.EmailEntry.Text, popup.PasswordEntry.Text);
    }

    private async void OnDeleteServerClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Server server)
            await ServersInteractor.DeleteServerAsync(server.Ip, server.Port, server.User);
    }
}