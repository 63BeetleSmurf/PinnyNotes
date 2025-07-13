using System;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class SplitTool : BaseTool, ITool
{
    private enum ToolActions
    {
        SplitComma,
        SplitSpace,
        SplitTab,
        SplitSelected
    }

    private string? _selectedText = null;

    public ToolStates State => _settings.ToolSettings.SplitToolState;

    public SplitTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Split",
            [
                new ToolMenuAction("Comma", new RelayCommand(() => MenuAction(ToolActions.SplitComma))),
                new ToolMenuAction("Space", new RelayCommand(() => MenuAction(ToolActions.SplitSpace))),
                new ToolMenuAction("Tab", new RelayCommand(() => MenuAction(ToolActions.SplitTab))),
                new ToolMenuAction("-"),
                new ToolMenuAction("Selected", new RelayCommand(() => MenuAction(ToolActions.SplitSelected)))
            ]
        );
    }

    private void MenuAction(ToolActions action)
    {
        if (action != ToolActions.SplitSelected)
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
            case ToolActions.SplitComma:
                return text.Replace(",", Environment.NewLine);
            case ToolActions.SplitSpace:
                return text.Replace(" ", Environment.NewLine);
            case ToolActions.SplitTab:
                return text.Replace("\t", Environment.NewLine);
            case ToolActions.SplitSelected:
                if (!string.IsNullOrEmpty(_selectedText))
                    return text.Replace(_selectedText, Environment.NewLine);
                break;
        }

        return text;
    }
}
