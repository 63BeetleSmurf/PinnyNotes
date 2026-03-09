namespace PinnyNotes.WpfUi.Controls.BackgroundSpellCheck;

public class BackgroundSpellingSuggestion
{
    public string Word { get; set; } = string.Empty;
    public BackgroundSpellingError Error { get; set; } = new();
}
