namespace A2UI.Theming;

/// <summary>
/// The default A2UI theme with clean, modern styling.
/// </summary>
public class DefaultTheme : IA2UITheme
{
    public string Name => "Default";

    public ComponentTheme Components { get; } = new()
    {
        Text = "a2ui-text",
        TextH1 = "a2ui-text-h1",
        TextH2 = "a2ui-text-h2",
        TextH3 = "a2ui-text-h3",
        TextH4 = "a2ui-text-h4",
        TextH5 = "a2ui-text-h5",
        TextCaption = "a2ui-text-caption",
        TextBody = "a2ui-text-body",

        Image = "a2ui-image",
        ImageIcon = "a2ui-image-icon",
        ImageAvatar = "a2ui-image-avatar",
        ImageFeature = "a2ui-image-feature",

        Icon = "a2ui-icon",

        Button = "a2ui-button",
        ButtonPrimary = "a2ui-button-primary",
        ButtonSecondary = "a2ui-button-secondary",

        Card = "a2ui-card",

        Row = "a2ui-row",
        Column = "a2ui-column",
        List = "a2ui-list",

        TextField = "a2ui-textfield",
        TextFieldContainer = "a2ui-textfield-container",
        TextFieldLabel = "a2ui-textfield-label",
        TextFieldInput = "a2ui-textfield-input",

        CheckBox = "a2ui-checkbox",
        CheckBoxContainer = "a2ui-checkbox-container",
        CheckBoxInput = "a2ui-checkbox-input",
        CheckBoxLabel = "a2ui-checkbox-label",

        DateTimeInput = "a2ui-datetime",
        DateTimeInputContainer = "a2ui-datetime-container",
        DateTimeInputInput = "a2ui-datetime-input",
        DateTimeInputLabel = "a2ui-datetime-label",

        Slider = "a2ui-slider",
        SliderContainer = "a2ui-slider-container",
        SliderInput = "a2ui-slider-input",
        SliderLabel = "a2ui-slider-label",

        MultipleChoice = "a2ui-multiplechoice",
        MultipleChoiceContainer = "a2ui-multiplechoice-container",
        MultipleChoiceOption = "a2ui-multiplechoice-option",
        MultipleChoiceLabel = "a2ui-multiplechoice-label",

        Divider = "a2ui-divider",
        DividerHorizontal = "a2ui-divider-horizontal",
        DividerVertical = "a2ui-divider-vertical",

        Tabs = "a2ui-tabs",
        TabsContainer = "a2ui-tabs-container",
        TabsControls = "a2ui-tabs-controls",
        TabsControl = "a2ui-tabs-control",
        TabsControlActive = "a2ui-tabs-control-active",
        TabsContent = "a2ui-tabs-content",

        Modal = "a2ui-modal",
        ModalBackdrop = "a2ui-modal-backdrop",
        ModalContent = "a2ui-modal-content",

        Video = "a2ui-video",
        AudioPlayer = "a2ui-audio",
    };

    public string PrimaryColor => "#3b82f6"; // Blue
    public string SecondaryColor => "#6b7280"; // Gray
    public string BackgroundColor => "#ffffff";
    public string TextColor => "#1f2937";
    public string ErrorColor => "#ef4444"; // Red
    public string SuccessColor => "#10b981"; // Green
    public string FontFamily => "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif";
    public string BorderRadius => "0.375rem";

    public Dictionary<string, string> CustomVariables { get; } = new();
}

/// <summary>
/// A dark theme variant for A2UI.
/// </summary>
public class DarkTheme : DefaultTheme
{
    public new string Name => "Dark";
    public new string BackgroundColor => "#1f2937";
    public new string TextColor => "#f9fafb";
    public new string SecondaryColor => "#9ca3af";
}

