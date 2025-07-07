using System;
using System.Collections.Generic;
using System.Text;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class RemoveTool : BaseTool, ITool
{
    private enum ToolActions
    {
        RemoveSpaces,
        RemoveTabs,
        RemoveNewLines,
        RemoveForwardSlashes,
        RemoveBackSlashes,
        RemoveAllSlashes,
        RemoveSelected
    }

    private string? _selectedText = null;

    public ToolStates State => _settings.AppSettings.RemoveState;

    public RemoveTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Remove",
            [
                new ToolMenuAction("Spaces", new RelayCommand(() => MenuAction(ToolActions.RemoveSpaces))),
                new ToolMenuAction("Tabs", new RelayCommand(() => MenuAction(ToolActions.RemoveTabs))),
                new ToolMenuAction("New Lines", new RelayCommand(() => MenuAction(ToolActions.RemoveNewLines))),
                new ToolMenuAction("-"),
                new ToolMenuAction("Forward Slashes (/)", new RelayCommand(() => MenuAction(ToolActions.RemoveForwardSlashes))),
                new ToolMenuAction("Back Slashes (\\)", new RelayCommand(() => MenuAction(ToolActions.RemoveBackSlashes))),
                new ToolMenuAction("All Slashes", new RelayCommand(() => MenuAction(ToolActions.RemoveAllSlashes))),
                new ToolMenuAction("-"),
                new ToolMenuAction("Selected", new RelayCommand(() => MenuAction(ToolActions.RemoveSelected)))
            ]
        );
    }

    private void MenuAction(ToolActions action)
    {
        if (action != ToolActions.RemoveSelected)
        {
            _selectedText = null;
        }
        else
        {
            _selectedText = _noteTextBox.SelectedText;
            _noteTextBox.SelectionLength = 0;
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
