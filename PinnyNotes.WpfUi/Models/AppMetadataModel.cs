using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Base;

namespace PinnyNotes.WpfUi.Models;

public class AppMetadataModel : NotifyPropertyChangedBase
{
    public long? LastUpdateCheck { get => _lastUpdateCheck; set => SetProperty(ref _lastUpdateCheck, value); }
    private long? _lastUpdateCheck;
    public ThemeColors ThemeColor { get => _themeColor; set => SetProperty(ref _themeColor, value); }
    private ThemeColors _themeColor;
}
