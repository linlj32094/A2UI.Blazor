using A2UI.AgentSDK.Builders;
using A2UI.Core;
using A2UI.Core.Messages;

namespace A2UI.AgentSDK;

/// <summary>
/// Quick helpers for creating common A2UI patterns.
/// </summary>
public static class A2UIQuickStart
{
    /// <summary>
    /// Creates a simple text card.
    /// </summary>
    public static List<ServerToClientMessage> CreateTextCard(
        string surfaceId,
        string title,
        string body)
    {
        return new SurfaceBuilder(surfaceId)
            .AddColumn("root", col => col
                .AddChild("card"))
            .AddCard("card", card => card
                .WithChild("content"))
            .AddColumn("content", col => col
                .AddChild("title")
                .AddChild("body"))
            .AddText("title", text => text
                .WithText(title)
                .WithUsageHint(A2UIConstants.TextUsageHints.H3))
            .AddText("body", text => text
                .WithText(body))
            .WithRoot("root")
            .Build();
    }

    /// <summary>
    /// Creates a button with text.
    /// </summary>
    public static List<ServerToClientMessage> CreateButton(
        string surfaceId,
        string buttonId,
        string label,
        string actionName,
        bool isPrimary = false)
    {
        var builder = new SurfaceBuilder(surfaceId)
            .AddButton(buttonId, btn =>
            {
                btn.WithChild($"{buttonId}_text")
                   .WithAction(actionName);
                if (isPrimary) btn.AsPrimary();
            })
            .AddText($"{buttonId}_text", text => text
                .WithText(label))
            .WithRoot(buttonId);

        return builder.Build();
    }

    /// <summary>
    /// Creates a form with a text input and submit button.
    /// </summary>
    public static List<ServerToClientMessage> CreateSimpleForm(
        string surfaceId,
        string title,
        string submitLabel,
        string submitActionName)
    {
        return new SurfaceBuilder(surfaceId)
            .AddColumn("root", col => col
                .AddChild("title")
                .AddChild("submit_button"))
            .AddText("title", text => text
                .WithText(title)
                .WithUsageHint(A2UIConstants.TextUsageHints.H2))
            .AddButton("submit_button", btn => btn
                .WithChild("submit_text")
                .WithAction(submitActionName)
                .AsPrimary())
            .AddText("submit_text", text => text
                .WithText(submitLabel))
            .WithRoot("root")
            .Build();
    }

    /// <summary>
    /// Creates a data update message.
    /// </summary>
    public static ServerToClientMessage CreateDataUpdate(
        string surfaceId,
        Dictionary<string, object> data,
        string? path = null)
    {
        var entries = new List<DataEntry>();
        
        foreach (var kvp in data)
        {
            var entry = new DataEntry { Key = kvp.Key };
            
            if (kvp.Value is string s)
                entry.ValueString = s;
            else if (kvp.Value is double d)
                entry.ValueNumber = d;
            else if (kvp.Value is int i)
                entry.ValueNumber = i;
            else if (kvp.Value is bool b)
                entry.ValueBoolean = b;
            
            entries.Add(entry);
        }

        return new ServerToClientMessage
        {
            DataModelUpdate = new DataModelUpdateMessage
            {
                SurfaceId = surfaceId,
                Path = path,
                Contents = entries
            }
        };
    }

    /// <summary>
    /// Creates a delete surface message.
    /// </summary>
    public static ServerToClientMessage DeleteSurface(string surfaceId)
    {
        return new ServerToClientMessage
        {
            DeleteSurface = new DeleteSurfaceMessage
            {
                SurfaceId = surfaceId
            }
        };
    }
}

