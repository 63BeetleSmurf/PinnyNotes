using PinnyNotes.WpfUi.ViewModels;

namespace PinnyNotes.WpfUi.Messages;

public record CreateNewNoteMessage(
    NoteViewModel? ParentViewModel = null
);
