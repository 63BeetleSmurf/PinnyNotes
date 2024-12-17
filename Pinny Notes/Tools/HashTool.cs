using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Tools;

public static class HashTool
{
    public const string Name = "Hash";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("SHA512", OnSHA512MenuItemClick),
                new("SHA384", OnSHA384MenuItemClick),
                new("SHA256", OnSHA256MenuItemClick),
                new("SHA1", OnSHA1MenuItemClick),
                new("MD5", OnMD5MenuItemClick)
            ]
        );

    private static void OnSHA512MenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), HashSHA512);
    private static void OnSHA384MenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), HashSHA384);
    private static void OnSHA256MenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), HashSHA256);
    private static void OnSHA1MenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), HashSHA1);
    private static void OnMD5MenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), HashMD5);

    private static string HashSHA512(string text, object? additionalParam)
        => BitConverter.ToString(
            SHA512.HashData(Encoding.UTF8.GetBytes(text))
        ).Replace("-", "");

    private static string HashSHA384(string text, object? additionalParam)
        => BitConverter.ToString(
            SHA384.HashData(Encoding.UTF8.GetBytes(text))
        ).Replace("-", "");

    private static string HashSHA256(string text, object? additionalParam)
        => BitConverter.ToString(
            SHA256.HashData(Encoding.UTF8.GetBytes(text))
        ).Replace("-", "");

    private static string HashSHA1(string text, object? additionalParam)
        => BitConverter.ToString(
            SHA1.HashData(Encoding.UTF8.GetBytes(text))
        ).Replace("-", "");

    private static string HashMD5(string text, object? additionalParam)
        => BitConverter.ToString(
            MD5.HashData(Encoding.UTF8.GetBytes(text))
        ).Replace("-", "");
}
