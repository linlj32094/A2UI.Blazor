using A2UI.Core.Messages;
using A2UI.Core.Processing;

namespace A2UI.Sample.BlazorServer.Services;

/// <summary>
/// A2A Client service that communicates with the Agent and processes UI responses
/// This follows the Google A2UI pattern: User Input → Agent → JSON → MessageProcessor → Render
/// </summary>
public class A2AClientService
{
    private readonly MockA2AAgent _agent;
    private readonly MessageProcessor _messageProcessor;
    private readonly ILogger<A2AClientService> _logger;

    public A2AClientService(
        MockA2AAgent agent,
        MessageProcessor messageProcessor,
        ILogger<A2AClientService> logger)
    {
        _agent = agent;
        _messageProcessor = messageProcessor;
        _logger = logger;
    }

    /// <summary>
    /// Sends a user query to the Agent and processes the A2UI response
    /// </summary>
    /// <param name="query">User's text query</param>
    /// <param name="surfaceId">The target surface ID to render on</param>
    /// <returns>The A2UI messages received from the agent</returns>
    public async Task<List<ServerToClientMessage>> SendQueryAsync(string query, string surfaceId = "demo-surface")
    {
        _logger.LogInformation($"[A2AClient] Sending query: {query}");

        try
        {
            // Step 1: Send query to Agent (simulating LLM)
            var messages = await _agent.ProcessQueryAsync(query);

            if (messages == null || messages.Count == 0)
            {
                _logger.LogWarning("[A2AClient] Agent returned no messages");
                return new List<ServerToClientMessage>();
            }

            _logger.LogInformation($"[A2AClient] Received {messages.Count} messages from agent");

            // Step 2: Replace surfaceId in all messages to match the requested one
            foreach (var message in messages)
            {
                if (message.BeginRendering != null)
                {
                    message.BeginRendering.SurfaceId = surfaceId;
                    _logger.LogInformation($"[A2AClient] Updated BeginRendering surfaceId to: {surfaceId}");
                }
                if (message.SurfaceUpdate != null)
                {
                    message.SurfaceUpdate.SurfaceId = surfaceId;
                }
                if (message.DataModelUpdate != null)
                {
                    message.DataModelUpdate.SurfaceId = surfaceId;
                }
            }

            // Step 3: Process messages through MessageProcessor
            // This builds the component tree and data model
            _messageProcessor.ProcessMessages(messages);

            _logger.LogInformation("[A2AClient] Messages processed successfully");

            return messages;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[A2AClient] Error processing query");
            throw;
        }
    }

    /// <summary>
    /// Sends a user action (button click, form submit, etc.) to the Agent
    /// </summary>
    /// <param name="actionName">The action name from the button/component</param>
    /// <param name="context">Action context data</param>
    /// <returns>The A2UI messages received from the agent</returns>
    public async Task<List<ServerToClientMessage>> SendActionAsync(string actionName, Dictionary<string, object>? context = null)
    {
        _logger.LogInformation($"[A2AClient] Sending action: {actionName}");

        // In a real implementation, this would format the action as A2UI ClientEvent
        // and send it to the agent. For now, we'll convert it to a query string
        var contextStr = context != null && context.Count > 0
            ? string.Join(", ", context.Select(kv => $"{kv.Key}={kv.Value}"))
            : "";

        var query = $"ACTION: {actionName}" + (string.IsNullOrEmpty(contextStr) ? "" : $" ({contextStr})");

        return await SendQueryAsync(query);
    }

    /// <summary>
    /// Clears all surfaces in the MessageProcessor
    /// </summary>
    public void ClearSurfaces()
    {
        _messageProcessor.ClearSurfaces();
    }
}

