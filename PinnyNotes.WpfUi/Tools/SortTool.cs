using System;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class SortTool : BaseTool, ITool
{
    private enum ToolActions
    {
        SortAscending,
        SortDescending
    }

    public ToolStates State => _settings.ToolSettings.SortToolState;

    public SortTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Sort",
            [
                new ToolMenuAction("Ascending", new RelayCommand(() => MenuAction(ToolActions.SortAscending))),
                new ToolMenuAction("Descending", new RelayCommand(() => MenuAction(ToolActions.SortDescending)))
            ]
        );
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        switch (action)
        {
            case ToolActions.SortAscending:
                return SortNoteText(text);
            case ToolActions.SortDescending:
                return SortNoteText(text, true);
        }

        return text;
    }

    private string SortNoteText(string text, bool reverse = false)
    {
        string[] lines = text.Split(Environment.NewLine);
        Array.Sort(lines);
        if (reverse)
            Array.Reverse(lines);
        return string.Join(Environment.NewLine, lines);
    }
}
