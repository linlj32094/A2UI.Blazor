using A2UI.Core;
using A2UI.Core.Messages;

namespace A2UI.AgentSDK.Builders;

/// <summary>
/// Builder for creating A2UI surfaces with a fluent API.
/// </summary>
public class SurfaceBuilder
{
    private readonly string _surfaceId;
    private readonly List<ComponentDefinition> _components = new();
    private readonly List<DataEntry> _dataEntries = new();
    private string? _rootComponentId;
    private string? _catalogId;
    private Dictionary<string, object>? _styles;

    public SurfaceBuilder(string surfaceId)
    {
        _surfaceId = surfaceId;
    }

    /// <summary>
    /// Sets the root component ID.
    /// </summary>
    public SurfaceBuilder WithRoot(string rootComponentId)
    {
        _rootComponentId = rootComponentId;
        return this;
    }

    /// <summary>
    /// Sets the catalog ID.
    /// </summary>
    public SurfaceBuilder WithCatalog(string catalogId)
    {
        _catalogId = catalogId;
        return this;
    }

    /// <summary>
    /// Adds global styles.
    /// </summary>
    public SurfaceBuilder WithStyles(Dictionary<string, object> styles)
    {
        _styles = styles;
        return this;
    }

    /// <summary>
    /// Adds a text component.
    /// </summary>
    public SurfaceBuilder AddText(string id, Action<TextComponentBuilder> configure)
    {
        var builder = new TextComponentBuilder(id);
        configure(builder);
        _components.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a button component.
    /// </summary>
    public SurfaceBuilder AddButton(string id, Action<ButtonComponentBuilder> configure)
    {
        var builder = new ButtonComponentBuilder(id);
        configure(builder);
        _components.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds an image component.
    /// </summary>
    public SurfaceBuilder AddImage(string id, Action<ImageComponentBuilder> configure)
    {
        var builder = new ImageComponentBuilder(id);
        configure(builder);
        _components.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a card component.
    /// </summary>
    public SurfaceBuilder AddCard(string id, Action<CardComponentBuilder> configure)
    {
        var builder = new CardComponentBuilder(id);
        configure(builder);
        _components.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a row component.
    /// </summary>
    public SurfaceBuilder AddRow(string id, Action<RowComponentBuilder> configure)
    {
        var builder = new RowComponentBuilder(id);
        configure(builder);
        _components.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a column component.
    /// </summary>
    public SurfaceBuilder AddColumn(string id, Action<ColumnComponentBuilder> configure)
    {
        var builder = new ColumnComponentBuilder(id);
        configure(builder);
        _components.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds data to the data model.
    /// </summary>
    public SurfaceBuilder AddData(string key, string value)
    {
        _dataEntries.Add(new DataEntry { Key = key, ValueString = value });
        return this;
    }

    /// <summary>
    /// Adds data to the data model.
    /// </summary>
    public SurfaceBuilder AddData(string key, double value)
    {
        _dataEntries.Add(new DataEntry { Key = key, ValueNumber = value });
        return this;
    }

    /// <summary>
    /// Adds data to the data model.
    /// </summary>
    public SurfaceBuilder AddData(string key, bool value)
    {
        _dataEntries.Add(new DataEntry { Key = key, ValueBoolean = value });
        return this;
    }

    /// <summary>
    /// Builds the messages for this surface.
    /// </summary>
    public List<ServerToClientMessage> Build()
    {
        var messages = new List<ServerToClientMessage>();

        // Add surface update message
        if (_components.Count > 0)
        {
            messages.Add(new ServerToClientMessage
            {
                SurfaceUpdate = new SurfaceUpdateMessage
                {
                    SurfaceId = _surfaceId,
                    Components = _components
                }
            });
        }

        // Add data model update message
        if (_dataEntries.Count > 0)
        {
            messages.Add(new ServerToClientMessage
            {
                DataModelUpdate = new DataModelUpdateMessage
                {
                    SurfaceId = _surfaceId,
                    Contents = _dataEntries
                }
            });
        }

        // Add begin rendering message
        if (!string.IsNullOrEmpty(_rootComponentId))
        {
            messages.Add(new ServerToClientMessage
            {
                BeginRendering = new BeginRenderingMessage
                {
                    SurfaceId = _surfaceId,
                    Root = _rootComponentId,
                    CatalogId = _catalogId,
                    Styles = _styles
                }
            });
        }

        return messages;
    }
}

