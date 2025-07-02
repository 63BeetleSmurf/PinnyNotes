namespace PinnyNotes.WpfUi.Themes;

public class NotePalette(string titleBarHex, string titleButtonHex, string backgroundHex, string borderHex, string textHex)
{
    public PaletteColor TitleBar { get; set; } = new(titleBarHex);
    public PaletteColor TitleButton { get; set; } = new(titleButtonHex);
    public PaletteColor Background { get; set; } = new(backgroundHex);
    public PaletteColor Border { get; set; } = new(borderHex);
    public PaletteColor Text { get; set; } = new(textHex);
}
