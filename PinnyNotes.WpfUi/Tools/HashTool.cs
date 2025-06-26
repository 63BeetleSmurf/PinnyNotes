using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class HashTool : BaseTool, ITool
{
    public bool IsEnabled => ToolSettings.Default.HashToolEnabled;
    public bool IsFavourite => ToolSettings.Default.HashToolFavourite;

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
        _menuActions.Add(new("SHA512", new RelayCommand(() => MenuAction(ToolActions.HashSHA512))));
        _menuActions.Add(new("SHA384", new RelayCommand(() => MenuAction(ToolActions.HashSHA384))));
        _menuActions.Add(new("SHA256", new RelayCommand(() => MenuAction(ToolActions.HashSHA256))));
        _menuActions.Add(new("SHA1", new RelayCommand(() => MenuAction(ToolActions.HashSHA1))));
        _menuActions.Add(new("MD5", new RelayCommand(() => MenuAction(ToolActions.HashMD5))));
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

        return BitConverter.ToString(
            hasher.ComputeHash(Encoding.UTF8.GetBytes(_noteTextBox.Text))
        ).Replace("-", "");
    }
}
