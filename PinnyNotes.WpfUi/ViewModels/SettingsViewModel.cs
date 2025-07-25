using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;

public class SettingsViewModel : BaseViewModel, INotifyPropertyChanged
{
    private static readonly KeyValuePair<StartupPositions, string>[] _startupPositionsList = [
        new(StartupPositions.TopLeft, "Top Left"),
        new(StartupPositions.TopCenter, "Top Center"),
        new(StartupPositions.TopRight, "Top Right"),
        new(StartupPositions.MiddleLeft, "Middle Left"),
        new(StartupPositions.MiddleCenter, "Middle Center"),
        new(StartupPositions.MiddleRight, "Middle Right"),
        new(StartupPositions.BottomLeft, "Bottom Left"),
        new(StartupPositions.BottomCenter, "Bottom Center"),
        new(StartupPositions.BottomRight, "Bottom Right")
    ];

    private static readonly KeyValuePair<MinimizeModes, string>[] _minimizeModeList = [
        new(MinimizeModes.Allow, "Yes"),
        new(MinimizeModes.Prevent, "No"),
        new(MinimizeModes.PreventIfPinned, "When not pinned")
    ];

    private static readonly KeyValuePair<ColorModes, string>[] _colorModeList = [
        new(ColorModes.Light, "Light"),
        new(ColorModes.Dark, "Dark"),
        new(ColorModes.System, "System Default")
    ];

    private static readonly KeyValuePair<TransparencyModes, string>[] _transparencyModeList = [
        new(TransparencyModes.Disabled, "Disabled"),
        new(TransparencyModes.Enabled, "Enabled"),
        new(TransparencyModes.WhenPinned, "Only when pinned")
    ];

    private static readonly KeyValuePair<string, string>[] _fontFamilyList
        = new InstalledFontCollection().Families
                                       .Select(f => new KeyValuePair<string, string>(f.Name, f.Name))
                                       .ToArray();

    private static readonly KeyValuePair<CopyActions, string>[] _copyActionList = [
        new(CopyActions.CopySelected, "Copy selected"),
        new(CopyActions.CopyLine, "Copy line"),
        new(CopyActions.CopyAll, "Copy all")
    ];

    private static readonly KeyValuePair<CopyFallbackActions, string>[] _copyFallbackActionList = [
        new(CopyFallbackActions.None, "None"),
        new(CopyFallbackActions.CopyLine, "Copy line"),
        new(CopyFallbackActions.CopyNote, "Copy note")
    ];

    private static readonly KeyValuePair<ToolStates, string>[] _toolStateList = [
        new(ToolStates.Disabled, "Disabled"),
        new(ToolStates.Enabled, "Enabled"),
        new(ToolStates.Favourite, "Favourite")
    ];

    public SettingsViewModel(
        AppMetadataService appMetadata, SettingsService settingsService, MessengerService messengerService
        ) : base(appMetadata, settingsService, messengerService)
    {
        Settings = new()
        {
            ApplicationSettings = SettingsService.ApplicationSettings,
            NoteSettings = SettingsService.NoteSettings,
            EditorSettings = SettingsService.EditorSettings,
            ToolSettings = SettingsService.ToolSettings
        };

        _isTransparencyEnabled = (Settings.NoteSettings.TransparencyMode != TransparencyModes.Disabled);
    }

    public SettingsModel Settings { get; set; }

    public KeyValuePair<StartupPositions, string>[] StartupPositionsList => _startupPositionsList;
    public KeyValuePair<MinimizeModes, string>[] MinimizeModeList => _minimizeModeList;
    public KeyValuePair<ColorModes, string>[] ColorModeList => _colorModeList;
    public KeyValuePair<TransparencyModes, string>[] TransparencyModeList => _transparencyModeList;
    public KeyValuePair<string, string>[] FontFamilyList => _fontFamilyList;
    public KeyValuePair<CopyActions, string>[] CopyActionList => _copyActionList;
    public KeyValuePair<CopyFallbackActions, string>[] CopyFallbackActionList => _copyFallbackActionList;
    public KeyValuePair<ToolStates, string>[] ToolStateList => _toolStateList;

    public bool IsTransparencyEnabled
    {
        get => _isTransparencyEnabled;
        set => SetProperty(ref _isTransparencyEnabled, value);
    }
    private bool _isTransparencyEnabled;
}
