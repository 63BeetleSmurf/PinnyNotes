using System;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class IndentTool
{
    public const string Name = "Indent";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("2 Spaces", On2SpacesMenuItemClick),
                new("4 Spaces", On4SpacesMenuItemClick),
                new("Tab", OnTabMenuItemClick)
            ]
        );

    private static void On2SpacesMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), Indent2Spaces);
    private static void On4SpacesMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), Indent4Spaces);
    private static void OnTabMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), IndentTab);

    private static string? Indent2Spaces(string line, int index)
        => $"  {line}";

    private static string? Indent4Spaces(string line, int index)
        => $"    {line}";

    private static string? IndentTab(string line, int index)
        => $"\t{line}";
}
