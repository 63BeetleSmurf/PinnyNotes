using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

// TO DO: Need to fix split by selected text. Possibly TextAction needs to get both text and selected text.

public partial class SplitTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)ToolSettings.Default.SplitToolState;

    private string? _selectedText = null;

    public enum ToolActions
    {
        SplitComma,
        SplitSpace,
        SplitTab,
        SplitSelected
    }

    public SplitTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Split";
        _menuActions.Add(new("Comma", SplitCommaMenuAction));
        _menuActions.Add(new("Space", SplitSpaceMenuAction));
        _menuActions.Add(new("Tab", SplitTabMenuAction));
        _menuActions.Add(new("-"));
        _menuActions.Add(new("Selected", SplitSelectedMenuAction));
    }

    private void SplitCommaMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.SplitComma);
    private void SplitSpaceMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.SplitSpace);
    private void SplitTabMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.SplitTab);
    private void SplitSelectedMenuAction(object sender, EventArgs e) => MenuAction(ToolActions.SplitSelected);

    private void MenuAction(Enum action)
    {
        switch (action)
        {
            case (ToolActions.SplitSelected):
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
