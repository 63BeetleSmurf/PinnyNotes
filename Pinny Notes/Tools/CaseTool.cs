using System;
using System.Globalization;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public partial class CaseTool : BaseTool, ITool
{
    public bool IsEnabled => ToolSettings.Default.CaseToolEnabled;
    public bool IsFavourite => ToolSettings.Default.CaseToolFavourite;

    public enum ToolActions
    {
        CaseLower,
        CaseUpper,
        CaseTitle
    }

    public CaseTool(TextBox noteTextBox) : base(noteTextBox)
    {
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
