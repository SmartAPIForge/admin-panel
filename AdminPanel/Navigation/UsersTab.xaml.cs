using System.Text.Json.Nodes;
using AdminPanel.Interactors;
using AdminPanel.Messaging;
using AdminPanel.Models;
using AdminPanel.Popups;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace AdminPanel.Navigation;

public partial class UsersTab : ContentPage
{
	private int _selectedRoleFilter = -1;
	private CancellationTokenSource _cts = new();
	
	public UsersTab()
	{
		InitializeComponent();
		LoadUsers();
		RoleFilterPicker.SelectedIndex = 0;
	}

	private async void LoadUsers()
	{
		try
		{
			var data = await UsersInteractor.GetUsersAsync(_cts.Token);

			MainThread.InvokeOnMainThreadAsync(() => UsersListView.ItemsSource = data);
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
            
			LoadUsers();
		}
		catch (TaskCanceledException) {}
	}
	
	private async void OnRoleFilterChanged(object sender, EventArgs e)
	{
		LoadUsers();
	}
	
	private async void OnAddUserClicked(object sender, EventArgs e)
	{
		var popup = new CreateUserPopup();
		var result = await Shell.Current.ShowPopupAsync(popup);
    
		if (result is bool confirmed && confirmed)
			await UsersInteractor.AddUserAsync(popup.EmailEntry.Text, popup.PasswordEntry.Text);
	}

	private async void OnDeleteUserClicked(object sender, EventArgs e)
	{
		if (sender is Button btn && btn.CommandParameter is string username)
			await UsersInteractor.DeleteUserAsync(username);
	}

	private async void OnViewProjectsClicked(object sender, EventArgs e)
	{
		if (sender is Button button)
		{
			var email = button.CommandParameter.ToString();
			Shell.Current.CurrentItem.CurrentItem = Shell.Current.CurrentItem.Items[2];
			
			WeakReferenceMessenger.Default.Send(new OwnerFilterMessage(email));
		}
	}
}