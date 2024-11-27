using System.Windows.Shapes;

namespace PinnyNotes.WpfUi.Models;

public class ThemeModel(string name, ThemeColorModel lightColor, ThemeColorModel darkColor)
{
    public string Name { get; set; } = name;

    public Rectangle MenuIcon
    {
        get => new()
        {
            Width = 20,
            Height = 20,
            Fill = LightColor.TitleBarBrush
        };
    }

    public ThemeColorModel LightColor { get; set; } = lightColor;
    public ThemeColorModel DarkColor { get; set; } = darkColor;
}
