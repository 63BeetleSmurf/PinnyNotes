using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.ViewModels;

namespace PinnyNotes.WpfUi.Factories;

public class NoteViewModelFactory(AppMetadataService applicationData, SettingsService settings, MessengerService messenger)
{
    private readonly AppMetadataService _appMetadata = applicationData;
    private readonly SettingsService _settings = settings;
    private readonly MessengerService _messenger = messenger;

    public NoteViewModel Create(NoteViewModel? parentViewModel = null)
    {
        return new NoteViewModel(_appMetadata, _settings, _messenger, parentViewModel);
    }
}
