using System.Text.Json.Serialization;

namespace A2UI.Core.Types;

/// <summary>
/// Represents a value that can be either a literal or bound to the data model.
/// </summary>
/// <typeparam name="T">The type of the literal value.</typeparam>
public class BoundValue<T>
{
    /// <summary>
    /// A static literal value.
    /// </summary>
    [JsonPropertyName("literalString")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LiteralString { get; set; }

    [JsonPropertyName("literalNumber")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? LiteralNumber { get; set; }

    [JsonPropertyName("literalBoolean")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LiteralBoolean { get; set; }

    /// <summary>
    /// A path to a value in the data model (e.g., '/user/name').
    /// </summary>
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    /// <summary>
    /// Gets the literal value as the specified type.
    /// </summary>
    public T? GetLiteralValue()
    {
        if (LiteralString != null && typeof(T) == typeof(string))
            return (T)(object)LiteralString;
        if (LiteralNumber != null && typeof(T) == typeof(double))
            return (T)(object)LiteralNumber;
        if (LiteralBoolean != null && typeof(T) == typeof(bool))
            return (T)(object)LiteralBoolean;
        return default;
    }

    /// <summary>
    /// Checks if this bound value has a literal value.
    /// </summary>
    public bool HasLiteralValue => LiteralString != null || LiteralNumber != null || LiteralBoolean != null;

    /// <summary>
    /// Checks if this bound value has a path binding.
    /// </summary>
    public bool HasPath => !string.IsNullOrEmpty(Path);
}

/// <summary>
/// Represents a string bound value (most common case).
/// </summary>
public class StringValue : BoundValue<string>
{
}

/// <summary>
/// Represents a number bound value.
/// </summary>
public class NumberValue : BoundValue<double>
{
}

/// <summary>
/// Represents a boolean bound value.
/// </summary>
public class BooleanValue : BoundValue<bool>
{
}

/// <summary>
/// Represents children of a container component.
/// Must contain exactly one of: ExplicitList or Template.
/// </summary>
public class ComponentChildren
{
    /// <summary>
    /// An ordered list of component IDs that are direct children.
    /// </summary>
    [JsonPropertyName("explicitList")]
    public List<string>? ExplicitList { get; set; }

    /// <summary>
    /// Defines a template for rendering dynamic lists of children.
    /// </summary>
    [JsonPropertyName("template")]
    public ChildrenTemplate? Template { get; set; }
}

/// <summary>
/// Defines a template for rendering dynamic lists of children.
/// </summary>
public class ChildrenTemplate
{
    /// <summary>
    /// A path to a list in the data model.
    /// </summary>
    [JsonPropertyName("dataBinding")]
    public required string DataBinding { get; set; }

    /// <summary>
    /// The ID of the component to use as a template for each item in the data-bound list.
    /// </summary>
    [JsonPropertyName("componentId")]
    public required string ComponentId { get; set; }
}

/// <summary>
/// Defines an action that can be triggered by a component.
/// </summary>
public class ComponentAction
{
    /// <summary>
    /// The name of the action.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Context data to send with the action.
    /// Each entry contains a key and a bound value.
    /// </summary>
    [JsonPropertyName("context")]
    public List<ActionContextEntry>? Context { get; set; }
}

/// <summary>
/// A single entry in an action's context.
/// </summary>
public class ActionContextEntry
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("value")]
    public required Dictionary<string, object> Value { get; set; }
}

