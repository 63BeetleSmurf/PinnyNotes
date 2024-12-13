using System;
using System.Text;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public partial class SlashTool : BaseTool, ITool
{
    public ToolStates State { get; }

    public enum ToolActions
    {
        SlashAllForward,
        SlashAllBack,
        SlashSwap
    }

    public SlashTool(NoteTextBoxControl noteTextBox, ToolStates state) : base(noteTextBox)
    {
        State = state;
        _name = "Slash";
        _menuActions.Add(new("All Forward (/)", SlashAllForwardMenuAction));
        _menuActions.Add(new("All Back (\\)", SlashAllBackMenuAction));
        _menuActions.Add(new("Swap", SlashSwapMenuAction));
    }

    private void SlashAllForwardMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.SlashAllForward);
    private void SlashAllBackMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.SlashAllBack);
    private void SlashSwapMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.SlashSwap);

    private string ModifyTextCallback(string text, Enum action)
    {
        switch (action)
        {
            case ToolActions.SlashAllForward:
                return text.Replace('\\', '/');
            case ToolActions.SlashAllBack:
                return text.Replace('/', '\\');
            case ToolActions.SlashSwap:
                return SwapCharacters(text, '\\', '/');
        }

        return text;
    }

    private string SwapCharacters(string text, char character1, char? character2 = null)
    {
        StringBuilder stringBuilder = new(text.Length);

        foreach (char currentChar in text)
        {
            if (currentChar == character1)
                stringBuilder.Append(character2);
            else if (currentChar == character2)
                stringBuilder.Append(character1);
            else
                stringBuilder.Append(currentChar);
        }

        return stringBuilder.ToString();
    }
}
