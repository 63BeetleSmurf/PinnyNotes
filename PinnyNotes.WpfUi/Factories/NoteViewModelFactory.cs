using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.ViewModels;

namespace PinnyNotes.WpfUi.Factories;

public class NoteViewModelFactory(MessengerService messenger)
{
    private readonly MessengerService _messenger = messenger;

    public NoteViewModel Create(NoteViewModel? parentViewModel = null)
    {
        return new NoteViewModel(_messenger, parentViewModel);
    }
}
