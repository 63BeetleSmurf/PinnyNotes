using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class SplitTool
{
    public const string Name = "Split";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Comma", OnCommaMenuItemClick),
                new("Space", OnSpaceMenuItemClick),
                new("Tab", OnTabMenuItemClick),
                new("-"),
                new("Selected", OnSelectedMenuItemClick)
            ]
        );

    private static void OnCommaMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), SplitOnComma);
    private static void OnSpaceMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), SplitOnSpace);
    private static void OnTabMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), SplitOnTab);
    private static void OnSelectedMenuItemClick(object sender, EventArgs e)
    {
        NoteTextBoxControl noteTextBox = ToolHelper.GetNoteTextBoxFromSender(sender);
        string selectedText = noteTextBox.SelectedText;
        noteTextBox.SelectionLength = 0;
        ToolHelper.ApplyFunctionToNoteText(noteTextBox, SplitOnSelected, noteTextBox.SelectedText);
    }

    private static string SplitOnComma(string text, object? additionalParam)
        => text.Replace(",", Environment.NewLine);

    private static string SplitOnSpace(string text, object? additionalParam)
        => text.Replace(" ", Environment.NewLine);

    private static string SplitOnTab(string text, object? additionalParam)
        => text.Replace("\t", Environment.NewLine);

    private static string SplitOnSelected(string text, object? additionalParam)
    {
        string selectedText = additionalParam as string ?? "";
        if (string.IsNullOrEmpty(selectedText))
            return text;

        return text.Replace(selectedText, Environment.NewLine);
    }
}
