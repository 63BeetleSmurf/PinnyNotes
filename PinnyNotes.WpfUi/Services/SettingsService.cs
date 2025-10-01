using PinnyNotes.DataAccess.Models;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Services;

public class SettingsService
{
    private readonly DatabaseService _databaseService;

    public ApplicationSettingsModel ApplicationSettings { get; }
    public NoteSettingsModel NoteSettings { get; }
    public EditorSettingsModel EditorSettings { get; }
    public ToolSettingsModel ToolSettings { get; }

    public SettingsService(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        SettingsDataModel settings = _databaseService.SettingsRepository.GetById(1);

        ApplicationSettings = new()
        {
            ShowNotifiyIcon = settings.ShowTrayIcon,
            CheckForUpdates = settings.CheckForUpdates
        };
        NoteSettings = new()
        {
            DefaultWidth = settings.DefaultNoteWidth,
            DefaultHeight = settings.DefaultNoteHeight,
            StartupPosition = settings.StartupPosition,
            MinimizeMode = settings.MinimizeMode,
            VisibilityMode = settings.VisibilityMode,
            HideTitleBar = settings.HideTitleBar,
            CycleColors = settings.CycleColors,
            ColorMode = settings.ColorMode,
            TransparencyMode = settings.TransparencyMode,
            OpaqueWhenFocused = settings.OpaqueWhenFocused,
            OpaqueValue = settings.OpaqueOpacity,
            TransparentValue = settings.TransparentOpacity
        };
        EditorSettings = new()
        {
            CheckSpelling = settings.SpellCheck,
            NewLineAtEnd = settings.NewLineAtEnd,
            KeepNewLineVisible = settings.KeepNewLineVisible,
            WrapText = settings.WrapText,
            StandardFontFamily = settings.StandardFontFamily,
            MonoFontFamily = settings.MonoFontFamily,
            UseMonoFont = settings.UseMonoFont,
            AutoIndent = settings.AutoIndent,
            UseSpacesForTab = settings.TabUsesSpaces,
            ConvertIndentationOnPaste = settings.ConvertIndentationOnPaste,
            TabSpacesWidth = settings.TabWidth,
            CopyAction = settings.CopyAction,
            TrimTextOnCopy = settings.TrimTextOnCopy,
            CopyAltAction = settings.CopyAltAction,
            TrimTextOnAltCopy = settings.TrimTextOnAltCopy,
            CopyFallbackAction = settings.CopyFallbackAction,
            TrimTextOnFallbackCopy = settings.TrimTextOnFallbackCopy,
            CopyAltFallbackAction = settings.CopyAltFallbackAction,
            TrimTextOnAltFallbackCopy = settings.TrimTextOnAltFallbackCopy,
            CopyOnSelect = settings.CopyTextOnHighlight,
            PasteAction = settings.PasteAction,
            TrimTextOnPaste = settings.TrimTextOnPaste,
            PasteAltAction = settings.PasteAltAction,
            TrimTextOnAltPaste = settings.TrimTextOnAltPaste,
            MiddleClickPaste = settings.MiddleClickPaste
        };
        ToolSettings = new()
        {
            Base64ToolState = settings.Base64State,
            BracketToolState = settings.BracketState,
            CaseToolState = settings.CaseState,
            ColorToolState = settings.ColorState,
            DateTimeToolState = settings.DateTimeState,
            GibberishToolState = settings.GibberishState,
            HashToolState = settings.HashState,
            HtmlEntityToolState = settings.HTMLEntityState,
            IndentToolState = settings.IndentState,
            JoinToolState = settings.JoinState,
            JsonToolState = settings.JSONState,
            ListToolState = settings.ListState,
            QuoteToolState = settings.QuoteState,
            RemoveToolState = settings.RemoveState,
            SlashToolState = settings.SlashState,
            SortToolState = settings.SortState,
            SplitToolState = settings.SplitState,
            TrimToolState = settings.TrimState
        };
    }

    public void Save()
    {
        _ = _databaseService.SettingsRepository.Update(
            new SettingsDataModel(
                Id: 1,

                ShowTrayIcon: ApplicationSettings.ShowNotifiyIcon,
                CheckForUpdates: ApplicationSettings.CheckForUpdates,

                DefaultNoteWidth: NoteSettings.DefaultWidth,
                DefaultNoteHeight: NoteSettings.DefaultHeight,
                StartupPosition: NoteSettings.StartupPosition,
                MinimizeMode: NoteSettings.MinimizeMode,
                VisibilityMode: NoteSettings.VisibilityMode,
                HideTitleBar: NoteSettings.HideTitleBar,
                CycleColors: NoteSettings.CycleColors,
                ColorMode: NoteSettings.ColorMode,
                TransparencyMode: NoteSettings.TransparencyMode,
                OpaqueWhenFocused: NoteSettings.OpaqueWhenFocused,
                OpaqueOpacity: NoteSettings.OpaqueValue,
                TransparentOpacity: NoteSettings.TransparentValue,

                SpellCheck: EditorSettings.CheckSpelling,
                NewLineAtEnd: EditorSettings.NewLineAtEnd,
                KeepNewLineVisible: EditorSettings.KeepNewLineVisible,
                WrapText: EditorSettings.WrapText,
                StandardFontFamily: EditorSettings.StandardFontFamily,
                MonoFontFamily: EditorSettings.MonoFontFamily,
                UseMonoFont: EditorSettings.UseMonoFont,
                AutoIndent: EditorSettings.AutoIndent,
                TabUsesSpaces: EditorSettings.UseSpacesForTab,
                ConvertIndentationOnPaste: EditorSettings.ConvertIndentationOnPaste,
                TabWidth: EditorSettings.TabSpacesWidth,
                CopyAction: EditorSettings.CopyAction,
                TrimTextOnCopy: EditorSettings.TrimTextOnCopy,
                CopyAltAction: EditorSettings.CopyAltAction,
                TrimTextOnAltCopy: EditorSettings.TrimTextOnAltCopy,
                CopyFallbackAction: EditorSettings.CopyFallbackAction,
                TrimTextOnFallbackCopy: EditorSettings.TrimTextOnFallbackCopy,
                CopyAltFallbackAction: EditorSettings.CopyAltFallbackAction,
                TrimTextOnAltFallbackCopy: EditorSettings.TrimTextOnAltFallbackCopy,
                CopyTextOnHighlight: EditorSettings.CopyOnSelect,
                PasteAction: EditorSettings.PasteAction,
                TrimTextOnPaste: EditorSettings.TrimTextOnPaste,
                PasteAltAction: EditorSettings.PasteAltAction,
                TrimTextOnAltPaste: EditorSettings.TrimTextOnAltPaste,
                MiddleClickPaste: EditorSettings.MiddleClickPaste,

                Base64State: ToolSettings.Base64ToolState,
                BracketState: ToolSettings.BracketToolState,
                CaseState: ToolSettings.CaseToolState,
                ColorState: ToolSettings.ColorToolState,
                DateTimeState: ToolSettings.DateTimeToolState,
                GibberishState: ToolSettings.GibberishToolState,
                HashState: ToolSettings.HashToolState,
                HTMLEntityState: ToolSettings.HtmlEntityToolState,
                IndentState: ToolSettings.IndentToolState,
                JoinState: ToolSettings.JoinToolState,
                JSONState: ToolSettings.JsonToolState,
                ListState: ToolSettings.ListToolState,
                QuoteState: ToolSettings.QuoteToolState,
                RemoveState: ToolSettings.RemoveToolState,
                SlashState: ToolSettings.SlashToolState,
                SortState: ToolSettings.SortToolState,
                SplitState: ToolSettings.SplitToolState,
                TrimState: ToolSettings.TrimToolState
            )
        );
    }
}
