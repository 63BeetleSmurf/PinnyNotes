using System.Windows.Media;

using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Models;

public class ThemeModel
{
    public required string Name { get; set; }
    public ThemeColors ThemeColor { get; set; }
    public Color TitleBarColor { get; set; }
    public Color BackgroundColor { get; set; }
    public Color BorderColor { get; set; }
}
