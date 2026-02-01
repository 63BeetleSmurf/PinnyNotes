using System.Security.Cryptography;
using System.Text;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class HashTool : BaseTool, ITool
{
    private enum ToolActions
    {
        HashSHA512,
        HashSHA384,
        HashSHA256,
        HashSHA1,
        HashMD5
    }
    public ToolStates State => ToolSettings.HashToolState;

    public HashTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Hash",
            [
                new ToolMenuAction("SHA512", new RelayCommand(() => MenuAction(ToolActions.HashSHA512))),
                new ToolMenuAction("SHA384", new RelayCommand(() => MenuAction(ToolActions.HashSHA384))),
                new ToolMenuAction("SHA256", new RelayCommand(() => MenuAction(ToolActions.HashSHA256))),
                new ToolMenuAction("SHA1", new RelayCommand(() => MenuAction(ToolActions.HashSHA1))),
                new ToolMenuAction("MD5", new RelayCommand(() => MenuAction(ToolActions.HashMD5)))
            ]
        );
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        HashAlgorithm hasher;
        switch (action)
        {
            case ToolActions.HashSHA512:
                hasher = SHA512.Create();
                break;
            case ToolActions.HashSHA384:
                hasher = SHA384.Create();
                break;
            case ToolActions.HashSHA256:
                hasher = SHA256.Create();
                break;
            case ToolActions.HashSHA1:
                hasher = SHA1.Create();
                break;
            case ToolActions.HashMD5:
                hasher = MD5.Create();
                break;
            default:
                return text;
        }

        return Convert.ToHexString(
            hasher.ComputeHash(
                Encoding.UTF8.GetBytes(NoteTextBox.Text)
            )
        );
    }
}
