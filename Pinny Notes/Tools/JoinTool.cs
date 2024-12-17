using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Tools;

public static class JoinTool
{
    public const string Name = "Join";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Comma", OnCommaMenuItemClick),
                new("Space", OnSpaceMenuItemClick),
                new("Tab", OnTabMenuItemClick)
            ]
        );

    private static void OnCommaMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), JoinComma);
    private static void OnSpaceMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), JoinSpace);
    private static void OnTabMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), JoinTab);

    private static string JoinComma(string text, object? additionalParam)
        => text.Replace(Environment.NewLine, ",");

    private static string JoinSpace(string text, object? additionalParam)
        => text.Replace(Environment.NewLine, " ");

    private static string JoinTab(string text, object? additionalParam)
        => text.Replace(Environment.NewLine, "\t");
}
