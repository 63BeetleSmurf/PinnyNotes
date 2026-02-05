namespace PinnyNotes.WpfUi.Models;

public class ApplicationSettingsModel : BaseModel
{
    // General
    public bool ShowNotifiyIcon { get; set => SetProperty(ref field, value); }
    public bool CheckForUpdates { get; set => SetProperty(ref field, value); }
}
