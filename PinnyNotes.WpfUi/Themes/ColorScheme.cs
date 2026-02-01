using System.Windows.Media;
using System.Windows.Shapes;

namespace PinnyNotes.WpfUi.Themes;

public class ColorScheme(string name, string iconColorHex, Palette light, Palette dark)
{
    private const int IconSize = 16;

    public string Name { get; set; } = name;
    public Color IconColor { get; set; } = (Color)ColorConverter.ConvertFromString(iconColorHex);
    public Palette Light { get; set; } = light;
    public Palette Dark { get; set; } = dark;

    public Rectangle Icon
    {
        get
        {
            _icon ??= new()
            {
                Width = IconSize,
                Height = IconSize,
                Fill = new SolidColorBrush(IconColor)
            };

            return _icon;
        }
    }
    private Rectangle? _icon = null;
}
