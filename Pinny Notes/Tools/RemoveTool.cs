using System;
using System.Collections.Generic;
using System.Text;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public partial class RemoveTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)ToolSettings.Default.RemoveToolState;

    private string? _selectedText = null;

    public enum ToolActions
    {
        RemoveSpaces,
        RemoveTabs,
        RemoveNewLines,
        RemoveForwardSlashes,
        RemoveBackSlashes,
        RemoveAllSlashes,
        RemoveSelected
    }

    public RemoveTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        _name = "Remove";
        _menuActions.Add(new("Spaces", RemoveSpacesMenuAction));
        _menuActions.Add(new("Tabs", RemoveTabsMenuAction));
        _menuActions.Add(new("New Lines", RemoveNewLinesMenuAction));
        _menuActions.Add(new("-"));
        _menuActions.Add(new("Forward Slashes (/)", RemoveForwardSlashesMenuAction));
        _menuActions.Add(new("Back Slashes (\\)", RemoveBackSlashesMenuAction));
        _menuActions.Add(new("All Slashes", RemoveAllSlashesMenuAction));
        _menuActions.Add(new("-"));
        _menuActions.Add(new("Selected", RemoveSelectedMenuAction));
    }

    private void RemoveSpacesMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.RemoveSpaces);
    private void RemoveTabsMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.RemoveTabs);
    private void RemoveNewLinesMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.RemoveNewLines);
    private void RemoveForwardSlashesMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.RemoveForwardSlashes);
    private void RemoveBackSlashesMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.RemoveBackSlashes);
    private void RemoveAllSlashesMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.RemoveAllSlashes);
    private void RemoveSelectedMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.RemoveSelected);

    private void MenuAction(Enum action)
    {
        switch (action)
        {
            case ToolActions.RemoveSelected:
                _selectedText = null;
                break;
            default:
                _selectedText = _noteTextBox.SelectedText;
                _noteTextBox.SelectionLength = 0;
                break;
        }

        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        switch (action)
        {
            case ToolActions.RemoveSpaces:
                return RemoveCharacters(text, [' ']);
            case ToolActions.RemoveTabs:
                return RemoveCharacters(text, ['\t']);
            case ToolActions.RemoveNewLines:
                return RemoveCharacters(text, ['\r', '\n']);
            case ToolActions.RemoveForwardSlashes:
                return RemoveCharacters(text, ['/']);
            case ToolActions.RemoveBackSlashes:
                return RemoveCharacters(text, ['\\']);
            case ToolActions.RemoveAllSlashes:
                return RemoveCharacters(text, ['\\', '/']);
            case ToolActions.RemoveSelected:
                if (!string.IsNullOrEmpty(_selectedText))
                    return RemoveString(text, _selectedText);
                break;
        }

        return text;
    }

    private string RemoveCharacters(string text, HashSet<char> character)
    {
        StringBuilder stringBuilder = new(text.Length);

        foreach (char currentChar in text)
        {
            if (!character.Contains(currentChar))
                stringBuilder.Append(currentChar);
        }

        return stringBuilder.ToString();
    }

    private string RemoveString(string text, string toRemove)
    {
        return text.Replace(toRemove, "");
    }
}
