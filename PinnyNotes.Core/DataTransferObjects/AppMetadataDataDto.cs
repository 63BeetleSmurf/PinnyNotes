namespace PinnyNotes.Core.DataTransferObjects;

public record AppMetadataDataDto(
    int Id,

    long? LastUpdateCheck,
    string? ColorScheme
);
