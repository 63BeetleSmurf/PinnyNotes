using PinnyNotes.Core.Enums;

namespace PinnyNotes.DataAccess.Models;

public class AppMetadataDataModel
{
    public int Id { get; init; }

    public long? LastUpdateCheck { get; set; }
    public ThemeColors ThemeColor { get; set; }
}
