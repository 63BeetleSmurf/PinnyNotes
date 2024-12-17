using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class RemoveTool
{
    public const string Name = "Remove";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Spaces", OnSpacesMenuItemClick),
                new("Tabs", OnTabsMenuItemClick),
                new("New Lines", OnNewLinesMenuItemClick),
                new("-"),
                new("Forward Slashes (/)", OnForwardSlashesMenuItemClick),
                new("Back Slashes (\\)", OnBackSlashesMenuItemClick),
                new("All Slashes", OnAllSlashesMenuItemClick),
                new("-"),
                new("Selected", OnSelectedMenuItemClick)
            ]
        );

    private static void OnSpacesMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), RemoveSpaces);
    private static void OnTabsMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), RemoveTabs);
    private static void OnNewLinesMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), RemoveNewLines);
    private static void OnForwardSlashesMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), RemoveForwardSlashes);
    private static void OnBackSlashesMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), RemoveBackSlashes);
    private static void OnAllSlashesMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), RemoveAllSlashes);
    private static void OnSelectedMenuItemClick(object sender, EventArgs e)
    {
        NoteTextBoxControl noteTextBox = ToolHelper.GetNoteTextBoxFromSender(sender);
        string selectedText = noteTextBox.SelectedText;
        noteTextBox.SelectionLength = 0;
        ToolHelper.ApplyFunctionToNoteText(noteTextBox, RemoveSelected, noteTextBox.SelectedText);
    }

    private static string RemoveSpaces(string text, object? additionalParam)
    => RemoveCharacters(text, [' ']);

    private static string RemoveTabs(string text, object? additionalParam)
    => RemoveCharacters(text, ['\t']);
    private static string RemoveNewLines(string text, object? additionalParam)
    => RemoveCharacters(text, ['\r', '\n']);

    private static string RemoveForwardSlashes(string text, object? additionalParam)
    => RemoveCharacters(text, ['/']);

    private static string RemoveBackSlashes(string text, object? additionalParam)
        => RemoveCharacters(text, ['\\']);

    private static string RemoveAllSlashes(string text, object? additionalParam)
        => RemoveCharacters(text, ['\\', '/']);

    private static string RemoveSelected(string text, object? additionalParam)
    {
        string selectedText = additionalParam as string ?? "";
        if (string.IsNullOrEmpty(selectedText))
            return text;

        return text.Replace(text, selectedText);
    }

    private static string RemoveCharacters(string text, HashSet<char> character)
    {
        StringBuilder stringBuilder = new(text.Length);

        foreach (char currentChar in text)
        {
            if (!character.Contains(currentChar))
                stringBuilder.Append(currentChar);
        }

        return stringBuilder.ToString();
    }
}
