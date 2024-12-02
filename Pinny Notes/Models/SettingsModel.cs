using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;
using System.Collections.Generic;

namespace PinnyNotes.WpfUi.Models;

public class SettingsModel
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

    private static readonly KeyValuePair<ToolStates, string>[] _toolStateList = [
        new(ToolStates.Enabled, "Enabled"),
        new(ToolStates.Disabled, "Disabled"),
        new(ToolStates.Favorite, "Favorite")
    ];

    public SettingsModel(NoteModel? noteWindow = null)
    {
        LoadSettings();
        InitWindowPosition(noteWindow);
    }

    private void LoadSettings()
    {
        StartupPosition = (StartupPositions)Settings.Default.StartupPosition;
        CycleColors = Settings.Default.CycleColors;
        TrimCopiedText = Settings.Default.TrimCopiedText;
        TrimPastedText = Settings.Default.TrimPastedText;
        MiddleClickPaste = Settings.Default.MiddleClickPaste;
        AutoCopy = Settings.Default.AutoCopy;
        SpellChecker = Settings.Default.SpellCheck;
        NewLineAtEnd = Settings.Default.NewLineAtEnd;
        KeepNewLineAtEndVisible = Settings.Default.KeepNewLineAtEndVisible;
        AutoIndent = Settings.Default.AutoIndent;
        TabSpaces = Settings.Default.TabSpaces;
        TabWidth = Settings.Default.TabWidth;
        ConvertIndentation = Settings.Default.ConvertIndentation;
        MinimizeMode = (MinimizeModes)Settings.Default.MinimizeMode;
        TransparentNotes = Settings.Default.TransparentNotes;
        OpaqueWhenFocused = Settings.Default.OpaqueWhenFocused;
        OnlyTransparentWhenPinned = Settings.Default.OnlyTransparentWhenPinned;
        ColorMode = (ColorModes)Settings.Default.ColorMode;
        UseMonoFont = Settings.Default.UseMonoFont;
        HideTitleBar = Settings.Default.HideTitleBar;
        ShowTrayIcon = Settings.Default.ShowTrayIcon;
        ShowNotesInTaskbar = Settings.Default.ShowNotesInTaskbar;
        CheckForUpdates = Settings.Default.CheckForUpdates;

        #region Tools

        Base64ToolState = (ToolStates)ToolSettings.Default.Base64ToolState;
        BracketToolState = (ToolStates)ToolSettings.Default.BracketToolState;
        CaseToolState = (ToolStates)ToolSettings.Default.CaseToolState;
        DateTimeToolState = (ToolStates)ToolSettings.Default.DateTimeToolState;
        GibberishToolState = (ToolStates)ToolSettings.Default.GibberishToolState;
        HashToolState = (ToolStates)ToolSettings.Default.HashToolState;
        HtmlEntityToolState = (ToolStates)ToolSettings.Default.HtmlEntityToolState;
        IndentToolState = (ToolStates)ToolSettings.Default.IndentToolState;
        JoinToolState = (ToolStates)ToolSettings.Default.JoinToolState;
        JsonToolState = (ToolStates)ToolSettings.Default.JsonToolState;
        ListToolState = (ToolStates)ToolSettings.Default.ListToolState;
        QuoteToolState = (ToolStates)ToolSettings.Default.QuoteToolState;
        RemoveToolState = (ToolStates)ToolSettings.Default.RemoveToolState;
        SlashToolState = (ToolStates)ToolSettings.Default.SlashToolState;
        SortToolState = (ToolStates)ToolSettings.Default.SortToolState;
        SplitToolState = (ToolStates)ToolSettings.Default.SplitToolState;
        TrimToolState = (ToolStates)ToolSettings.Default.TrimToolState;

        #endregion
    }

    private void InitWindowPosition(NoteModel? parent = null)
    {
    }

    public StartupPositions StartupPosition { get; set; }
    public bool CycleColors { get; set; }
    public bool TrimCopiedText { get; set; }
    public bool TrimPastedText { get; set; }
    public bool MiddleClickPaste { get; set; }
    public bool AutoCopy { get; set; }
    public bool SpellChecker { get; set; }
    public bool NewLineAtEnd { get; set; }
    public bool KeepNewLineAtEndVisible { get; set; }
    public bool AutoIndent { get; set; }
    public bool TabSpaces { get; set; }
    public int TabWidth { get; set; }
    public bool ConvertIndentation { get; set; }
    public MinimizeModes MinimizeMode { get; set; }
    public bool TransparentNotes { get; set; }
    public bool OpaqueWhenFocused { get; set; }
    public bool OnlyTransparentWhenPinned { get; set; }
    public ColorModes ColorMode { get; set; }
    public bool UseMonoFont { get; set; }
    public bool HideTitleBar { get; set; }
    public bool ShowTrayIcon { get; set; }
    public bool ShowNotesInTaskbar { get; set; }
    public bool CheckForUpdates { get; set; }

    #region Tools

    public ToolStates Base64ToolState { get; set; }
    public ToolStates BracketToolState { get; set; }
    public ToolStates CaseToolState { get; set; }
    public ToolStates DateTimeToolState { get; set; }
    public ToolStates GibberishToolState { get; set; }
    public ToolStates HashToolState { get; set; }
    public ToolStates HtmlEntityToolState { get; set; }
    public ToolStates IndentToolState { get; set; }
    public ToolStates JoinToolState { get; set; }
    public ToolStates JsonToolState { get; set; }
    public ToolStates ListToolState { get; set; }
    public ToolStates QuoteToolState { get; set; }
    public ToolStates RemoveToolState { get; set; }
    public ToolStates SlashToolState { get; set; }
    public ToolStates SortToolState { get; set; }
    public ToolStates SplitToolState { get; set; }
    public ToolStates TrimToolState { get; set; }

    #endregion
}
