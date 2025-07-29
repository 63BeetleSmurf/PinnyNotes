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
    public static KeyValuePair<StartupPositions, string>[] StartupPositionsList { get; } = [
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

    public static KeyValuePair<MinimizeModes, string>[] MinimizeModeList { get; } = [
        new(MinimizeModes.Allow, "Yes"),
        new(MinimizeModes.Prevent, "No"),
        new(MinimizeModes.PreventIfPinned, "When not pinned")
    ];

    public static KeyValuePair<ColorModes, string>[] ColorModeList { get; } = [
        new(ColorModes.Light, "Light"),
        new(ColorModes.Dark, "Dark"),
        new(ColorModes.System, "System Default")
    ];

    public static KeyValuePair<TransparencyModes, string>[] TransparencyModeList { get; } = [
        new(TransparencyModes.Disabled, "Disabled"),
        new(TransparencyModes.Enabled, "Enabled"),
        new(TransparencyModes.WhenPinned, "Only when pinned")
    ];

    public static KeyValuePair<string, string>[] FontFamilyList { get; }
        = new InstalledFontCollection().Families
                                       .Select(f => new KeyValuePair<string, string>(f.Name, f.Name))
                                       .ToArray();

    public static KeyValuePair<CopyActions, string>[] CopyActionList { get; } = [
        new(CopyActions.None, "None"),
        new(CopyActions.CopySelected, "Copy selected"),
        new(CopyActions.CopyLine, "Copy line"),
        new(CopyActions.CopyAll, "Copy all")
    ];

    public static KeyValuePair<PasteActions, string>[] PasteActionList { get; } = [
        new(PasteActions.None, "None"),
        new(PasteActions.Paste, "Paste"),
        new(PasteActions.PasteAndReplaceAll, "Paste and replace all"),
        new(PasteActions.PasteAtEnd, "Paste at end")
    ];

    public static KeyValuePair<CopyFallbackActions, string>[] CopyFallbackActionList { get; } = [
        new(CopyFallbackActions.None, "None"),
        new(CopyFallbackActions.CopyLine, "Copy line"),
        new(CopyFallbackActions.CopyNote, "Copy note")
    ];

    public static KeyValuePair<ToolStates, string>[] ToolStateList { get; } = [
        new(ToolStates.Disabled, "Disabled"),
        new(ToolStates.Enabled, "Enabled"),
        new(ToolStates.Favourite, "Favourite")
    ];

    public SettingsViewModel(
        AppMetadataService appMetadata, SettingsService settingsService, MessengerService messengerService
        ) : base(appMetadata, settingsService, messengerService)
    {
        Settings = new(
            SettingsService.ApplicationSettings,
            SettingsService.NoteSettings,
            SettingsService.EditorSettings,
            SettingsService.ToolSettings
        );

        _isTransparencyEnabled = (Settings.NoteSettings.TransparencyMode != TransparencyModes.Disabled);
    }

    public SettingsModel Settings { get; set; }

    public bool IsTransparencyEnabled
    {
        get => _isTransparencyEnabled;
        set => SetProperty(ref _isTransparencyEnabled, value);
    }
    private bool _isTransparencyEnabled;
}
