namespace A2UI.Theming;

/// <summary>
/// Service for managing A2UI themes.
/// </summary>
public class ThemeService
{
    private IA2UITheme _currentTheme;
    private readonly Dictionary<string, IA2UITheme> _themes = new();

    /// <summary>
    /// Event raised when the theme changes.
    /// </summary>
    public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;

    public ThemeService()
    {
        // Register default themes
        RegisterTheme(new DefaultTheme());
        RegisterTheme(new DarkTheme());

        // Set default theme
        _currentTheme = _themes["Default"];
    }

    /// <summary>
    /// Gets the current theme.
    /// </summary>
    public IA2UITheme CurrentTheme => _currentTheme;

    /// <summary>
    /// Registers a theme.
    /// </summary>
    public void RegisterTheme(IA2UITheme theme)
    {
        _themes[theme.Name] = theme;
    }

    /// <summary>
    /// Sets the current theme by name.
    /// </summary>
    public bool SetTheme(string themeName)
    {
        if (_themes.TryGetValue(themeName, out var theme))
        {
            var oldTheme = _currentTheme;
            _currentTheme = theme;
            ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(oldTheme, theme));
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets all registered theme names.
    /// </summary>
    public IEnumerable<string> GetThemeNames() => _themes.Keys;

    /// <summary>
    /// Gets a theme by name.
    /// </summary>
    public IA2UITheme? GetTheme(string themeName)
    {
        return _themes.TryGetValue(themeName, out var theme) ? theme : null;
    }

    /// <summary>
    /// Generates CSS for the current theme.
    /// </summary>
    public string GenerateThemeCss()
    {
        return ThemeCssGenerator.GenerateCssVariables(_currentTheme) + "\n\n" + ThemeCssGenerator.GenerateBaseStyles();
    }
}

/// <summary>
/// Event args for theme changes.
/// </summary>
public class ThemeChangedEventArgs : EventArgs
{
    public IA2UITheme OldTheme { get; }
    public IA2UITheme NewTheme { get; }

    public ThemeChangedEventArgs(IA2UITheme oldTheme, IA2UITheme newTheme)
    {
        OldTheme = oldTheme;
        NewTheme = newTheme;
    }
}

