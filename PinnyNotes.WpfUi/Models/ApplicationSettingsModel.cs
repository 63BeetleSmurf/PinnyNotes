using PinnyNotes.WpfUi.Base;

namespace PinnyNotes.WpfUi.Models;

public class ApplicationSettingsModel : NotifyPropertyChangedBase
{
    // General
    public bool ShowNotifiyIcon { get => _showNotifiyIcon; set => SetProperty(ref _showNotifiyIcon, value); }
    private bool _showNotifiyIcon;
    public bool CheckForUpdates { get => _checkForUpdates; set => SetProperty(ref _checkForUpdates, value); }
    private bool _checkForUpdates;
}
