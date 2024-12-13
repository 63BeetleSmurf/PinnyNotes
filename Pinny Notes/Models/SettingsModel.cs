using System.Collections.Generic;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Models;

public class SettingsModel
{
    public static readonly KeyValuePair<StartupPositions, string>[] StartupPositionsList = [
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

    public static readonly KeyValuePair<MinimizeModes, string>[] MinimizeModeList = [
        new(MinimizeModes.Allow, "Yes"),
        new(MinimizeModes.Prevent, "No"),
        new(MinimizeModes.PreventIfPinned, "When not pinned")
    ];

    public static readonly KeyValuePair<string, string>[] DefaultColorList = ThemeHelper.GetDefaultColorList();

    public static readonly KeyValuePair<ColorModes, string>[] ColorModeList = [
        new(ColorModes.Light, "Light"),
        new(ColorModes.Dark, "Dark"),
        new(ColorModes.System, "System Default")
    ];

    public static readonly KeyValuePair<TransparencyModes, string>[] TransparencyModeList = [
        new(TransparencyModes.Disabled, "Disabled"),
        new(TransparencyModes.Enabled, "Enabled"),
        new(TransparencyModes.OnlyWhenPinned, "Only when pinned")
    ];

    public static readonly KeyValuePair<ToolStates, string>[] ToolStateList = [
        new(ToolStates.Enabled, "Enabled"),
        new(ToolStates.Disabled, "Disabled"),
        new(ToolStates.Favorite, "Favorite")
    ];

    public int Id { get; init; }

    public bool Application_TrayIcon { get; set; }
    public bool Application_NotesInTaskbar { get; set; }
    public bool Application_CheckForUpdates { get; set; }

    public int Notes_DefaultWidth { get; set; }
    public int Notes_DefaultHeight { get; set; }
    public StartupPositions Notes_StartupPosition { get; set; }
    public MinimizeModes Notes_MinimizeMode { get; set; }
    public bool Notes_HideTitleBar { get; set; }
    public string Notes_DefaultThemeColorKey { get; set; } = null!;
    public ColorModes Notes_ColorMode { get; set; }
    public TransparencyModes Notes_TransparencyMode { get; set; }
    public bool Notes_OpaqueWhenFocused { get; set; }
    public double Notes_TransparentOpacity { get; set; }
    public double Notes_OpaqueOpacity { get; set; }

    public bool Editor_UseMonoFont { get; set; }
    public string Editor_MonoFontFamily { get; set; } = null!;
    public bool Editor_SpellCheck { get; set; }
    public bool Editor_AutoIndent { get; set; }
    public bool Editor_NewLineAtEnd { get; set; }
    public bool Editor_KeepNewLineVisible { get; set; }
    public bool Editor_TabsToSpaces { get; set; }
    public bool Editor_ConvertIndentationOnPaste { get; set; }
    public int Editor_TabWidth { get; set; }
    public bool Editor_MiddleClickPaste { get; set; }
    public bool Editor_TrimPastedText { get; set; }
    public bool Editor_TrimCopiedText { get; set; }
    public bool Editor_CopyHighlightedText { get; set; }

    public ToolStates Tool_Base64State { get; set; }
    public ToolStates Tool_BracketState { get; set; }
    public ToolStates Tool_CaseState { get; set; }
    public ToolStates Tool_DateTimeState { get; set; }
    public ToolStates Tool_GibberishState { get; set; }
    public ToolStates Tool_HashState { get; set; }
    public ToolStates Tool_HtmlEntityState { get; set; }
    public ToolStates Tool_IndentState { get; set; }
    public ToolStates Tool_JoinState { get; set; }
    public ToolStates Tool_JsonState { get; set; }
    public ToolStates Tool_ListState { get; set; }
    public ToolStates Tool_QuoteState { get; set; }
    public ToolStates Tool_RemoveState { get; set; }
    public ToolStates Tool_SlashState { get; set; }
    public ToolStates Tool_SortState { get; set; }
    public ToolStates Tool_SplitState { get; set; }
    public ToolStates Tool_TrimState { get; set; }
}
