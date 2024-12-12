using System.Windows.Shapes;

namespace PinnyNotes.WpfUi.Models;

public class ThemeModel
{
    public ThemeModel(string key, string displayName, string menuIconColorHex, ThemeColorsModel lightColor, ThemeColorsModel darkColor)
    {
        Key = key;
        DisplayName = displayName;

        MenuIconColor = new()
        {
            ColorHex = menuIconColorHex
        };

        LightColor = lightColor;
        DarkColor = darkColor;
    }

    public string Key { get; set; }
    public string DisplayName { get; set; }

    public ColorModel MenuIconColor { get; }

    public Rectangle MenuIcon
    {
        get => new()
        {
            Width = 20,
            Height = 20,
            Fill = MenuIconColor.Brush
        };
    }

    public ThemeColorsModel LightColor { get; set; }
    public ThemeColorsModel DarkColor { get; set; }
}
