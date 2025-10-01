using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Base;

namespace PinnyNotes.WpfUi.Models;

public class NoteSettingsModel : NotifyPropertyChangedBase
{
    // General
    public double DefaultWidth { get => _defaultWidth; set => SetProperty(ref _defaultWidth, value); }
    private double _defaultWidth;
    public double DefaultHeight { get => _defaultHeight; set => SetProperty(ref _defaultHeight, value); }
    private double _defaultHeight;
    public StartupPositions StartupPosition { get => _startupPosition; set => SetProperty(ref _startupPosition, value); }
    private StartupPositions _startupPosition;
    public MinimizeModes MinimizeMode { get => _minimizeMode; set => SetProperty(ref _minimizeMode, value); }
    private MinimizeModes _minimizeMode;
    public VisibilityModes VisibilityMode { get => _visibilityMode; set => SetProperty(ref _visibilityMode, value); }
    private VisibilityModes _visibilityMode;
    public bool HideTitleBar { get => _hideTitleBar; set => SetProperty(ref _hideTitleBar, value); }
    private bool _hideTitleBar;

    // Theme
    public bool CycleColors { get => _cycleColors; set => SetProperty(ref _cycleColors, value); }
    private bool _cycleColors;
    public ColorModes ColorMode { get => _colorMode; set => SetProperty(ref _colorMode, value); }
    private ColorModes _colorMode;

    // Transparency
    public TransparencyModes TransparencyMode { get => _transparencyMode; set => SetProperty(ref _transparencyMode, value); }
    private TransparencyModes _transparencyMode;
    public bool OpaqueWhenFocused { get => _opaqueWhenFocused; set => SetProperty(ref _opaqueWhenFocused, value); }
    private bool _opaqueWhenFocused;
    public double OpaqueValue { get => _opaqueValue; set => SetProperty(ref _opaqueValue, value); }
    private double _opaqueValue;
    public double TransparentValue { get => _transparentValue; set => SetProperty(ref _transparentValue, value); }
    private double _transparentValue;
}
