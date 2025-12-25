namespace A2UI.Core;

/// <summary>
/// Constants for the A2UI protocol.
/// </summary>
public static class A2UIConstants
{
    /// <summary>
    /// The default catalog ID for A2UI 0.8.
    /// </summary>
    public const string StandardCatalogId = "https://github.com/google/A2UI/blob/main/specification/0.8/json/standard_catalog_definition.json";

    /// <summary>
    /// The default surface ID when none is specified.
    /// </summary>
    public const string DefaultSurfaceId = "@default";

    /// <summary>
    /// Component types defined in the standard catalog.
    /// </summary>
    public static class ComponentTypes
    {
        public const string Text = "Text";
        public const string Image = "Image";
        public const string Icon = "Icon";
        public const string Button = "Button";
        public const string Card = "Card";
        public const string Row = "Row";
        public const string Column = "Column";
        public const string List = "List";
        public const string TextField = "TextField";
        public const string CheckBox = "CheckBox";
        public const string DateTimeInput = "DateTimeInput";
        public const string Slider = "Slider";
        public const string MultipleChoice = "MultipleChoice";
        public const string Divider = "Divider";
        public const string Tabs = "Tabs";
        public const string Modal = "Modal";
        public const string Video = "Video";
        public const string AudioPlayer = "AudioPlayer";
    }

    /// <summary>
    /// Text usage hints.
    /// </summary>
    public static class TextUsageHints
    {
        public const string H1 = "h1";
        public const string H2 = "h2";
        public const string H3 = "h3";
        public const string H4 = "h4";
        public const string H5 = "h5";
        public const string Caption = "caption";
        public const string Body = "body";
    }

    /// <summary>
    /// Image fit values.
    /// </summary>
    public static class ImageFit
    {
        public const string Contain = "contain";
        public const string Cover = "cover";
        public const string Fill = "fill";
        public const string None = "none";
        public const string ScaleDown = "scale-down";
    }

    /// <summary>
    /// Image usage hints.
    /// </summary>
    public static class ImageUsageHints
    {
        public const string Icon = "icon";
        public const string Avatar = "avatar";
        public const string SmallFeature = "smallFeature";
        public const string MediumFeature = "mediumFeature";
        public const string LargeFeature = "largeFeature";
        public const string Header = "header";
    }

    /// <summary>
    /// Layout distribution values (justify-content).
    /// </summary>
    public static class Distribution
    {
        public const string Start = "start";
        public const string End = "end";
        public const string Center = "center";
        public const string SpaceBetween = "spaceBetween";
        public const string SpaceAround = "spaceAround";
        public const string SpaceEvenly = "spaceEvenly";
    }

    /// <summary>
    /// Layout alignment values (align-items).
    /// </summary>
    public static class Alignment
    {
        public const string Start = "start";
        public const string End = "end";
        public const string Center = "center";
        public const string Stretch = "stretch";
        public const string Baseline = "baseline";
    }

    /// <summary>
    /// TextField types.
    /// </summary>
    public static class TextFieldTypes
    {
        public const string ShortText = "shortText";
        public const string LongText = "longText";
        public const string Number = "number";
        public const string Obscured = "obscured";
        public const string Date = "date";
    }

    /// <summary>
    /// List/Divider direction values.
    /// </summary>
    public static class Direction
    {
        public const string Horizontal = "horizontal";
        public const string Vertical = "vertical";
    }
}

