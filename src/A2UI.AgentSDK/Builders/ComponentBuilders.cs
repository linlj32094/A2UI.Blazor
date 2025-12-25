using A2UI.Core;
using A2UI.Core.Messages;

namespace A2UI.AgentSDK.Builders;

/// <summary>
/// Base class for component builders.
/// </summary>
public abstract class ComponentBuilder
{
    protected readonly string Id;
    protected double? Weight;

    protected ComponentBuilder(string id)
    {
        Id = id;
    }

    /// <summary>
    /// Sets the weight (flex-grow) for this component.
    /// </summary>
    public ComponentBuilder WithWeight(double weight)
    {
        Weight = weight;
        return this;
    }

    public abstract ComponentDefinition Build();
}

/// <summary>
/// Builder for Text components.
/// </summary>
public class TextComponentBuilder : ComponentBuilder
{
    private string? _text;
    private string? _path;
    private string? _usageHint;

    public TextComponentBuilder(string id) : base(id) { }

    public TextComponentBuilder WithText(string text)
    {
        _text = text;
        return this;
    }

    public TextComponentBuilder BindToPath(string path)
    {
        _path = path;
        return this;
    }

    public TextComponentBuilder WithUsageHint(string usageHint)
    {
        _usageHint = usageHint;
        return this;
    }

    public override ComponentDefinition Build()
    {
        var textValue = new Dictionary<string, object>();
        if (_text != null) textValue["literalString"] = _text;
        if (_path != null) textValue["path"] = _path;

        var properties = new Dictionary<string, object>
        {
            { "text", textValue }
        };

        if (_usageHint != null)
            properties["usageHint"] = _usageHint;

        return new ComponentDefinition
        {
            Id = Id,
            Weight = Weight,
            Component = new Dictionary<string, object>
            {
                { A2UIConstants.ComponentTypes.Text, properties }
            }
        };
    }
}

/// <summary>
/// Builder for Button components.
/// </summary>
public class ButtonComponentBuilder : ComponentBuilder
{
    private string? _childId;
    private string? _actionName;
    private readonly List<(string key, string? path, object? literal)> _actionContext = new();
    private bool _isPrimary;

    public ButtonComponentBuilder(string id) : base(id) { }

    public ButtonComponentBuilder WithChild(string childId)
    {
        _childId = childId;
        return this;
    }

    public ButtonComponentBuilder WithAction(string actionName)
    {
        _actionName = actionName;
        return this;
    }

    public ButtonComponentBuilder AddActionContext(string key, string path)
    {
        _actionContext.Add((key, path, null));
        return this;
    }

    public ButtonComponentBuilder AddActionContext(string key, object literal)
    {
        _actionContext.Add((key, null, literal));
        return this;
    }

    public ButtonComponentBuilder AsPrimary()
    {
        _isPrimary = true;
        return this;
    }

    public override ComponentDefinition Build()
    {
        var properties = new Dictionary<string, object>();

        if (_childId != null)
            properties["child"] = _childId;

        if (_actionName != null)
        {
            var action = new Dictionary<string, object>
            {
                { "name", _actionName }
            };

            if (_actionContext.Count > 0)
            {
                var context = new List<Dictionary<string, object>>();
                foreach (var (key, path, literal) in _actionContext)
                {
                    var entry = new Dictionary<string, object> { { "key", key } };
                    var value = new Dictionary<string, object>();
                    
                    if (path != null) value["path"] = path;
                    if (literal != null)
                    {
                        if (literal is string s) value["literalString"] = s;
                        else if (literal is double d) value["literalNumber"] = d;
                        else if (literal is bool b) value["literalBoolean"] = b;
                    }
                    
                    entry["value"] = value;
                    context.Add(entry);
                }
                action["context"] = context;
            }

            properties["action"] = action;
        }

        if (_isPrimary)
            properties["primary"] = true;

        return new ComponentDefinition
        {
            Id = Id,
            Weight = Weight,
            Component = new Dictionary<string, object>
            {
                { A2UIConstants.ComponentTypes.Button, properties }
            }
        };
    }
}

/// <summary>
/// Builder for Image components.
/// </summary>
public class ImageComponentBuilder : ComponentBuilder
{
    private string? _url;
    private string? _path;
    private string? _fit;
    private string? _usageHint;

    public ImageComponentBuilder(string id) : base(id) { }

    public ImageComponentBuilder WithUrl(string url)
    {
        _url = url;
        return this;
    }

    public ImageComponentBuilder BindToPath(string path)
    {
        _path = path;
        return this;
    }

    public ImageComponentBuilder WithFit(string fit)
    {
        _fit = fit;
        return this;
    }

    public ImageComponentBuilder WithUsageHint(string usageHint)
    {
        _usageHint = usageHint;
        return this;
    }

    public override ComponentDefinition Build()
    {
        var urlValue = new Dictionary<string, object>();
        if (_url != null) urlValue["literalString"] = _url;
        if (_path != null) urlValue["path"] = _path;

        var properties = new Dictionary<string, object>
        {
            { "url", urlValue }
        };

        if (_fit != null) properties["fit"] = _fit;
        if (_usageHint != null) properties["usageHint"] = _usageHint;

        return new ComponentDefinition
        {
            Id = Id,
            Weight = Weight,
            Component = new Dictionary<string, object>
            {
                { A2UIConstants.ComponentTypes.Image, properties }
            }
        };
    }
}

/// <summary>
/// Builder for Card components.
/// </summary>
public class CardComponentBuilder : ComponentBuilder
{
    private string? _childId;

    public CardComponentBuilder(string id) : base(id) { }

    public CardComponentBuilder WithChild(string childId)
    {
        _childId = childId;
        return this;
    }

    public override ComponentDefinition Build()
    {
        var properties = new Dictionary<string, object>();
        if (_childId != null)
            properties["child"] = _childId;

        return new ComponentDefinition
        {
            Id = Id,
            Weight = Weight,
            Component = new Dictionary<string, object>
            {
                { A2UIConstants.ComponentTypes.Card, properties }
            }
        };
    }
}

/// <summary>
/// Builder for Row components.
/// </summary>
public class RowComponentBuilder : ComponentBuilder
{
    private readonly List<string> _children = new();
    private string? _distribution;
    private string? _alignment;

    public RowComponentBuilder(string id) : base(id) { }

    public RowComponentBuilder AddChild(string childId)
    {
        _children.Add(childId);
        return this;
    }

    public RowComponentBuilder WithDistribution(string distribution)
    {
        _distribution = distribution;
        return this;
    }

    public RowComponentBuilder WithAlignment(string alignment)
    {
        _alignment = alignment;
        return this;
    }

    public override ComponentDefinition Build()
    {
        var children = new Dictionary<string, object>
        {
            { "explicitList", _children }
        };

        var properties = new Dictionary<string, object>
        {
            { "children", children }
        };

        if (_distribution != null) properties["distribution"] = _distribution;
        if (_alignment != null) properties["alignment"] = _alignment;

        return new ComponentDefinition
        {
            Id = Id,
            Weight = Weight,
            Component = new Dictionary<string, object>
            {
                { A2UIConstants.ComponentTypes.Row, properties }
            }
        };
    }
}

/// <summary>
/// Builder for Column components.
/// </summary>
public class ColumnComponentBuilder : ComponentBuilder
{
    private readonly List<string> _children = new();
    private string? _distribution;
    private string? _alignment;

    public ColumnComponentBuilder(string id) : base(id) { }

    public ColumnComponentBuilder AddChild(string childId)
    {
        _children.Add(childId);
        return this;
    }

    public ColumnComponentBuilder WithDistribution(string distribution)
    {
        _distribution = distribution;
        return this;
    }

    public ColumnComponentBuilder WithAlignment(string alignment)
    {
        _alignment = alignment;
        return this;
    }

    public override ComponentDefinition Build()
    {
        var children = new Dictionary<string, object>
        {
            { "explicitList", _children }
        };

        var properties = new Dictionary<string, object>
        {
            { "children", children }
        };

        if (_distribution != null) properties["distribution"] = _distribution;
        if (_alignment != null) properties["alignment"] = _alignment;

        return new ComponentDefinition
        {
            Id = Id,
            Weight = Weight,
            Component = new Dictionary<string, object>
            {
                { A2UIConstants.ComponentTypes.Column, properties }
            }
        };
    }
}

