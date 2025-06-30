using System;
using System.Globalization;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class CaseTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)Settings.Default.CaseToolState;

    public enum ToolActions
    {
        CaseLower,
        CaseUpper,
        CaseTitle
    }

    public CaseTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Case";
        _menuActions.Add(new("Lower", new RelayCommand(() => MenuAction(ToolActions.CaseLower))));
        _menuActions.Add(new("Upper", new RelayCommand(() => MenuAction(ToolActions.CaseUpper))));
        _menuActions.Add(new("Title", new RelayCommand(() => MenuAction(ToolActions.CaseTitle))));
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

        switch (action)
        {
            case ToolActions.CaseLower:
                return textInfo.ToLower(text);
            case ToolActions.CaseUpper:
                return textInfo.ToUpper(text);
            case ToolActions.CaseTitle:
                return textInfo.ToTitleCase(text);
        }

        return text;
    }
}
