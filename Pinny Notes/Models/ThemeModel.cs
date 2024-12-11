using PinnyNotes.WpfUi.Helpers;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PinnyNotes.WpfUi.Models;

public class ThemeModel
{
    public ThemeModel(string key, string displayName, string menuIconColor, ThemeColorModel lightColor, ThemeColorModel darkColor)
    {
        Key = key;
        DisplayName = displayName;

        MenuIconColorHex = menuIconColor;

        LightColor = lightColor;
        DarkColor = darkColor;
    }

    public string Key { get; set; }
    public string DisplayName { get; set; }

    // Menu Icon
    private string _menuIconColorHex = null!;
    public string MenuIconColorHex
    {
        get => _menuIconColorHex;
        set
        {
            _menuIconColorHex = value;
            _menuIconColor = ThemeHelper.HexToColor(_menuIconColorHex);
        }
    }

    private Color _menuIconColor;
    public Color TitleBarColor
    {
        get => _menuIconColor;
        set
        {
            _menuIconColor = value;
            _menuIconColorHex = ThemeHelper.ColorToHex(_menuIconColor);
        }
    }

    public SolidColorBrush MenuIconBrush
    {
        get => new SolidColorBrush(_menuIconColor);
    }

    public Rectangle MenuIcon
    {
        get => new()
        {
            Width = 20,
            Height = 20,
            Fill = MenuIconBrush
        };
    }

    public ThemeColorModel LightColor { get; set; }
    public ThemeColorModel DarkColor { get; set; }
}
