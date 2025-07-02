using System.Windows.Media;

namespace PinnyNotes.WpfUi.Themes;

public class PaletteColor(string hex)
{
    public string Hex { get; set; } = hex;
    public Color Color => (Color)ColorConverter.ConvertFromString(Hex);
}
