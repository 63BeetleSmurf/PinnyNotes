namespace PinnyNotes.Core.DataTransferObjects;

public record NoteDto(
    int Id,

    string Content,

    double X,
    double Y,
    double Width,
    double Height,

    int GravityX,
    int GravityY,

    string ThemeColourScheme,

    bool IsPinned,
    bool IsOpen,

    DateTime? ReminderTrigger
);
