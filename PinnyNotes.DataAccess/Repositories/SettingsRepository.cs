using Microsoft.Data.Sqlite;

using PinnyNotes.Core.Enums;
using PinnyNotes.DataAccess.Models;

namespace PinnyNotes.DataAccess.Repositories;

public class SettingsRepository(string connectionString) : BaseRepository(connectionString)
{
    public static readonly string TableName = "Settings";

    public static readonly string TableSchema = $@"
        (
            Id  INTEGER PRIMARY KEY AUTOINCREMENT,

            Application_ShowTrayIcon            INTEGER DEFAULT 1,
            Application_CheckForUpdates         INTEGER DEFAULT 0,

            Notes_ShowInTaskbar                 INTEGER DEFAULT 1,
            Notes_DefaultWidth                  INTEGER DEFAULT 300,
            Notes_DefaultHeight                 INTEGER DEFAULT 300,
            Notes_StartupPosition               INTEGER DEFAULT 0,
            Notes_MinimizeMode                  INTEGER DEFAULT 0,
            Notes_HideTitleBar                  INTEGER DEFAULT 0,
            Notes_CycleColors                   INTEGER DEFAULT 1,
            Notes_ColorMode                     INTEGER DEFAULT 0,
            Notes_TransparencyMode              INTEGER DEFAULT 2,
            Notes_OpaqueWhenFocused             INTEGER DEFAULT 1,
            Notes_OpaqueOpacity                 REAL    DEFAULT 1.0,
            Notes_TransparentOpacity            REAL    DEFAULT 0.8,

            Editor_SpellCheck                   INTEGER DEFAULT 1,
            Editor_AutoIndent                   INTEGER DEFAULT 1,
            Editor_NewLineAtEnd                 INTEGER DEFAULT 1,
            Editor_KeepNewLineVisible           INTEGER DEFAULT 1,
            Editor_WrapText                     INTEGER DEFAULT 1,
            Editor_StandardFontFamily           TEXT    DEFAULT 'Segoe UI',
            Editor_MonoFontFamily               TEXT    DEFAULT 'Consolas',
            Editor_UseMonoFont                  INTEGER DEFAULT 0,
            Editor_TabUsesSpaces                INTEGER DEFAULT 0,
            Editor_ConvertIndentationOnPaste    INTEGER DEFAULT 0,
            Editor_TabWidth                     INTEGER DEFAULT 4,
            Editor_CopyAction                   INTEGER DEFAULT 1,
            Editor_TrimTextOnCopy               INTEGER DEFAULT 0,
            Editor_CopyAltAction                INTEGER DEFAULT 1,
            Editor_TrimTextOnAltCopy            INTEGER DEFAULT 1,
            Editor_CopyFallbackAction           INTEGER DEFAULT 1,
            Editor_TrimTextOnFallbackCopy       INTEGER DEFAULT 0,
            Editor_CopyAltFallbackAction        INTEGER DEFAULT 2,
            Editor_TrimTextOnAltFallbackCopy    INTEGER DEFAULT 0,
            Editor_CopyTextOnHighlight          INTEGER DEFAULT 0,
            Editor_PasteAction                  INTEGER DEFAULT 1,
            Editor_TrimTextOnPaste              INTEGER DEFAULT 0,
            Editor_PasteAltAction               INTEGER DEFAULT 1,
            Editor_TrimTextOnAltPaste           INTEGER DEFAULT 1,
            Editor_MiddleClickPaste             INTEGER DEFAULT 1,

            Tool_Base64State                    INTEGER DEFAULT 1,
            Tool_BracketState                   INTEGER DEFAULT 1,
            Tool_CaseState                      INTEGER DEFAULT 1,
            Tool_ColorState                     INTEGER DEFAULT 1,
            Tool_DateTimeState                  INTEGER DEFAULT 1,
            Tool_GibberishState                 INTEGER DEFAULT 1,
            Tool_HashState                      INTEGER DEFAULT 1,
            Tool_HTMLEntityState                INTEGER DEFAULT 1,
            Tool_IndentState                    INTEGER DEFAULT 1,
            Tool_JoinState                      INTEGER DEFAULT 1,
            Tool_JSONState                      INTEGER DEFAULT 1,
            Tool_ListState                      INTEGER DEFAULT 1,
            Tool_QuoteState                     INTEGER DEFAULT 1,
            Tool_RemoveState                    INTEGER DEFAULT 1,
            Tool_SlashState                     INTEGER DEFAULT 1,
            Tool_SortState                      INTEGER DEFAULT 1,
            Tool_SplitState                     INTEGER DEFAULT 1,
            Tool_TrimState                      INTEGER DEFAULT 1
        )
    ";

    public SettingsDataModel GetById(int id)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        using SqliteDataReader reader = ExecuteReader(
            connection,
            @"
                SELECT *
                FROM Settings
                WHERE Id = @id;
            ",
            parameters: [
                new("@id", id)
            ]
        );
        if (!reader.Read())
            throw new Exception($"Could not find settings with id: {id}.");

        return new SettingsDataModel(
            Id: GetInt(reader, "Id"),

            ShowTrayIcon: GetBool(reader, "Application_ShowTrayIcon"),
            CheckForUpdates: GetBool(reader, "Application_CheckForUpdates"),

            ShowNotesInTaskbar: GetBool(reader, "Notes_ShowInTaskbar"),
            DefaultNoteWidth: GetInt(reader, "Notes_DefaultWidth"),
            DefaultNoteHeight: GetInt(reader, "Notes_DefaultHeight"),
            StartupPosition: GetEnum<StartupPositions>(reader, "Notes_StartupPosition"),
            MinimizeMode: GetEnum<MinimizeModes>(reader, "Notes_MinimizeMode"),
            HideTitleBar: GetBool(reader, "Notes_HideTitleBar"),
            CycleColors: GetBool(reader, "Notes_CycleColors"),
            ColorMode: GetEnum<ColorModes>(reader, "Notes_ColorMode"),
            TransparencyMode: GetEnum<TransparencyModes>(reader, "Notes_TransparencyMode"),
            OpaqueWhenFocused: GetBool(reader, "Notes_OpaqueWhenFocused"),
            OpaqueOpacity: GetDouble(reader, "Notes_OpaqueOpacity"),
            TransparentOpacity: GetDouble(reader, "Notes_TransparentOpacity"),

            SpellCheck: GetBool(reader, "Editor_SpellCheck"),
            AutoIndent: GetBool(reader, "Editor_AutoIndent"),
            NewLineAtEnd: GetBool(reader, "Editor_NewLineAtEnd"),
            KeepNewLineVisible: GetBool(reader, "Editor_KeepNewLineVisible"),
            WrapText: GetBool(reader, "Editor_WrapText"),
            StandardFontFamily: GetString(reader, "Editor_StandardFontFamily"),
            MonoFontFamily: GetString(reader, "Editor_MonoFontFamily"),
            UseMonoFont: GetBool(reader, "Editor_UseMonoFont"),
            TabUsesSpaces: GetBool(reader, "Editor_TabUsesSpaces"),
            ConvertIndentationOnPaste: GetBool(reader, "Editor_ConvertIndentationOnPaste"),
            TabWidth: GetInt(reader, "Editor_TabWidth"),
            CopyAction: GetEnum<CopyActions>(reader, "Editor_CopyAction"),
            TrimTextOnCopy: GetBool(reader, "Editor_TrimTextOnCopy"),
            CopyAltAction: GetEnum<CopyActions>(reader, "Editor_CopyAltAction"),
            TrimTextOnAltCopy: GetBool(reader, "Editor_TrimTextOnAltCopy"),
            CopyFallbackAction: GetEnum<CopyFallbackActions>(reader, "Editor_CopyFallbackAction"),
            TrimTextOnFallbackCopy: GetBool(reader, "Editor_TrimTextOnFallbackCopy"),
            CopyAltFallbackAction: GetEnum<CopyFallbackActions>(reader, "Editor_CopyAltFallbackAction"),
            TrimTextOnAltFallbackCopy: GetBool(reader, "Editor_TrimTextOnAltFallbackCopy"),
            CopyTextOnHighlight: GetBool(reader, "Editor_CopyTextOnHighlight"),
            PasteAction: GetEnum<PasteActions>(reader, "Editor_PasteAction"),
            TrimTextOnPaste: GetBool(reader, "Editor_TrimTextOnPaste"),
            PasteAltAction: GetEnum<PasteActions>(reader, "Editor_PasteAltAction"),
            TrimTextOnAltPaste: GetBool(reader, "Editor_TrimTextOnAltPaste"),
            MiddleClickPaste: GetBool(reader, "Editor_MiddleClickPaste"),

            Base64State: GetEnum<ToolStates>(reader, "Tool_Base64State"),
            BracketState: GetEnum<ToolStates>(reader, "Tool_BracketState"),
            CaseState: GetEnum<ToolStates>(reader, "Tool_CaseState"),
            ColorState: GetEnum<ToolStates>(reader, "Tool_ColorState"),
            DateTimeState: GetEnum<ToolStates>(reader, "Tool_DateTimeState"),
            GibberishState: GetEnum<ToolStates>(reader, "Tool_GibberishState"),
            HashState: GetEnum<ToolStates>(reader, "Tool_HashState"),
            HTMLEntityState: GetEnum<ToolStates>(reader, "Tool_HTMLEntityState"),
            IndentState: GetEnum<ToolStates>(reader, "Tool_IndentState"),
            JoinState: GetEnum<ToolStates>(reader, "Tool_JoinState"),
            JSONState: GetEnum<ToolStates>(reader, "Tool_JSONState"),
            ListState: GetEnum<ToolStates>(reader, "Tool_ListState"),
            QuoteState: GetEnum<ToolStates>(reader, "Tool_QuoteState"),
            RemoveState: GetEnum<ToolStates>(reader, "Tool_RemoveState"),
            SlashState: GetEnum<ToolStates>(reader, "Tool_SlashState"),
            SortState: GetEnum<ToolStates>(reader, "Tool_SortState"),
            SplitState: GetEnum<ToolStates>(reader, "Tool_SplitState"),
            TrimState: GetEnum<ToolStates>(reader, "Tool_TrimState")
        );
    }

    public int Update(SettingsDataModel settings)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        return ExecuteNonQuery(
            connection,
            @"
                UPDATE
                    Settings
                SET
                    Application_ShowTrayIcon = @application_ShowTrayIcon,
                    Application_CheckForUpdates = @application_CheckForUpdates,

                    Notes_ShowInTaskbar = @notes_ShowInTaskbar,
                    Notes_DefaultWidth = @notes_DefaultWidth,
                    Notes_DefaultHeight = @notes_DefaultHeight,
                    Notes_StartupPosition = @notes_StartupPosition,
                    Notes_MinimizeMode = @notes_MinimizeMode,
                    Notes_HideTitleBar = @notes_HideTitleBar,
                    Notes_CycleColors = @notes_CycleColors,
                    Notes_ColorMode = @notes_ColorMode,
                    Notes_TransparencyMode = @notes_TransparencyMode,
                    Notes_OpaqueWhenFocused = @notes_OpaqueWhenFocused,
                    Notes_OpaqueOpacity = @notes_OpaqueOpacity,
                    Notes_TransparentOpacity = @notes_TransparentOpacity,

                    Editor_SpellCheck = @editor_SpellCheck,
                    Editor_AutoIndent = @editor_AutoIndent,
                    Editor_NewLineAtEnd = @editor_NewLineAtEnd,
                    Editor_KeepNewLineVisible = @editor_KeepNewLineVisible,
                    Editor_WrapText = @editor_WrapText,
                    Editor_StandardFontFamily = @editor_StandardFontFamily,
                    Editor_MonoFontFamily = @editor_MonoFontFamily,
                    Editor_UseMonoFont = @editor_UseMonoFont,
                    Editor_TabUsesSpaces = @editor_TabUsesSpaces,
                    Editor_ConvertIndentationOnPaste = @editor_ConvertIndentationOnPaste,
                    Editor_TabWidth = @editor_TabWidth,
                    Editor_CopyAction = @editor_CopyAction,
                    Editor_TrimTextOnCopy = @editor_TrimTextOnCopy,
                    Editor_CopyAltAction = @editor_CopyAltAction,
                    Editor_TrimTextOnAltCopy = @editor_TrimTextOnAltCopy,
                    Editor_CopyFallbackAction = @editor_CopyFallbackAction,
                    Editor_TrimTextOnFallbackCopy = @editor_TrimTextOnFallbackCopy,
                    Editor_CopyAltFallbackAction = @editor_CopyAltFallbackAction,
                    Editor_TrimTextOnAltFallbackCopy = @editor_TrimTextOnAltFallbackCopy,
                    Editor_CopyTextOnHighlight = @editor_CopyTextOnHighlight,
                    Editor_PasteAction = @editor_PasteAction,
                    Editor_TrimTextOnPaste = @editor_TrimTextOnPaste,
                    Editor_PasteAltAction = @editor_PasteAltAction,
                    Editor_TrimTextOnAltPaste = @editor_TrimTextOnAltPaste,
                    Editor_MiddleClickPaste = @editor_MiddleClickPaste,

                    Tool_Base64State = @tool_Base64State,
                    Tool_BracketState = @tool_BracketState,
                    Tool_CaseState = @tool_CaseState,
                    Tool_ColorState = @tool_ColorState,
                    Tool_DateTimeState = @tool_DateTimeState,
                    Tool_GibberishState = @tool_GibberishState,
                    Tool_HashState = @tool_HashState,
                    Tool_HTMLEntityState = @tool_HTMLEntityState,
                    Tool_IndentState = @tool_IndentState,
                    Tool_JoinState = @tool_JoinState,
                    Tool_JSONState = @tool_JSONState,
                    Tool_ListState = @tool_ListState,
                    Tool_QuoteState = @tool_QuoteState,
                    Tool_RemoveState = @tool_RemoveState,
                    Tool_SlashState = @tool_SlashState,
                    Tool_SortState = @tool_SortState,
                    Tool_SplitState = @tool_SplitState,
                    Tool_TrimState = @tool_TrimState
                WHERE
                    Id = @id;
            ",
            parameters: [
                new("@application_ShowTrayIcon", settings.ShowTrayIcon),
                new("@application_CheckForUpdates", settings.CheckForUpdates),

                new("@notes_ShowInTaskbar", settings.ShowNotesInTaskbar),
                new("@notes_DefaultWidth", settings.DefaultNoteWidth),
                new("@notes_DefaultHeight", settings.DefaultNoteHeight),
                new("@notes_StartupPosition", settings.StartupPosition),
                new("@notes_MinimizeMode", settings.MinimizeMode),
                new("@notes_HideTitleBar", settings.HideTitleBar),
                new("@notes_CycleColors", settings.CycleColors),
                new("@notes_ColorMode", settings.ColorMode),
                new("@notes_TransparencyMode", settings.TransparencyMode),
                new("@notes_OpaqueWhenFocused", settings.OpaqueWhenFocused),
                new("@notes_OpaqueOpacity", settings.OpaqueOpacity),
                new("@notes_TransparentOpacity", settings.TransparentOpacity),

                new("@editor_SpellCheck", settings.SpellCheck),
                new("@editor_AutoIndent", settings.AutoIndent),
                new("@editor_NewLineAtEnd", settings.NewLineAtEnd),
                new("@editor_KeepNewLineVisible", settings.KeepNewLineVisible),
                new("@editor_WrapText", settings.WrapText),
                new("@editor_StandardFontFamily", settings.StandardFontFamily),
                new("@editor_MonoFontFamily", settings.MonoFontFamily),
                new("@editor_UseMonoFont", settings.UseMonoFont),
                new("@editor_TabUsesSpaces", settings.TabUsesSpaces),
                new("@editor_ConvertIndentationOnPaste", settings.ConvertIndentationOnPaste),
                new("@editor_TabWidth", settings.TabWidth),
                new("@editor_CopyAction", settings.CopyAction),
                new("@editor_TrimTextOnCopy", settings.TrimTextOnCopy),
                new("@editor_CopyAltAction", settings.CopyAltAction),
                new("@editor_TrimTextOnAltCopy", settings.TrimTextOnAltCopy),
                new("@editor_CopyFallbackAction", settings.CopyFallbackAction),
                new("@editor_TrimTextOnFallbackCopy", settings.TrimTextOnFallbackCopy),
                new("@editor_CopyAltFallbackAction", settings.CopyAltFallbackAction),
                new("@editor_TrimTextOnAltFallbackCopy", settings.TrimTextOnAltFallbackCopy),
                new("@editor_CopyTextOnHighlight", settings.CopyTextOnHighlight),
                new("@editor_PasteAction", settings.PasteAction),
                new("@editor_TrimTextOnPaste", settings.TrimTextOnPaste),
                new("@editor_PasteAltAction", settings.PasteAltAction),
                new("@editor_TrimTextOnAltPaste", settings.TrimTextOnAltPaste),
                new("@editor_MiddleClickPaste", settings.MiddleClickPaste),

                new("@tool_Base64State", settings.Base64State),
                new("@tool_BracketState", settings.BracketState),
                new("@tool_CaseState", settings.CaseState),
                new("@tool_ColorState", settings.ColorState),
                new("@tool_DateTimeState", settings.DateTimeState),
                new("@tool_GibberishState", settings.GibberishState),
                new("@tool_HashState", settings.HashState),
                new("@tool_HTMLEntityState", settings.HTMLEntityState),
                new("@tool_IndentState", settings.IndentState),
                new("@tool_JoinState", settings.JoinState),
                new("@tool_JSONState", settings.JSONState),
                new("@tool_ListState", settings.ListState),
                new("@tool_QuoteState", settings.QuoteState),
                new("@tool_RemoveState", settings.RemoveState),
                new("@tool_SlashState", settings.SlashState),
                new("@tool_SortState", settings.SortState),
                new("@tool_SplitState", settings.SplitState),
                new("@tool_TrimState", settings.TrimState),

                new("@id", settings.Id)
            ]
        );
    }
}
