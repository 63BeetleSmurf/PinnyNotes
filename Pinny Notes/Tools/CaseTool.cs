using System;
using System.Globalization;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public partial class CaseTool : BaseTool, ITool
{
    public ToolStates State { get; }

    public enum ToolActions
    {
        CaseLower,
        CaseUpper,
        CaseTitle
    }

    public CaseTool(NoteTextBoxControl noteTextBox, ToolStates state) : base(noteTextBox)
    {
        State = state;
        _name = "Case";
        _menuActions.Add(new("Lower", CaseLowerMenuAction));
        _menuActions.Add(new("Upper", CaseUpperMenuAction));
        _menuActions.Add(new("Title", CaseTitleMenuAction));
    }

    private void CaseLowerMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.CaseLower);
    private void CaseUpperMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.CaseUpper);
    private void CaseTitleMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.CaseTitle);

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
