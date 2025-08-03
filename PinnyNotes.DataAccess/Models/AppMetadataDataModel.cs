using PinnyNotes.Core.Enums;

namespace PinnyNotes.DataAccess.Models;

public record AppMetadataDataModel(
    int Id,

    long? LastUpdateCheck,
    ThemeColors ThemeColor
);
