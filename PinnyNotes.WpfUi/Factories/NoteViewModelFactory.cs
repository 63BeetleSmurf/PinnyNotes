using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.ViewModels;

namespace PinnyNotes.WpfUi.Factories;

public class NoteViewModelFactory(AppMetadataService applicationDataService, SettingsService settingsService, MessengerService messengerService)
{
    private readonly AppMetadataService _appMetadataService = applicationDataService;
    private readonly SettingsService _settingsService = settingsService;
    private readonly MessengerService _messengerService = messengerService;

    public NoteViewModel Create(NoteViewModel? parentViewModel = null)
    {
        return new NoteViewModel(_appMetadataService, _settingsService, _messengerService, parentViewModel);
    }
}
