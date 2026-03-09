namespace PinnyNotes.WpfUi.Controls.BackgroundSpellCheck;

public class BackgroundSpellingError
{
    public string Word { get; set; } = string.Empty;
    public int StartIndex { get; set; }
    public int Length { get; set; }
}
