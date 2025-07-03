using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.ViewModels;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Factories;

public class NoteWindowFactory(MessengerService messenger, NoteViewModelFactory viewModelFactory)
{
    private readonly MessengerService _messenger = messenger;
    private readonly NoteViewModelFactory _viewModelFactory = viewModelFactory;

    public NoteWindow Create(NoteViewModel? parentViewModel = null)
    {
        return new NoteWindow(
            _messenger,
            _viewModelFactory.Create(parentViewModel)
        );
    }
}
