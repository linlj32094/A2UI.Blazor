namespace A2UI.Core.Types;

/// <summary>
/// Represents a UI surface containing components and a data model.
/// </summary>
public class Surface
{
    /// <summary>
    /// The unique identifier for this surface.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// The catalog ID to use for this surface.
    /// </summary>
    public string? CatalogId { get; set; }

    /// <summary>
    /// The ID of the root component.
    /// </summary>
    public string? RootComponentId { get; set; }

    /// <summary>
    /// Global styles for this surface.
    /// </summary>
    public Dictionary<string, object>? Styles { get; set; }

    /// <summary>
    /// Component buffer - stores all components by their ID (Adjacency List).
    /// </summary>
    public Dictionary<string, ComponentNode> Components { get; set; } = new();

    /// <summary>
    /// The data model for this surface.
    /// </summary>
    public DataModel DataModel { get; set; } = new();

    /// <summary>
    /// Indicates if this surface is ready to render.
    /// </summary>
    public bool IsReadyToRender { get; set; }
}

/// <summary>
/// Represents a resolved component node in the component tree.
/// </summary>
public class ComponentNode
{
    /// <summary>
    /// The unique identifier for this component.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// The component type (e.g., "Text", "Button", "Row").
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// The component's properties.
    /// </summary>
    public required Dictionary<string, object> Properties { get; set; }

    /// <summary>
    /// The weight (flex-grow) for this component.
    /// </summary>
    public double? Weight { get; set; }

    /// <summary>
    /// The data context path for this component (used for relative data binding).
    /// </summary>
    public string? DataContextPath { get; set; }
}

/// <summary>
/// Represents the data model for a surface.
/// </summary>
public class DataModel
{
    private readonly Dictionary<string, object> _data = new();

    /// <summary>
    /// Gets a value from the data model at the specified path.
    /// </summary>
    public object? GetValue(string path)
    {
        if (string.IsNullOrEmpty(path) || path == "/")
        {
            return _data;
        }

        var parts = path.TrimStart('/').Split('/');
        object? current = _data;

        foreach (var part in parts)
        {
            if (current is Dictionary<string, object> dict && dict.TryGetValue(part, out var value))
            {
                current = value;
            }
            else
            {
                return null;
            }
        }

        return current;
    }

    /// <summary>
    /// Sets a value in the data model at the specified path.
    /// </summary>
    public void SetValue(string path, object value)
    {
        if (string.IsNullOrEmpty(path) || path == "/")
        {
            // Replace entire data model
            _data.Clear();
            if (value is Dictionary<string, object> dict)
            {
                foreach (var kvp in dict)
                {
                    _data[kvp.Key] = kvp.Value;
                }
            }
            return;
        }

        var parts = path.TrimStart('/').Split('/');
        var current = _data as Dictionary<string, object>;

        for (int i = 0; i < parts.Length - 1; i++)
        {
            var part = parts[i];
            if (!current.ContainsKey(part))
            {
                current[part] = new Dictionary<string, object>();
            }

            if (current[part] is Dictionary<string, object> nextDict)
            {
                current = nextDict;
            }
            else
            {
                // Path conflict - create new dict
                current[part] = new Dictionary<string, object>();
                current = (Dictionary<string, object>)current[part];
            }
        }

        current[parts[^1]] = value;
    }

    /// <summary>
    /// Gets the entire data model as a dictionary.
    /// </summary>
    public Dictionary<string, object> GetAll() => _data;

    /// <summary>
    /// Clears the data model.
    /// </summary>
    public void Clear() => _data.Clear();
}

