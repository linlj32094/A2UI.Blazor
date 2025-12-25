using System.Text;
using System.Text.Json;
using A2UI.Core.Messages;

namespace A2UI.AgentSDK;

/// <summary>
/// Serializes A2UI messages to JSONL format.
/// </summary>
public class A2UIMessageSerializer
{
    private readonly JsonSerializerOptions _options;

    public A2UIMessageSerializer()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    /// <summary>
    /// Serializes a single message to JSON.
    /// </summary>
    public string SerializeMessage(ServerToClientMessage message)
    {
        return JsonSerializer.Serialize(message, _options);
    }

    /// <summary>
    /// Serializes multiple messages to JSONL format.
    /// </summary>
    public string SerializeMessages(IEnumerable<ServerToClientMessage> messages)
    {
        var sb = new StringBuilder();
        foreach (var message in messages)
        {
            sb.AppendLine(SerializeMessage(message));
        }
        return sb.ToString();
    }

    /// <summary>
    /// Writes messages to a stream as JSONL.
    /// </summary>
    public async Task WriteMessagesAsync(
        Stream stream, 
        IEnumerable<ServerToClientMessage> messages,
        CancellationToken cancellationToken = default)
    {
        using var writer = new StreamWriter(stream, leaveOpen: true);
        
        foreach (var message in messages)
        {
            var json = SerializeMessage(message);
            await writer.WriteLineAsync(json.AsMemory(), cancellationToken);
            await writer.FlushAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Serializes a client message.
    /// </summary>
    public string SerializeClientMessage(ClientToServerMessage message)
    {
        return JsonSerializer.Serialize(message, _options);
    }
}

