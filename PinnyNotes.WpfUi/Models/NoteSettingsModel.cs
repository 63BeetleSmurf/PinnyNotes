using PinnyNotes.Core.Enums;

namespace PinnyNotes.WpfUi.Models;

public class NoteSettingsModel
{
    // General
    public double DefaultWidth { get; set; }
    public double DefaultHeight { get; set; }
    public StartupPositions StartupPosition { get; set; }
    public MinimizeModes MinimizeMode { get; set; }
    public bool HideTitleBar { get; set; }
    public bool ShowInTaskBar { get; set; }

    // Theme
    public bool CycleColors {  get; set; }
    public ColorModes ColorMode { get; set; }

    // Transparency
    public TransparencyModes TransparencyMode { get; set; }
    public bool OpaqueWhenFocused { get; set; }
    public double OpaqueValue { get; set; }
    public double TransparentValue { get; set; }
}
