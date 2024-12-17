using System;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class ListTool
{
    public const string Name = "List";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Enumerate", OnEnumerateMenuItemClick),
                new("Dash", OnDashMenuItemClick),
                new("Remove", OnRemoveMenuItemClick)
            ]
        );

    private static void OnEnumerateMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), AddEnumeration);
    private static void OnDashMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), AddDashes);
    private static void OnRemoveMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), RemoveAll);

    private static string? AddEnumeration(string line, int index)
        => $"{index + 1}. {line}";

    private static string? AddDashes(string line, int index)
        => $"- {line}";

    private static string? RemoveAll(string line, int index)
        => line[(line.IndexOf(' ') + 1)..];
}
