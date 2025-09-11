using System.Text.Json.Serialization;

namespace JustBeeWeb.Serialization;

/// <summary>
/// JSON source generator context for email serialization with trimming support
/// </summary>
[JsonSerializable(typeof(EmailData))]
[JsonSerializable(typeof(EmailSender))]
[JsonSerializable(typeof(EmailRecipient))]
[JsonSerializable(typeof(EmailRecipient[]))]
[JsonSourceGenerationOptions(WriteIndented = false)]
public partial class EmailSerializationContext : JsonSerializerContext
{
}

/// <summary>
/// Email data structure for serialization
/// </summary>
public class EmailData
{
    public EmailSender? Sender { get; set; }
    public EmailRecipient[]? To { get; set; }
    public string? Subject { get; set; }
    public string? HtmlContent { get; set; }
}

/// <summary>
/// Email sender information
/// </summary>
public class EmailSender
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}

/// <summary>
/// Email recipient information
/// </summary>
public class EmailRecipient
{
    public string? Email { get; set; }
    public string? Name { get; set; }
}