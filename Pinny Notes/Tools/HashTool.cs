using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Tools;

public partial class HashTool : BaseTool, ITool
{
    public ToolStates State => ToolStates.Disabled; // (ToolStates)ToolSettings.Default.HashToolState;

    public enum ToolActions
    {
        HashSHA512,
        HashSHA384,
        HashSHA256,
        HashSHA1,
        HashMD5
    }

    public HashTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Hash";
        _menuActions.Add(new("SHA512", HashSHA512MenuAction));
        _menuActions.Add(new("SHA384", HashSHA384MenuAction));
        _menuActions.Add(new("SHA256", HashSHA256MenuAction));
        _menuActions.Add(new("SHA1", HashSHA1MenuAction));
        _menuActions.Add(new("MD5", HashMD5MenuAction));
    }

    private void HashSHA512MenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.HashSHA512);
    private void HashSHA384MenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.HashSHA384);
    private void HashSHA256MenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.HashSHA256);
    private void HashSHA1MenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.HashSHA1);
    private void HashMD5MenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.HashMD5);

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

        return BitConverter.ToString(
            hasher.ComputeHash(Encoding.UTF8.GetBytes(_noteTextBox.Text))
        ).Replace("-", "");
    }
}
