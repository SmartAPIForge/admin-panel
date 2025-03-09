using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        if (Application.Current?.Windows[0] is Window window)
        {
            window.Page = new AppShell();
        }
    }
}