using System.Drawing.Text;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;

public class SettingsViewModel
{
    private readonly SettingsService _settingsService;

    public SettingsViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;

        ApplicationSettings = _settingsService.ApplicationSettings;
        NoteSettings = _settingsService.NoteSettings;
        EditorSettings = _settingsService.EditorSettings;
        ToolSettings = _settingsService.ToolSettings;

        IsTransparencyEnabled = (NoteSettings.TransparencyMode != TransparencyMode.Disabled);
    }

    public ApplicationSettingsModel ApplicationSettings { get; set; }
    public NoteSettingsModel NoteSettings { get; set; }
    public EditorSettingsModel EditorSettings { get; set; }
    public ToolSettingsModel ToolSettings { get; set; }

    public static KeyValuePair<StartupBehaviour, string>[] StartupBehaviourList { get; } = [
        new(StartupBehaviour.RestoreOpenNotes, "Restore open notes"),
        new(StartupBehaviour.CreateNewNote, "Create new note"),
        new(StartupBehaviour.ShowManagementWindow, "Show Management Window")
    ];

    public static KeyValuePair<NewInstanceBehaviour, string>[] NewInstanceBehaviourList { get; } = [
        new(NewInstanceBehaviour.CreateNewNote, "Create new note"),
        new(NewInstanceBehaviour.ShowManagementWindow, "Show Management Window")
    ];

    public static KeyValuePair<StartupPosition, string>[] StartupPositionsList { get; } = [
        new(StartupPosition.TopLeft, "Top left"),
        new(StartupPosition.TopCentre, "Top centre"),
        new(StartupPosition.TopRight, "Top right"),
        new(StartupPosition.MiddleLeft, "Middle left"),
        new(StartupPosition.MiddleCentre, "Middle centre"),
        new(StartupPosition.MiddleRight, "Middle right"),
        new(StartupPosition.BottomLeft, "Bottom left"),
        new(StartupPosition.BottomCentre, "Bottom centre"),
        new(StartupPosition.BottomRight, "Bottom right")
    ];

    public static KeyValuePair<MinimizeMode, string>[] MinimizeModeList { get; } = [
        new(MinimizeMode.Allow, "Yes"),
        new(MinimizeMode.Prevent, "No"),
        new(MinimizeMode.PreventIfPinned, "When not pinned")
    ];

    public static KeyValuePair<VisibilityMode, string>[] VisibilityModeList { get; } = [
        new(VisibilityMode.ShowInTaskbar, "Show in taskbar"),
        new(VisibilityMode.HideInTaskbar, "Hide in taskbar"),
        new(VisibilityMode.HideInTaskbarAndTaskSwitcher, "Hide in taskbar and task switcher")
    ];

    public static KeyValuePair<ColourMode, string>[] ColourModeList { get; } = [
        new(ColourMode.Light, "Light"),
        new(ColourMode.Dark, "Dark"),
        new(ColourMode.System, "System default")
    ];

    public static KeyValuePair<TransparencyMode, string>[] TransparencyModeList { get; } = [
        new(TransparencyMode.Disabled, "Disabled"),
        new(TransparencyMode.Enabled, "Enabled"),
        new(TransparencyMode.WhenPinned, "Only when pinned")
    ];

    public static KeyValuePair<string, string>[] FontFamilyList { get; } = [..
        new InstalledFontCollection().Families
                                     .Select(f => new KeyValuePair<string, string>(f.Name, f.Name))
    ];

    public static KeyValuePair<CopyAction, string>[] CopyActionList { get; } = [
        new(CopyAction.None, "None"),
        new(CopyAction.CopySelected, "Copy selected"),
        new(CopyAction.CopyLine, "Copy line"),
        new(CopyAction.CopyAll, "Copy all")
    ];

    public static KeyValuePair<PasteAction, string>[] PasteActionList { get; } = [
        new(PasteAction.None, "None"),
        new(PasteAction.Paste, "Paste"),
        new(PasteAction.PasteAndReplaceAll, "Paste and replace all"),
        new(PasteAction.PasteAtEnd, "Paste at end")
    ];

    public static KeyValuePair<CopyFallbackAction, string>[] CopyFallbackActionList { get; } = [
        new(CopyFallbackAction.None, "None"),
        new(CopyFallbackAction.CopyLine, "Copy line"),
        new(CopyFallbackAction.CopyNote, "Copy note")
    ];

    public static KeyValuePair<ToolState, string>[] ToolStateList { get; } = [
        new(ToolState.Disabled, "Disabled"),
        new(ToolState.Enabled, "Enabled"),
        new(ToolState.Favourite, "Favourite")
    ];

    public bool IsStartWithWindowsEnabled { get; } = (((App)App.Current).ApplicationMode == ApplicationMode.Normal);

    public bool IsTransparencyEnabled { get; set; }
}
