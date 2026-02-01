using PinnyNotes.Core.Enums;

namespace PinnyNotes.Core.Models;

public record AppMetadataDataDto(
    int Id,

    long? LastUpdateCheck,
    ThemeColors ThemeColor
);
