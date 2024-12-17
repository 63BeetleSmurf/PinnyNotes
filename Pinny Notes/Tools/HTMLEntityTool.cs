using System;
using System.Net;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class HtmlEntityTool
{
    public const string Name = "HTML Entity";

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
        => WebUtility.HtmlEncode(text);

    private static string Decode(string text, object? additionalParam)
        => WebUtility.HtmlDecode(text);
}
