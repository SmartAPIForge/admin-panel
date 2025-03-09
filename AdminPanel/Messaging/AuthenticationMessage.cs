using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AdminPanel.Messaging;

public class AuthenticationMessage : RequestMessage<bool>
{
    public string Reason { get; set; }
}