using System.Text.Json.Serialization;

namespace A2UI.Core.Messages;

/// <summary>
/// Represents a message sent from the server to the client in the A2UI protocol.
/// A message MUST contain exactly ONE of the action properties.
/// </summary>
public class ServerToClientMessage
{
    [JsonPropertyName("beginRendering")]
    public BeginRenderingMessage? BeginRendering { get; set; }

    [JsonPropertyName("surfaceUpdate")]
    public SurfaceUpdateMessage? SurfaceUpdate { get; set; }

    [JsonPropertyName("dataModelUpdate")]
    public DataModelUpdateMessage? DataModelUpdate { get; set; }

    [JsonPropertyName("deleteSurface")]
    public DeleteSurfaceMessage? DeleteSurface { get; set; }
}

/// <summary>
/// Signals the client to begin rendering a surface with a root component and specific styles.
/// </summary>
public class BeginRenderingMessage
{
    /// <summary>
    /// The unique identifier for the UI surface to be rendered.
    /// </summary>
    [JsonPropertyName("surfaceId")]
    public required string SurfaceId { get; set; }

    /// <summary>
    /// The identifier of the component catalog to use for this surface.
    /// If omitted, the client MUST default to the standard catalog for this A2UI version.
    /// </summary>
    [JsonPropertyName("catalogId")]
    public string? CatalogId { get; set; }

    /// <summary>
    /// The ID of the root component to render.
    /// </summary>
    [JsonPropertyName("root")]
    public required string Root { get; set; }

    /// <summary>
    /// Styling information for the UI.
    /// </summary>
    [JsonPropertyName("styles")]
    public Dictionary<string, object>? Styles { get; set; }
}

/// <summary>
/// Updates a surface with a new set of components.
/// </summary>
public class SurfaceUpdateMessage
{
    /// <summary>
    /// The unique identifier for the UI surface to be updated.
    /// </summary>
    [JsonPropertyName("surfaceId")]
    public required string SurfaceId { get; set; }

    /// <summary>
    /// A list containing all UI components for the surface.
    /// </summary>
    [JsonPropertyName("components")]
    public required List<ComponentDefinition> Components { get; set; }
}

/// <summary>
/// Represents a single component in a UI widget tree.
/// </summary>
public class ComponentDefinition
{
    /// <summary>
    /// The unique identifier for this component.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The relative weight of this component within a Row or Column.
    /// This corresponds to the CSS 'flex-grow' property.
    /// </summary>
    [JsonPropertyName("weight")]
    public double? Weight { get; set; }

    /// <summary>
    /// A wrapper object that MUST contain exactly one key, which is the name of the component type.
    /// The value is an object containing the properties for that specific component.
    /// </summary>
    [JsonPropertyName("component")]
    public required Dictionary<string, object> Component { get; set; }
}

/// <summary>
/// Updates the data model for a surface.
/// </summary>
public class DataModelUpdateMessage
{
    /// <summary>
    /// The unique identifier for the UI surface this data model update applies to.
    /// </summary>
    [JsonPropertyName("surfaceId")]
    public required string SurfaceId { get; set; }

    /// <summary>
    /// An optional path to a location within the data model (e.g., '/user/name').
    /// If omitted or set to '/', the entire data model will be replaced.
    /// </summary>
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    /// <summary>
    /// An array of data entries. Each entry must contain a 'key' and exactly one corresponding typed 'value*' property.
    /// </summary>
    [JsonPropertyName("contents")]
    public required List<DataEntry> Contents { get; set; }
}

/// <summary>
/// A single data entry in the data model.
/// </summary>
public class DataEntry
{
    /// <summary>
    /// The key for this data entry.
    /// </summary>
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("valueString")]
    public string? ValueString { get; set; }

    [JsonPropertyName("valueNumber")]
    public double? ValueNumber { get; set; }

    [JsonPropertyName("valueBoolean")]
    public bool? ValueBoolean { get; set; }

    [JsonPropertyName("valueMap")]
    public List<DataEntry>? ValueMap { get; set; }
}

/// <summary>
/// Signals the client to delete the surface identified by 'surfaceId'.
/// </summary>
public class DeleteSurfaceMessage
{
    /// <summary>
    /// The unique identifier for the UI surface to be deleted.
    /// </summary>
    [JsonPropertyName("surfaceId")]
    public required string SurfaceId { get; set; }
}

