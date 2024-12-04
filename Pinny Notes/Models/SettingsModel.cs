using System.Collections.Generic;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

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
    public TransparencyModes TransparencyMode { get; set; }
    public bool OpaqueWhenFocused { get; set; }
    public ColorModes ColorMode { get; set; }
    public bool UseMonoFont { get; set; }
    public bool HideTitleBar { get; set; }
    public bool ShowTrayIcon { get; set; }
    public bool ShowNotesInTaskbar { get; set; }
    public bool CheckForUpdates { get; set; }

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

    public void LoadSettings()
    {
        StartupPosition = (StartupPositions)Settings.Default.StartupPosition;
        CycleColors = Settings.Default.CycleThemes;
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
        TransparencyMode = (TransparencyModes)Settings.Default.TransparencyMode;
        OpaqueWhenFocused = Settings.Default.OpaqueWhenFocused;
        ColorMode = (ColorModes)Settings.Default.ColorMode;
        UseMonoFont = Settings.Default.UseMonoFont;
        HideTitleBar = Settings.Default.HideTitleBar;
        ShowTrayIcon = Settings.Default.ShowTrayIcon;
        ShowNotesInTaskbar = Settings.Default.ShowNotesInTaskbar;
        CheckForUpdates = Settings.Default.CheckForUpdates;

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
    }
    public void SaveSettings()
    {
        Settings.Default.StartupPosition = (int)StartupPosition;
        Settings.Default.CycleThemes = CycleColors;
        Settings.Default.TrimCopiedText = TrimCopiedText;
        Settings.Default.TrimPastedText = TrimPastedText;
        Settings.Default.MiddleClickPaste = MiddleClickPaste;
        Settings.Default.AutoCopy = AutoCopy;
        Settings.Default.SpellCheck = SpellChecker;
        Settings.Default.NewLineAtEnd = NewLineAtEnd;
        Settings.Default.KeepNewLineAtEndVisible = KeepNewLineAtEndVisible;
        Settings.Default.AutoIndent = AutoIndent;
        Settings.Default.TabSpaces = TabSpaces;
        Settings.Default.TabWidth = TabWidth;
        Settings.Default.ConvertIndentation = ConvertIndentation;
        Settings.Default.MinimizeMode = (int)MinimizeMode;
        Settings.Default.TransparencyMode = (int)TransparencyMode;
        Settings.Default.OpaqueWhenFocused = OpaqueWhenFocused;
        Settings.Default.ColorMode = (int)ColorMode;
        Settings.Default.UseMonoFont = UseMonoFont;
        Settings.Default.HideTitleBar = HideTitleBar;
        Settings.Default.ShowTrayIcon = ShowTrayIcon;
        Settings.Default.ShowNotesInTaskbar = ShowNotesInTaskbar;
        Settings.Default.CheckForUpdates = CheckForUpdates;

        ToolSettings.Default.Base64ToolState = (int)Base64ToolState;
        ToolSettings.Default.BracketToolState = (int)BracketToolState;
        ToolSettings.Default.CaseToolState = (int)CaseToolState;
        ToolSettings.Default.DateTimeToolState = (int)DateTimeToolState;
        ToolSettings.Default.GibberishToolState = (int)GibberishToolState;
        ToolSettings.Default.HashToolState = (int)HashToolState;
        ToolSettings.Default.HtmlEntityToolState = (int)HtmlEntityToolState;
        ToolSettings.Default.IndentToolState = (int)IndentToolState;
        ToolSettings.Default.JoinToolState = (int)JoinToolState;
        ToolSettings.Default.JsonToolState = (int)JsonToolState;
        ToolSettings.Default.ListToolState = (int)ListToolState;
        ToolSettings.Default.QuoteToolState = (int)QuoteToolState;
        ToolSettings.Default.RemoveToolState = (int)RemoveToolState;
        ToolSettings.Default.SlashToolState = (int)SlashToolState;
        ToolSettings.Default.SortToolState = (int)SortToolState;
        ToolSettings.Default.SplitToolState = (int)SplitToolState;
        ToolSettings.Default.TrimToolState = (int)TrimToolState;

        Settings.Default.Save();
    }
}
