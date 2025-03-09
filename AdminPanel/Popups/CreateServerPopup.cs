using CommunityToolkit.Maui.Views;
using System.Text.RegularExpressions;

namespace AdminPanel.Popups;

public class CreateServerPopup : Popup
{
    private readonly Label _ipErrorLabel = new()
    {
        Opacity = 0,
        TextColor = Colors.Firebrick,
        VerticalOptions = LayoutOptions.Start,
        FontSize = 12,
    };
    
    private readonly Label _portErrorLabel = new()
    {
        Opacity = 0,
        TextColor = Colors.Firebrick,
        FontSize = 12,
    };
    
    private readonly Label _emailErrorLabel = new()
    {
        Opacity = 0,
        TextColor = Colors.Firebrick,
        FontSize = 12,
    };
    
    private readonly Label _passwordErrorLabel = new()
    {
        Opacity = 0,
        TextColor = Colors.Firebrick,
        FontSize = 12,
    };

    private readonly Label _errorLabel = new()
    {
        Opacity = 0,
        TextColor = Colors.Firebrick,
        FontSize = 16,
        Margin = new Thickness(0, 10, 0, 0)
    };
    
    private readonly Button _createButton = new()
    {
        Text = "Создать",
        Margin = new Thickness(0, 30, 0, 0),
        IsEnabled = false,
    };

    private bool _isIpValid = false;
    private bool _isPortValid = false;
    private bool _isEmailValid = false;
    private bool _isPasswordValid = false;
    
    private readonly Regex _emailRegex = new(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9_-]+$", RegexOptions.Compiled);
    private readonly Regex _ipRegex = new(@"^((25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\.){3}(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$", RegexOptions.Compiled);

    public Entry IpEntry { get; } = new();

    public Entry PortEntry { get; } = new();
    
    public Entry EmailEntry { get; } = new();
    
    public Entry PasswordEntry { get; } = new();

    public Entry ConfirmPasswordEntry { get; } = new();

    

    public CreateServerPopup()
    {
        this.Color = Colors.Transparent;

        var layout = new VerticalStackLayout
        {
            Padding = 30,
            WidthRequest = 300,
            Children =
            {
                new Label
                {
                    Text = "Добавление сервера",
                    HorizontalOptions = LayoutOptions.Center,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, 0, 0, 20),
                    VerticalOptions = LayoutOptions.Start
                },
                new Label {Text = "IP"},
                IpEntry,
                _ipErrorLabel,
                new Label
                {
                    Text = "Порт",
                    Margin = new Thickness(0, 20, 0, 0),
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.End
                },
                PortEntry,
                _portErrorLabel,
                new Label
                {
                    Text = "Почта",
                    Margin = new Thickness(0, 20, 0, 0),
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.End
                },
                EmailEntry,
                _emailErrorLabel,
                new Label
                {
                    Text = "Пароль",
                    Margin = new Thickness(0, 20, 0, 0),
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.End
                },
                PasswordEntry,
                _passwordErrorLabel,
                new Label
                {
                    Text = "Подтвердите пароль",
                    Margin = new Thickness(0, 20, 0, 0),
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.End
                },
                ConfirmPasswordEntry,
                _errorLabel,
                _createButton
            }   
        };

        _createButton.Clicked += OnCreateButtonClicked;
        IpEntry.TextChanged += ValidateIp;
        PortEntry.TextChanged += ValidatePort;
        EmailEntry.TextChanged += ValidateEmail;
        PasswordEntry.TextChanged += ValidatePassword;
        ConfirmPasswordEntry.TextChanged += ValidatePassword;
        
        Content = new Frame
        {
            Content = layout,
            BackgroundColor = Color.FromArgb("#1f1f1f"),
            CornerRadius = 20,
        };
    }

    private async void ValidateIp(object sender, TextChangedEventArgs e)
    {
        if (!_ipRegex.IsMatch(IpEntry.Text ?? string.Empty))
        {
            _ipErrorLabel.Text = "Некорректный ip";
            _ipErrorLabel.Opacity = 1;
            _isIpValid = false;
        }
        else
        {
            _ipErrorLabel.Opacity = 0;
            _isIpValid = true;
        }
        await ValidateAsync();
    }

    private async void ValidatePort(object sender, TextChangedEventArgs e)
    {
        if (!int.TryParse(PortEntry.Text, out int port) || port < 1 || port > 65535)
        {
            _portErrorLabel.Text = "Некорректный порт";
            _portErrorLabel.Opacity = 1;
            _isPortValid = false;
        }
        else
        {
            _portErrorLabel.Opacity = 0;
            _isPortValid = true;
        }
        await ValidateAsync();
    }

    private async void ValidateEmail(object sender, TextChangedEventArgs e)
    {
        if (!_emailRegex.IsMatch(EmailEntry.Text ?? string.Empty))
        {
            _emailErrorLabel.Text = "Некорректная почта";
            _emailErrorLabel.Opacity = 1;
            _isEmailValid = false;
        }
        else
        {
            _emailErrorLabel.Opacity = 0;
            _isEmailValid = true;
        }
        await ValidateAsync();
    }

    private async void ValidatePassword(object sender, TextChangedEventArgs e)
    {
        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            _passwordErrorLabel.Text = "Пароли не совпадают";
            _passwordErrorLabel.Opacity = 1;
            _isPasswordValid = false;
        }
        else
        {
            _passwordErrorLabel.Opacity = 0;
            _isPasswordValid = true;
        }
        await ValidateAsync();
    }

    private async Task<bool> ValidateAsync()
    {
        if (_isEmailValid && _isPasswordValid && _isIpValid && _isPortValid)
        {
            _createButton.IsEnabled = true;
            return true;
        }
        else
        {
            _createButton.IsEnabled = false;
            return false;
        }
    }

    private async void OnCreateButtonClicked(object sender, EventArgs e)
    {
        if (await ValidateAsync())
        {
            Close(true);
        }
    }
}