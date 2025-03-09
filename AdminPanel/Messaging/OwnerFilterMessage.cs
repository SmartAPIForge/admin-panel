namespace AdminPanel.Messaging;

public class OwnerFilterMessage
{
    public string Owner { get; }
    public OwnerFilterMessage(string owner) => Owner = owner;
}