using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Messages;

public record OpenNoteWindowMessage(
    int? NoteId = null,
    NoteModel? ParentNote = null
);
