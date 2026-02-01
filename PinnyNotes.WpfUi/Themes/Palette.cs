using System.Windows.Media;

namespace PinnyNotes.WpfUi.Themes;

public class Palette(string borderHex, string backgroundHex, string titleBarHex, string buttonHex, string textHex)
{
    public Color Border { get; set; } = (Color)ColorConverter.ConvertFromString(borderHex);
    public Color Background { get; set; } = (Color)ColorConverter.ConvertFromString(backgroundHex);
    public Color Title { get; set; } = (Color)ColorConverter.ConvertFromString(titleBarHex);
    public Color Button { get; set; } = (Color)ColorConverter.ConvertFromString(buttonHex);
    public Color Text { get; set; } = (Color)ColorConverter.ConvertFromString(textHex);
}
