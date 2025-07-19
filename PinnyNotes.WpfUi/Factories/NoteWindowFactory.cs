using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.ViewModels;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Factories;

public class NoteWindowFactory(SettingsService settingsService, MessengerService messengerService, NoteViewModelFactory viewModelFactory)
{
    private readonly SettingsService _settingsService = settingsService;
    private readonly MessengerService _messengerService = messengerService;
    private readonly NoteViewModelFactory _viewModelFactory = viewModelFactory;

    public NoteWindow Create(NoteViewModel? parentViewModel = null)
    {
        return new NoteWindow(
            _settingsService,
            _messengerService,
            _viewModelFactory.Create(parentViewModel)
        );
    }
}
