using PinnyNotes.Core.Enums;

namespace PinnyNotes.WpfUi.Messages;

public record ReminderSetMessage(
    int NoteId,
    DateTime ReminderTrigger
);
