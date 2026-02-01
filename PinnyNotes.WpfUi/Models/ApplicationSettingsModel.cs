using PinnyNotes.WpfUi.Base;

namespace PinnyNotes.WpfUi.Models;

public class ApplicationSettingsModel : NotifyPropertyChangedBase
{
    // General
    public bool ShowNotifiyIcon { get; set => SetProperty(ref field, value); }
    public bool CheckForUpdates { get; set => SetProperty(ref field, value); }
}
