using System.Windows.Media;

using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Models;

public class ColorModel
{
    private string _colorHex = null!;
    public string ColorHex {
        get => _colorHex;
        set
        {
            _colorHex = value;
            _color = ThemeHelper.HexToColor(_colorHex);
        }
    }

    private Color _color;
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            _colorHex = ThemeHelper.ColorToHex(_color);
        }
    }

    public SolidColorBrush Brush
    {
        get => new SolidColorBrush(_color);
    }
}
