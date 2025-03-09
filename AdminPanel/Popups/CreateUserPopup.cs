using System.Text.RegularExpressions;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Options;

namespace AdminPanel.Popups;

public class CreateUserPopup : Popup
{
    private readonly Label _emailErrorLabel = new()
    {
        Opacity = 0,
        TextColor = Colors.Firebrick,
        VerticalOptions = LayoutOptions.Start,
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
    
    private bool _isEmailValid = false;
    private bool _isPasswordValid = false;
    
    private readonly Regex _emailRegex = new(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9_-]+$", RegexOptions.Compiled);
    
    public Entry EmailEntry { get; } = new Entry { Keyboard = Keyboard.Email };
    
    public Entry PasswordEntry { get; } = new Entry();

    public Entry ConfirmPasswordEntry { get; } = new Entry();

    public CreateUserPopup()
    {
        this.Color = Colors.Transparent;

        var layout = new VerticalStackLayout
        {
            WidthRequest = 300,
            Padding = 10,
            Children =
            {
                new Label
                {
                    Text = "Создание пользователя",
                    HorizontalOptions = LayoutOptions.Center,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, 0, 0, 20),
                    VerticalOptions = LayoutOptions.Start
                },
                new Label { Text = "Почта"},
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
                _createButton
            }
        };

        _createButton.Clicked += OnCreateButtonClicked;
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
    
    private async void OnCreateButtonClicked(object sender, EventArgs e)
    {
        if (await ValidateAsync())
        {
            Close(true);
        }
    }

    private async Task<bool> ValidateAsync()
    {
        if (_isEmailValid && _isPasswordValid)
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
}