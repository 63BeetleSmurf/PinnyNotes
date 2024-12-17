using System;
using System.Globalization;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Tools;

public static class CaseTool
{
    public const string Name = "Case";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Lower", OnLowerMenuItemClick),
                new("Upper", OnUpperMenuItemClick),
                new("Title", OnTitleMenuItemClick)
            ]
        );

    private static void OnLowerMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), LowerCase);
    private static void OnUpperMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), UpperCase);
    private static void OnTitleMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), TitleCase);

    private static string LowerCase(string text, object? additionalParam)
        => CultureInfo.CurrentCulture.TextInfo.ToLower(text);

    private static string UpperCase(string text, object? additionalParam)
        => CultureInfo.CurrentCulture.TextInfo.ToUpper(text);

    private static string TitleCase(string text, object? additionalParam)
        => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
}
