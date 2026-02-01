using System.Text;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class SlashTool : BaseTool, ITool
{
    private enum ToolActions
    {
        SlashAllForward,
        SlashAllBack,
        SlashSwap
    }

    public ToolStates State => ToolSettings.SlashToolState;

    public SlashTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Slash",
            [
                new ToolMenuAction("All Forward (/)", new RelayCommand(() => MenuAction(ToolActions.SlashAllForward))),
                new ToolMenuAction("All Back (\\)", new RelayCommand(() => MenuAction(ToolActions.SlashAllBack))),
                new ToolMenuAction("Swap", new RelayCommand(() => MenuAction(ToolActions.SlashSwap)))
            ]
        );
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        return action switch
        {
            ToolActions.SlashAllForward => text.Replace('\\', '/'),
            ToolActions.SlashAllBack => text.Replace('/', '\\'),
            ToolActions.SlashSwap => SwapCharacters(text, '\\', '/'),
            _ => text,
        };
    }

    private static string SwapCharacters(string text, char character1, char? character2 = null)
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
