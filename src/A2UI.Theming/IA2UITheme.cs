namespace A2UI.Theming;

/// <summary>
/// Defines the contract for an A2UI theme.
/// </summary>
public interface IA2UITheme
{
    /// <summary>
    /// Gets the theme name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets CSS classes for component styling.
    /// </summary>
    ComponentTheme Components { get; }

    /// <summary>
    /// Gets the primary color.
    /// </summary>
    string PrimaryColor { get; }

    /// <summary>
    /// Gets the secondary color.
    /// </summary>
    string SecondaryColor { get; }

    /// <summary>
    /// Gets the background color.
    /// </summary>
    string BackgroundColor { get; }

    /// <summary>
    /// Gets the text color.
    /// </summary>
    string TextColor { get; }

    /// <summary>
    /// Gets the error color.
    /// </summary>
    string ErrorColor { get; }

    /// <summary>
    /// Gets the success color.
    /// </summary>
    string SuccessColor { get; }

    /// <summary>
    /// Gets the font family.
    /// </summary>
    string FontFamily { get; }

    /// <summary>
    /// Gets the border radius.
    /// </summary>
    string BorderRadius { get; }

    /// <summary>
    /// Gets additional custom CSS variables.
    /// </summary>
    Dictionary<string, string> CustomVariables { get; }
}

/// <summary>
/// Contains CSS classes for all components.
/// </summary>
public class ComponentTheme
{
    public string Text { get; set; } = "";
    public string TextH1 { get; set; } = "";
    public string TextH2 { get; set; } = "";
    public string TextH3 { get; set; } = "";
    public string TextH4 { get; set; } = "";
    public string TextH5 { get; set; } = "";
    public string TextCaption { get; set; } = "";
    public string TextBody { get; set; } = "";

    public string Image { get; set; } = "";
    public string ImageIcon { get; set; } = "";
    public string ImageAvatar { get; set; } = "";
    public string ImageFeature { get; set; } = "";

    public string Icon { get; set; } = "";

    public string Button { get; set; } = "";
    public string ButtonPrimary { get; set; } = "";
    public string ButtonSecondary { get; set; } = "";

    public string Card { get; set; } = "";
    
    public string Row { get; set; } = "";
    public string Column { get; set; } = "";
    public string List { get; set; } = "";

    public string TextField { get; set; } = "";
    public string TextFieldContainer { get; set; } = "";
    public string TextFieldLabel { get; set; } = "";
    public string TextFieldInput { get; set; } = "";

    public string CheckBox { get; set; } = "";
    public string CheckBoxContainer { get; set; } = "";
    public string CheckBoxInput { get; set; } = "";
    public string CheckBoxLabel { get; set; } = "";

    public string DateTimeInput { get; set; } = "";
    public string DateTimeInputContainer { get; set; } = "";
    public string DateTimeInputInput { get; set; } = "";
    public string DateTimeInputLabel { get; set; } = "";

    public string Slider { get; set; } = "";
    public string SliderContainer { get; set; } = "";
    public string SliderInput { get; set; } = "";
    public string SliderLabel { get; set; } = "";

    public string MultipleChoice { get; set; } = "";
    public string MultipleChoiceContainer { get; set; } = "";
    public string MultipleChoiceOption { get; set; } = "";
    public string MultipleChoiceLabel { get; set; } = "";

    public string Divider { get; set; } = "";
    public string DividerHorizontal { get; set; } = "";
    public string DividerVertical { get; set; } = "";

    public string Tabs { get; set; } = "";
    public string TabsContainer { get; set; } = "";
    public string TabsControls { get; set; } = "";
    public string TabsControl { get; set; } = "";
    public string TabsControlActive { get; set; } = "";
    public string TabsContent { get; set; } = "";

    public string Modal { get; set; } = "";
    public string ModalBackdrop { get; set; } = "";
    public string ModalContent { get; set; } = "";

    public string Video { get; set; } = "";
    public string AudioPlayer { get; set; } = "";
}

