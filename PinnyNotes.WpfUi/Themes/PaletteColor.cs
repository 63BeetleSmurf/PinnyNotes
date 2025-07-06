using System.Windows.Media;

namespace PinnyNotes.WpfUi.Themes;

public class PaletteColor(string hex)
{
    private string _hex = hex;
    public string Hex
    {
        get => _hex;
        set
        {
            _hex = value;
            _color = null;
            _brush = null;
        }
    }

    private Color? _color = null;
    public Color Color
    {
        get
        {
            _color ??= (Color)ColorConverter.ConvertFromString(Hex);
            return (Color)_color;
        }
    }

    private SolidColorBrush? _brush = null;
    public SolidColorBrush Brush
    {
        get
        {
            _brush ??= new(Color);
            return _brush;
        }
    }
}
