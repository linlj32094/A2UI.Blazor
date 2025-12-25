using System.Text;

namespace A2UI.Theming;

/// <summary>
/// Generates CSS from an A2UI theme.
/// </summary>
public class ThemeCssGenerator
{
    /// <summary>
    /// Generates CSS variables from a theme.
    /// </summary>
    public static string GenerateCssVariables(IA2UITheme theme)
    {
        var sb = new StringBuilder();
        sb.AppendLine(":root {");
        sb.AppendLine($"  --a2ui-primary-color: {theme.PrimaryColor};");
        sb.AppendLine($"  --a2ui-secondary-color: {theme.SecondaryColor};");
        sb.AppendLine($"  --a2ui-background-color: {theme.BackgroundColor};");
        sb.AppendLine($"  --a2ui-text-color: {theme.TextColor};");
        sb.AppendLine($"  --a2ui-error-color: {theme.ErrorColor};");
        sb.AppendLine($"  --a2ui-success-color: {theme.SuccessColor};");
        sb.AppendLine($"  --a2ui-font-family: {theme.FontFamily};");
        sb.AppendLine($"  --a2ui-border-radius: {theme.BorderRadius};");

        foreach (var customVar in theme.CustomVariables)
        {
            sb.AppendLine($"  --a2ui-{customVar.Key}: {customVar.Value};");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>
    /// Generates base component styles.
    /// </summary>
    public static string GenerateBaseStyles()
    {
        return @"
/* A2UI Base Styles */
.a2ui-surface {
    font-family: var(--a2ui-font-family);
    color: var(--a2ui-text-color);
    background-color: var(--a2ui-background-color);
}

/* Text Components */
.a2ui-text {
    margin: 0;
    padding: 0;
}

.a2ui-text-h1 {
    font-size: 2.25rem;
    font-weight: 700;
    line-height: 2.5rem;
    margin-bottom: 1rem;
}

.a2ui-text-h2 {
    font-size: 1.875rem;
    font-weight: 600;
    line-height: 2.25rem;
    margin-bottom: 0.75rem;
}

.a2ui-text-h3 {
    font-size: 1.5rem;
    font-weight: 600;
    line-height: 2rem;
    margin-bottom: 0.5rem;
}

.a2ui-text-h4 {
    font-size: 1.25rem;
    font-weight: 500;
    line-height: 1.75rem;
    margin-bottom: 0.5rem;
}

.a2ui-text-h5 {
    font-size: 1.125rem;
    font-weight: 500;
    line-height: 1.75rem;
    margin-bottom: 0.5rem;
}

.a2ui-text-body {
    font-size: 1rem;
    line-height: 1.5rem;
}

.a2ui-text-caption {
    font-size: 0.875rem;
    line-height: 1.25rem;
    color: var(--a2ui-secondary-color);
}

/* Button Components */
.a2ui-button {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    padding: 0.5rem 1rem;
    font-size: 0.875rem;
    font-weight: 500;
    border-radius: var(--a2ui-border-radius);
    border: 1px solid transparent;
    cursor: pointer;
    transition: all 0.15s ease-in-out;
}

.a2ui-button-primary {
    background-color: var(--a2ui-primary-color);
    color: white;
}

.a2ui-button-primary:hover {
    opacity: 0.9;
}

.a2ui-button-secondary {
    background-color: transparent;
    color: var(--a2ui-text-color);
    border-color: var(--a2ui-secondary-color);
}

.a2ui-button-secondary:hover {
    background-color: rgba(0, 0, 0, 0.05);
}

/* Card Component */
.a2ui-card {
    background-color: var(--a2ui-background-color);
    border-radius: var(--a2ui-border-radius);
    border: 1px solid #e5e7eb;
    padding: 1rem;
    box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1);
}

/* Layout Components */
.a2ui-row {
    display: flex;
    flex-direction: row;
    gap: 0.5rem;
}

.a2ui-column {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
}

.a2ui-list {
    display: flex;
    gap: 0.5rem;
    overflow: auto;
}

/* Input Components */
.a2ui-textfield-container {
    display: flex;
    flex-direction: column;
    gap: 0.25rem;
}

.a2ui-textfield-label {
    font-size: 0.875rem;
    font-weight: 500;
    color: var(--a2ui-text-color);
}

.a2ui-textfield-input {
    padding: 0.5rem 0.75rem;
    border: 1px solid #d1d5db;
    border-radius: var(--a2ui-border-radius);
    font-size: 1rem;
    font-family: var(--a2ui-font-family);
}

.a2ui-textfield-input:focus {
    outline: none;
    border-color: var(--a2ui-primary-color);
    box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

/* Checkbox */
.a2ui-checkbox-container {
    display: flex;
    align-items: center;
    gap: 0.5rem;
}

.a2ui-checkbox-input {
    width: 1rem;
    height: 1rem;
    cursor: pointer;
}

.a2ui-checkbox-label {
    font-size: 0.875rem;
    cursor: pointer;
}

/* Image */
.a2ui-image {
    max-width: 100%;
    height: auto;
}

.a2ui-image-icon {
    width: 1.5rem;
    height: 1.5rem;
}

.a2ui-image-avatar {
    width: 2.5rem;
    height: 2.5rem;
    border-radius: 50%;
    object-fit: cover;
}

.a2ui-image-feature {
    width: 100%;
    height: auto;
    border-radius: var(--a2ui-border-radius);
}

/* Divider */
.a2ui-divider {
    border: none;
    background-color: #e5e7eb;
}

.a2ui-divider-horizontal {
    width: 100%;
    height: 1px;
    margin: 1rem 0;
}

.a2ui-divider-vertical {
    width: 1px;
    height: 100%;
    margin: 0 1rem;
}

/* Modal */
.a2ui-modal-backdrop {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
}

.a2ui-modal-content {
    background-color: var(--a2ui-background-color);
    border-radius: var(--a2ui-border-radius);
    padding: 1.5rem;
    max-width: 90%;
    max-height: 90%;
    overflow: auto;
    box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
}

/* Video & Audio */
.a2ui-video, .a2ui-audio {
    width: 100%;
    border-radius: var(--a2ui-border-radius);
}
";
    }
}

