using System;
using System.Globalization;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class CaseTool : BaseTool, ITool
{
    private enum ToolActions
    {
        CaseLower,
        CaseUpper,
        CaseTitle
    }

    public ToolStates State => _settings.ToolSettings.CaseToolState;

    public CaseTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Case",
            [
                new ToolMenuAction("Lower", new RelayCommand(() => MenuAction(ToolActions.CaseLower))),
                new ToolMenuAction("Upper", new RelayCommand(() => MenuAction(ToolActions.CaseUpper))),
                new ToolMenuAction("Title", new RelayCommand(() => MenuAction(ToolActions.CaseTitle)))
            ]
        );
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
