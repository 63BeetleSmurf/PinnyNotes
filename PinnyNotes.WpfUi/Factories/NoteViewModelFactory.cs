using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.ViewModels;

namespace PinnyNotes.WpfUi.Factories;

public class NoteViewModelFactory(ApplicationDataService applicationData, SettingsService settings, MessengerService messenger)
{
    private readonly ApplicationDataService _applicationData = applicationData;
    private readonly SettingsService _settings = settings;
    private readonly MessengerService _messenger = messenger;

    public NoteViewModel Create(NoteViewModel? parentViewModel = null)
    {
        return new NoteViewModel(_applicationData, _settings, _messenger, parentViewModel);
    }
}
