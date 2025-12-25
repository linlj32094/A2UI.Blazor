using System.Text.Json.Serialization;

namespace A2UI.Core.Messages;

/// <summary>
/// Represents a message sent from the client to the server in the A2UI protocol.
/// Must contain exactly ONE of: UserAction or Error.
/// </summary>
public class ClientToServerMessage
{
    [JsonPropertyName("userAction")]
    public UserActionMessage? UserAction { get; set; }

    [JsonPropertyName("error")]
    public Dictionary<string, object>? Error { get; set; }
}

/// <summary>
/// Reports a user-initiated action from a component.
/// </summary>
public class UserActionMessage
{
    /// <summary>
    /// The name of the action, taken from the component's action.name property.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The id of the surface where the event originated.
    /// </summary>
    [JsonPropertyName("surfaceId")]
    public required string SurfaceId { get; set; }

    /// <summary>
    /// The id of the component that triggered the event.
    /// </summary>
    [JsonPropertyName("sourceComponentId")]
    public required string SourceComponentId { get; set; }

    /// <summary>
    /// An ISO 8601 timestamp of when the event occurred.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public required DateTime Timestamp { get; set; }

    /// <summary>
    /// A JSON object containing the key-value pairs from the component's action.context,
    /// after resolving all data bindings.
    /// </summary>
    [JsonPropertyName("context")]
    public required Dictionary<string, object> Context { get; set; }
}

