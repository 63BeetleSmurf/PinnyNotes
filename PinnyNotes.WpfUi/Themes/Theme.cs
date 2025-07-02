using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Themes;

public class Theme(string name, ThemeColors themeColor, string menuIconHex)
{
    public string Name { get; set; } = name;

    public ThemeColors ThemeColor { get; set; } = themeColor;

    public PaletteColor MenuIcon { get; set; } = new(menuIconHex);

    public required NotePalette NoteLightPalette { get; set; }
    private NotePalette? _noteDarkPalette;
    public NotePalette NoteDarkPalette
    {
        get => _noteDarkPalette ?? NoteLightPalette;
        set => _noteDarkPalette = value;
    }
}
