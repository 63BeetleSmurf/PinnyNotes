using PinnyNotes.Core.Enums;

namespace PinnyNotes.WpfUi.Models;

public class NoteSettingsModel : BaseModel
{
    // General
    public double DefaultWidth { get; set => SetProperty(ref field, value); }
    public double DefaultHeight { get; set => SetProperty(ref field, value); }
    public StartupPosition StartupPosition { get; set => SetProperty(ref field, value); }
    public bool PinnedByDefault { get; set => SetProperty(ref field, value); }
    public MinimizeMode MinimizeMode { get; set => SetProperty(ref field, value); }
    public VisibilityMode VisibilityMode { get; set => SetProperty(ref field, value); }
    public bool HideTitleBar { get; set => SetProperty(ref field, value); }
    public NoteTitleBarItem[] TitleBarItems { get; set => SetProperty(ref field, value); } = [];

    // Theme
    public bool CycleColours { get; set => SetProperty(ref field, value); }
    public ColourMode ColourMode { get; set => SetProperty(ref field, value); }

    // Transparency
    public TransparencyMode TransparencyMode { get; set => SetProperty(ref field, value); }
    public bool OpaqueWhenFocused { get; set => SetProperty(ref field, value); }
    public double OpaqueValue { get; set => SetProperty(ref field, value); }
    public double TransparentValue { get; set => SetProperty(ref field, value); }
}
