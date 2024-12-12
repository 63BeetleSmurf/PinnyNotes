namespace PinnyNotes.WpfUi.Models;

public class ThemeColorsModel
{
    public ThemeColorsModel(string titleBarColorHex, string titleBarButtonsColorHex, string backgroundColorHex, string textColorHex, string borderColorHex)
    {
        TitleBarColor = new()
        {
            ColorHex = titleBarColorHex
        };

        TitleBarButtonsColor = new()
        {
            ColorHex = titleBarButtonsColorHex
        };

        BackgroundColor = new()
        {
            ColorHex = backgroundColorHex
        };

        TextColor = new()
        {
            ColorHex = textColorHex
        };

        BorderColor = new()
        {
            ColorHex = borderColorHex
        };
    }

    public ColorModel TitleBarColor { get; }
    public ColorModel TitleBarButtonsColor { get; }
    public ColorModel BackgroundColor { get; }
    public ColorModel TextColor { get; }
    public ColorModel BorderColor { get; }
}
