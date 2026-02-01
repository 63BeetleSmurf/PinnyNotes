using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Base;

namespace PinnyNotes.WpfUi.Models;

public class AppMetadataModel : NotifyPropertyChangedBase
{
    public long? LastUpdateCheck { get; set => SetProperty(ref field, value); }
    public string? ColorScheme { get => field; set => SetProperty(ref field, value); } = "";
}
