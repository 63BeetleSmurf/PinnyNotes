namespace PinnyNotes.WpfUi.Models;

public class AppMetadataModel : BaseModel
{
    public long? LastUpdateCheck { get; set => SetProperty(ref field, value); }
    public string? ColorScheme { get => field; set => SetProperty(ref field, value); } = "";
}
