using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public partial class ColorTool : BaseTool, ITool
{
    private enum ToolActions
    {
        RgbToHex,
        HexToRgb
    }

    public ToolStates State => (ToolStates)Settings.Default.ColorToolState;

    public ColorTool(TextBox noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Color",
            [
                new ToolMenuAction("RGB to Hex", new RelayCommand(() => MenuAction(ToolActions.RgbToHex))),
                new ToolMenuAction("Hex to RGB", new RelayCommand(() => MenuAction(ToolActions.HexToRgb)))
            ]
        );
    }

    private void MenuAction(Enum action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        switch (action)
        {
            case ToolActions.RgbToHex:
                return RgbRegex().Replace(
                    text,
                    match => {
                        int r = int.Parse(match.Groups[1].Value);
                        int g = int.Parse(match.Groups[2].Value);
                        int b = int.Parse(match.Groups[3].Value);

                        if (!IsValidRgb(r, g, b))
                            return match.Value; // Leave as-is if invalid

                        return $"#{r:X2}{g:X2}{b:X2}";
                    }
                );
            case ToolActions.HexToRgb:
                return HexRegex().Replace(
                    text,
                    match =>
                    {
                        string hexValue = match.Groups["hex"].Value;
                        bool isShorthand = (hexValue.Length == 3);

                        int r = (isShorthand) ? Convert.ToInt32(new string(hexValue[0], 2), 16) : Convert.ToInt32(hexValue.Substring(0, 2), 16);
                        int g = (isShorthand) ? Convert.ToInt32(new string(hexValue[1], 2), 16) : Convert.ToInt32(hexValue.Substring(2, 2), 16);
                        int b = (isShorthand) ? Convert.ToInt32(new string(hexValue[2], 2), 16) : Convert.ToInt32(hexValue.Substring(4, 2), 16);

                        return $"{r}, {g}, {b}";
                    }
                );
        }

        return text;
    }

    private static bool IsValidRgb(int r, int g, int b)
    {
        return (
            r >= 0 && r <= 255
            && g >= 0 && g <= 255
            && b >= 0 && b <= 255
        );
    }

    [GeneratedRegex(@"(?:rgb\s*)?\(?\[?(\d{1,3})[\s,]+(\d{1,3})[\s,]+(\d{1,3})\)?\]?", RegexOptions.IgnoreCase)]
    private static partial Regex RgbRegex();
    [GeneratedRegex(@"#?(?<hex>[0-9a-fA-F]{6}|[0-9a-fA-F]{3})")]
    private static partial Regex HexRegex();
}
