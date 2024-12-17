using System;
using System.Text;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class SlashTool
{
    public const string Name = "Slash";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("All Forward (/)", OnAllForwardMenuItemClick),
                new("All Back (\\)", OnAllBackMenuItemClick),
                new("Swap", OnSwapMenuItemClick)
            ]
        );

    private static void OnAllForwardMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), ConvertAllToForwardSlash);
    private static void OnAllBackMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), ConvertAllToBackSlash);
    private static void OnSwapMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), SwapSlashes);

    private static string ConvertAllToForwardSlash(string text, object? additionalParam)
        => text.Replace('\\', '/');

    private static string ConvertAllToBackSlash(string text, object? additionalParam)
        => text.Replace('/', '\\');

    private static string SwapSlashes(string text, object? additionalParam)
        => SwapCharacters(text, '\\', '/');

    private static string SwapCharacters(string text, char character1, char? character2 = null)
    {
        StringBuilder stringBuilder = new(text.Length);

        foreach (char currentChar in text)
        {
            if (currentChar == character1)
                stringBuilder.Append(character2);
            else if (currentChar == character2)
                stringBuilder.Append(character1);
            else
                stringBuilder.Append(currentChar);
        }

        return stringBuilder.ToString();
    }
}
