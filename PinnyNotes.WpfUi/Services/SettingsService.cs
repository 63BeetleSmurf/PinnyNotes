using System.Threading.Tasks;

using PinnyNotes.DataAccess.Models;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Services;

public class SettingsService(DatabaseService databaseService)
{
    private readonly DatabaseService _databaseService = databaseService;

    public ApplicationSettingsModel ApplicationSettings => _applicationSettings;
    private ApplicationSettingsModel _applicationSettings = null!;
    public NoteSettingsModel NoteSettings => _noteSettings;
    private NoteSettingsModel _noteSettings = null!;
    public EditorSettingsModel EditorSettings => _editorSettings;
    private EditorSettingsModel _editorSettings = null!;
    public ToolSettingsModel ToolSettings => _toolSettings;
    private ToolSettingsModel _toolSettings = null!;

    public async Task Load()
    {
        SettingsDataModel settings = await _databaseService.GetSettings(1);

        _applicationSettings = new()
        {
            ShowNotifiyIcon = settings.ShowTrayIcon,
            CheckForUpdates = settings.CheckForUpdates
        };

        _noteSettings = new()
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

        _editorSettings = new()
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

        _toolSettings = new()
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
            TrimToolState = settings.TrimState,
            UrlToolState = settings.UrlState
        };
    }

    public async Task Save()
    {
        _ = await _databaseService.UpdateSettings(
            new SettingsDataModel(
                Id: 1,

                ShowTrayIcon: _applicationSettings.ShowNotifiyIcon,
                CheckForUpdates: _applicationSettings.CheckForUpdates,

                DefaultNoteWidth: _noteSettings.DefaultWidth,
                DefaultNoteHeight: _noteSettings.DefaultHeight,
                StartupPosition: _noteSettings.StartupPosition,
                MinimizeMode: _noteSettings.MinimizeMode,
                VisibilityMode: _noteSettings.VisibilityMode,
                HideTitleBar: _noteSettings.HideTitleBar,
                CycleColors: _noteSettings.CycleColors,
                ColorMode: _noteSettings.ColorMode,
                TransparencyMode: _noteSettings.TransparencyMode,
                OpaqueWhenFocused: _noteSettings.OpaqueWhenFocused,
                OpaqueOpacity: _noteSettings.OpaqueValue,
                TransparentOpacity: _noteSettings.TransparentValue,

                SpellCheck: _editorSettings.CheckSpelling,
                NewLineAtEnd: _editorSettings.NewLineAtEnd,
                KeepNewLineVisible: _editorSettings.KeepNewLineVisible,
                WrapText: _editorSettings.WrapText,
                StandardFontFamily: _editorSettings.StandardFontFamily,
                MonoFontFamily: _editorSettings.MonoFontFamily,
                UseMonoFont: _editorSettings.UseMonoFont,
                AutoIndent: _editorSettings.AutoIndent,
                TabUsesSpaces: _editorSettings.UseSpacesForTab,
                ConvertIndentationOnPaste: _editorSettings.ConvertIndentationOnPaste,
                TabWidth: _editorSettings.TabSpacesWidth,
                CopyAction: _editorSettings.CopyAction,
                TrimTextOnCopy: _editorSettings.TrimTextOnCopy,
                CopyAltAction: _editorSettings.CopyAltAction,
                TrimTextOnAltCopy: _editorSettings.TrimTextOnAltCopy,
                CopyFallbackAction: _editorSettings.CopyFallbackAction,
                TrimTextOnFallbackCopy: _editorSettings.TrimTextOnFallbackCopy,
                CopyAltFallbackAction: _editorSettings.CopyAltFallbackAction,
                TrimTextOnAltFallbackCopy: _editorSettings.TrimTextOnAltFallbackCopy,
                CopyTextOnHighlight: _editorSettings.CopyOnSelect,
                PasteAction: _editorSettings.PasteAction,
                TrimTextOnPaste: _editorSettings.TrimTextOnPaste,
                PasteAltAction: _editorSettings.PasteAltAction,
                TrimTextOnAltPaste: _editorSettings.TrimTextOnAltPaste,
                MiddleClickPaste: _editorSettings.MiddleClickPaste,

                Base64State: _toolSettings.Base64ToolState,
                BracketState: _toolSettings.BracketToolState,
                CaseState: _toolSettings.CaseToolState,
                ColorState: _toolSettings.ColorToolState,
                DateTimeState: _toolSettings.DateTimeToolState,
                GibberishState: _toolSettings.GibberishToolState,
                HashState: _toolSettings.HashToolState,
                HTMLEntityState: _toolSettings.HtmlEntityToolState,
                IndentState: _toolSettings.IndentToolState,
                JoinState: _toolSettings.JoinToolState,
                JSONState: _toolSettings.JsonToolState,
                ListState: _toolSettings.ListToolState,
                QuoteState: _toolSettings.QuoteToolState,
                RemoveState: _toolSettings.RemoveToolState,
                SlashState: _toolSettings.SlashToolState,
                SortState: _toolSettings.SortToolState,
                SplitState: _toolSettings.SplitToolState,
                TrimState: _toolSettings.TrimToolState,
                UrlState: _toolSettings.UrlToolState
            )
        );
    }
}
