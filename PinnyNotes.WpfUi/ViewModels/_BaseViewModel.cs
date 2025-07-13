using PinnyNotes.WpfUi.Base;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;

public abstract class BaseViewModel : NotifyPropertyChangedBase
{
    protected AppMetadataService AppMetadata { get; }
    protected SettingsService Settings { get; }
    protected MessengerService Messenger { get; }

    protected BaseViewModel(AppMetadataService appMetadata, SettingsService settings, MessengerService messenger)
    {
        AppMetadata = appMetadata;
        Settings = settings;
        Messenger = messenger;
    }
}
