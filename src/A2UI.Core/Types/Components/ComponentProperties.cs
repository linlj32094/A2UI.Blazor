using System.Text.Json.Serialization;

namespace A2UI.Core.Types.Components;

/// <summary>
/// Text component properties.
/// </summary>
public class TextProperties
{
    [JsonPropertyName("text")]
    public required Dictionary<string, object> Text { get; set; }

    [JsonPropertyName("usageHint")]
    public string? UsageHint { get; set; } // h1, h2, h3, h4, h5, caption, body
}

/// <summary>
/// Image component properties.
/// </summary>
public class ImageProperties
{
    [JsonPropertyName("url")]
    public required Dictionary<string, object> Url { get; set; }

    [JsonPropertyName("fit")]
    public string? Fit { get; set; } // contain, cover, fill, none, scale-down

    [JsonPropertyName("usageHint")]
    public string? UsageHint { get; set; } // icon, avatar, smallFeature, mediumFeature, largeFeature, header
}

/// <summary>
/// Icon component properties.
/// </summary>
public class IconProperties
{
    [JsonPropertyName("name")]
    public required Dictionary<string, object> Name { get; set; }
}

/// <summary>
/// Button component properties.
/// </summary>
public class ButtonProperties
{
    [JsonPropertyName("child")]
    public required string Child { get; set; }

    [JsonPropertyName("action")]
    public Dictionary<string, object>? Action { get; set; }

    [JsonPropertyName("primary")]
    public bool? Primary { get; set; }
}

/// <summary>
/// Card component properties.
/// </summary>
public class CardProperties
{
    [JsonPropertyName("child")]
    public required string Child { get; set; }
}

/// <summary>
/// Row component properties.
/// </summary>
public class RowProperties
{
    [JsonPropertyName("children")]
    public required Dictionary<string, object> Children { get; set; }

    [JsonPropertyName("distribution")]
    public string? Distribution { get; set; } // start, end, center, spaceBetween, spaceAround, spaceEvenly

    [JsonPropertyName("alignment")]
    public string? Alignment { get; set; } // start, end, center, stretch, baseline
}

/// <summary>
/// Column component properties.
/// </summary>
public class ColumnProperties
{
    [JsonPropertyName("children")]
    public required Dictionary<string, object> Children { get; set; }

    [JsonPropertyName("distribution")]
    public string? Distribution { get; set; }

    [JsonPropertyName("alignment")]
    public string? Alignment { get; set; }
}

/// <summary>
/// List component properties.
/// </summary>
public class ListProperties
{
    [JsonPropertyName("children")]
    public required Dictionary<string, object> Children { get; set; }

    [JsonPropertyName("direction")]
    public string? Direction { get; set; } // horizontal, vertical

    [JsonPropertyName("alignment")]
    public string? Alignment { get; set; }
}

/// <summary>
/// TextField component properties.
/// </summary>
public class TextFieldProperties
{
    [JsonPropertyName("text")]
    public required Dictionary<string, object> Text { get; set; }

    [JsonPropertyName("label")]
    public Dictionary<string, object>? Label { get; set; }

    [JsonPropertyName("textFieldType")]
    public string? TextFieldType { get; set; } // shortText, longText, number, obscured, date

    [JsonPropertyName("validationRegexp")]
    public string? ValidationRegexp { get; set; }
}

/// <summary>
/// CheckBox component properties.
/// </summary>
public class CheckBoxProperties
{
    [JsonPropertyName("value")]
    public required Dictionary<string, object> Value { get; set; }

    [JsonPropertyName("label")]
    public Dictionary<string, object>? Label { get; set; }
}

/// <summary>
/// DateTimeInput component properties.
/// </summary>
public class DateTimeInputProperties
{
    [JsonPropertyName("value")]
    public required Dictionary<string, object> Value { get; set; }

    [JsonPropertyName("enableDate")]
    public bool? EnableDate { get; set; }

    [JsonPropertyName("enableTime")]
    public bool? EnableTime { get; set; }

    [JsonPropertyName("label")]
    public Dictionary<string, object>? Label { get; set; }
}

/// <summary>
/// Slider component properties.
/// </summary>
public class SliderProperties
{
    [JsonPropertyName("value")]
    public required Dictionary<string, object> Value { get; set; }

    [JsonPropertyName("minValue")]
    public Dictionary<string, object>? MinValue { get; set; }

    [JsonPropertyName("maxValue")]
    public Dictionary<string, object>? MaxValue { get; set; }

    [JsonPropertyName("label")]
    public Dictionary<string, object>? Label { get; set; }
}

/// <summary>
/// MultipleChoice component properties.
/// </summary>
public class MultipleChoiceProperties
{
    [JsonPropertyName("options")]
    public required Dictionary<string, object> Options { get; set; }

    [JsonPropertyName("selections")]
    public required Dictionary<string, object> Selections { get; set; }

    [JsonPropertyName("maxAllowedSelections")]
    public Dictionary<string, object>? MaxAllowedSelections { get; set; }

    [JsonPropertyName("label")]
    public Dictionary<string, object>? Label { get; set; }
}

/// <summary>
/// Divider component properties.
/// </summary>
public class DividerProperties
{
    [JsonPropertyName("axis")]
    public string? Axis { get; set; } // horizontal, vertical
}

/// <summary>
/// Tabs component properties.
/// </summary>
public class TabsProperties
{
    [JsonPropertyName("tabItems")]
    public required List<Dictionary<string, object>> TabItems { get; set; }

    [JsonPropertyName("selectedTab")]
    public Dictionary<string, object>? SelectedTab { get; set; }
}

/// <summary>
/// Modal component properties.
/// </summary>
public class ModalProperties
{
    [JsonPropertyName("entryPointChild")]
    public required string EntryPointChild { get; set; }

    [JsonPropertyName("contentChild")]
    public required string ContentChild { get; set; }
}

/// <summary>
/// Video component properties.
/// </summary>
public class VideoProperties
{
    [JsonPropertyName("url")]
    public required Dictionary<string, object> Url { get; set; }
}

/// <summary>
/// AudioPlayer component properties.
/// </summary>
public class AudioPlayerProperties
{
    [JsonPropertyName("url")]
    public required Dictionary<string, object> Url { get; set; }

    [JsonPropertyName("description")]
    public Dictionary<string, object>? Description { get; set; }
}

