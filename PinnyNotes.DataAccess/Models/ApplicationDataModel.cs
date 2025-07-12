using PinnyNotes.Core.Enums;

namespace PinnyNotes.DataAccess.Models;

public class ApplicationDataModel
{
    public int Id { get; init; }

    public long? LastUpdateCheck { get; set; }
    public ThemeColors ThemeColor { get; set; }
}
