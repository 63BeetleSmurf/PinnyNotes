using PinnyNotes.Core.Enums;

namespace PinnyNotes.Core.DataTransferObjects;

public record AppMetadataDataDto(
    int Id,

    long? LastUpdateCheck,
    ThemeColors ThemeColor
);
