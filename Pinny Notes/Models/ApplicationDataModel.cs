namespace PinnyNotes.WpfUi.Models;

public class ApplicationDataModel
{
    public int Id { get; init; }

    public long? LastUpdateCheck { get; set; }
    public string? LastThemeColorKey { get; set; }
}
