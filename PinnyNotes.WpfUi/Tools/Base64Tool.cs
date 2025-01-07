using System;
using System.Text;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Tools;

public static class Base64Tool
{
    public const string Name = "Base64";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Encode", OnEncodeMenuItemClick),
                new("Decode", OnDecodeMenuItemClick)
            ]
        );

    private static void OnEncodeMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), Encode);

    private static void OnDecodeMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), Decode);

    private static string Encode(string text, object? additionalParam)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(textBytes);
    }

    private static string Decode(string text, object? additionalParam)
    {
        try
        {
            byte[] base64Bytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(base64Bytes);
        }
        catch
        {
            return text;
        }
    }
}
