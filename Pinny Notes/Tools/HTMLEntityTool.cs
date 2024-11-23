using System;
using System.Net;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public partial class HtmlEntityTool : BaseTool, ITool
{
    public bool IsEnabled => ToolSettings.Default.HtmlEntityToolEnabled;
    public bool IsFavourite => ToolSettings.Default.HtmlEntityToolFavourite;

    public enum ToolActions
    {
        EntityEncode,
        EntityDecode
    }

    public HtmlEntityTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "HTML Entity";
        _menuActions.Add(new("Encode", EntityEncodeMenuAction));
        _menuActions.Add(new("Decode", EntityDecodeMenuAction));
    }

    private void EntityEncodeMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.EntityEncode);
    private void EntityDecodeMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.EntityDecode);

    private string ModifyTextCallback(string text, Enum action)
    {
        switch (action)
        {
            case ToolActions.EntityEncode:
                return WebUtility.HtmlEncode(text);
            case ToolActions.EntityDecode:
                return WebUtility.HtmlDecode(text);
        }

        return text;
    }
}
