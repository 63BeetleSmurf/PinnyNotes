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

    string? ThemeColorScheme,

    bool IsPinned,
    bool IsOpen
);
