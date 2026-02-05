using PinnyNotes.Core.Enums;

namespace PinnyNotes.WpfUi.Models;

public class NoteSettingsModel : BaseModel
{
    // General
    public double DefaultWidth { get; set => SetProperty(ref field, value); }
    public double DefaultHeight { get; set => SetProperty(ref field, value); }
    public StartupPositions StartupPosition { get; set => SetProperty(ref field, value); }
    public MinimizeModes MinimizeMode { get; set => SetProperty(ref field, value); }
    public VisibilityModes VisibilityMode { get; set => SetProperty(ref field, value); }
    public bool HideTitleBar { get; set => SetProperty(ref field, value); }

    // Theme
    public bool CycleColors { get; set => SetProperty(ref field, value); }
    public ColorModes ColorMode { get; set => SetProperty(ref field, value); }

    // Transparency
    public TransparencyModes TransparencyMode { get; set => SetProperty(ref field, value); }
    public bool OpaqueWhenFocused { get; set => SetProperty(ref field, value); }
    public double OpaqueValue { get; set => SetProperty(ref field, value); }
    public double TransparentValue { get; set => SetProperty(ref field, value); }
}
