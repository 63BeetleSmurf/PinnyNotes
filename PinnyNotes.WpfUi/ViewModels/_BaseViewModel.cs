using PinnyNotes.WpfUi.Base;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;

public abstract class BaseViewModel : NotifyPropertyChangedBase
{
    protected AppMetadataService AppMetadataService { get; }
    protected SettingsService SettingsService { get; }
    protected MessengerService MessengerService { get; }

    protected BaseViewModel(AppMetadataService appMetadataService, SettingsService settingsService, MessengerService messengerService)
    {
        AppMetadataService = appMetadataService;
        SettingsService = settingsService;
        MessengerService = messengerService;
    }
}
