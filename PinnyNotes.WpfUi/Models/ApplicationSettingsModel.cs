using PinnyNotes.Core.Enums;

namespace PinnyNotes.WpfUi.Models;

public class ApplicationSettingsModel : BaseModel
{
    // General
    public bool StartWithWindows { get; set => SetProperty(ref field, value); }
    public StartupBehaviour StartupBehaviour { get; set => SetProperty(ref field, value); }
    public NewInstanceBehaviour NewInstanceBehaviour { get; set => SetProperty(ref field, value); }
    public bool ShowNotifyIcon { get; set => SetProperty(ref field, value); }
    public bool CheckForUpdates { get; set => SetProperty(ref field, value); }
}
