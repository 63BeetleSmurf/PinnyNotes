using PinnyNotes.Core.Enums;

namespace PinnyNotes.Core.Models;

public record AppMetadataDataModel(
    int Id,

    long? LastUpdateCheck,
    ThemeColors ThemeColor
);
