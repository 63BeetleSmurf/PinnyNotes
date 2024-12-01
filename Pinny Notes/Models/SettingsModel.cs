using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;
using System.Drawing.Imaging;

namespace PinnyNotes.WpfUi.Models;

public class SettingsModel
{
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

        Base64ToolState = ToolSettings.Default.Base64ToolState;
        BracketToolState = ToolSettings.Default.BracketToolState;
        CaseToolState = ToolSettings.Default.CaseToolState;
        DateTimeToolState = ToolSettings.Default.DateTimeToolState;
        GibberishToolState = ToolSettings.Default.GibberishToolState;
        HashToolState = ToolSettings.Default.HashToolState;
        HtmlEntityToolState = ToolSettings.Default.HtmlEntityToolState;
        IndentToolState = ToolSettings.Default.IndentToolState;
        JoinToolState = ToolSettings.Default.JoinToolState;
        JsonToolState = ToolSettings.Default.JsonToolState;
        ListToolState = ToolSettings.Default.ListToolState;
        QuoteToolState = ToolSettings.Default.QuoteToolState;
        RemoveToolState = ToolSettings.Default.RemoveToolState;
        SlashToolState = ToolSettings.Default.SlashToolState;
        SortToolState = ToolSettings.Default.SortToolState;
        SplitToolState = ToolSettings.Default.SplitToolState;
        TrimToolState = ToolSettings.Default.TrimToolState;

        #endregion
    }

    private void InitWindowPosition(NoteModel? parent = null)
    {
    }

    private StartupPositions StartupPosition { get; set; }
    private bool CycleColors { get; set; }
    private bool TrimCopiedText { get; set; }
    private bool TrimPastedText { get; set; }
    private bool MiddleClickPaste { get; set; }
    private bool AutoCopy { get; set; }
    private bool SpellChecker { get; set; }
    private bool NewLineAtEnd { get; set; }
    private bool KeepNewLineAtEndVisible { get; set; }
    private bool AutoIndent { get; set; }
    private bool TabSpaces { get; set; }
    private int TabWidth { get; set; }
    private bool ConvertIndentation { get; set; }
    private MinimizeModes MinimizeMode { get; set; }
    private bool TransparentNotes { get; set; }
    private bool OpaqueWhenFocused { get; set; }
    private bool OnlyTransparentWhenPinned { get; set; }
    private ColorModes ColorMode { get; set; }
    private bool UseMonoFont { get; set; }
    private bool HideTitleBar { get; set; }
    private bool ShowTrayIcon { get; set; }
    private bool ShowNotesInTaskbar { get; set; }
    private bool CheckForUpdates { get; set; }

    #region Tools

    private ToolState Base64ToolState { get; set; }
    private ToolState BracketToolState { get; set; }
    private ToolState CaseToolState { get; set; }
    private ToolState DateTimeToolState { get; set; }
    private ToolState GibberishToolState { get; set; }
    private ToolState HashToolState { get; set; }
    private ToolState HtmlEntityToolState { get; set; }
    private ToolState IndentToolState { get; set; }
    private ToolState JoinToolState { get; set; }
    private ToolState JsonToolState { get; set; }
    private ToolState ListToolState { get; set; }
    private ToolState QuoteToolState { get; set; }
    private ToolState RemoveToolState { get; set; }
    private ToolState SlashToolState { get; set; }
    private ToolState SortToolState { get; set; }
    private ToolState SplitToolState { get; set; }
    private ToolState TrimToolState { get; set; }

    #endregion
}
