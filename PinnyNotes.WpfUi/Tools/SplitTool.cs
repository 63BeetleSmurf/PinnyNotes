using System.Text.RegularExpressions;

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
        SplitSelected,
        SplitSelectedRegex
    }

    private string? _selectedText = null;

    public SplitTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Split",
            [
                new ToolMenuAction("Comma", new RelayCommand(() => MenuAction(ToolActions.SplitComma))),
                new ToolMenuAction("Space", new RelayCommand(() => MenuAction(ToolActions.SplitSpace))),
                new ToolMenuAction("Tab", new RelayCommand(() => MenuAction(ToolActions.SplitTab))),
                new ToolMenuAction("-"),
                new ToolMenuAction("Selected", new RelayCommand(() => MenuAction(ToolActions.SplitSelected))),
                new ToolMenuAction("Selected (Regex)", new RelayCommand(() => MenuAction(ToolActions.SplitSelectedRegex)))
            ]
        );
    }

    public ToolState State => ToolSettings.SplitToolState;

    private void MenuAction(ToolActions action)
    {
        if (action != ToolActions.SplitSelected && action != ToolActions.SplitSelectedRegex)
        {
            _selectedText = null;
        }
        else
        {
            _selectedText = NoteTextBox.SelectedText;
            NoteTextBox.SelectionLength = 0;
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
            case ToolActions.SplitSelectedRegex:
                if (!string.IsNullOrEmpty(_selectedText))
                {
                    try
                    {
                        return Regex.Replace(text, _selectedText, Environment.NewLine, RegexOptions.None, TimeSpan.FromSeconds(1));
                    }
                    catch (ArgumentException)
                    {
                        // Leave text unchanged if selection is not a valid pattern.
                    }
                    catch (RegexMatchTimeoutException)
                    {
                        // Leave text unchanged if pattern takes too long to match.
                    }
                }
                break;
        }

        return text;
    }
}
