using Microsoft.Data.Sqlite;
using System;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Repositories;

public class SettingsRepository(string connectionString) : BaseRepository(connectionString)
{
    public static readonly string TableName = "Settings";

    public static readonly string TableSchema = $@"
        (
            Id  INTEGER PRIMARY KEY AUTOINCREMENT,

            Application_TrayIcon                INTEGER DEFAULT 1,
            Application_NotesInTaskbar          INTEGER DEFAULT 1,
            Application_CheckForUpdates         INTEGER DEFAULT 0,

            Notes_DefaultWidth                  INTEGER DEFAULT 300,
            Notes_DefaultHeight                 INTEGER DEFAULT 300,
            Notes_StartupPosition               INTEGER DEFAULT 0,
            Notes_MinimizeMode                  INTEGER DEFAULT 0,
            Notes_HideTitleBar                  INTEGER DEFAULT 0,
            Notes_DefaultThemeColorKey          TEXT    DEFAULT '{ThemeHelper.CycleThemeKey}',
            Notes_ColorMode                     INTEGER DEFAULT 0,
            Notes_TransparencyMode              INTEGER DEFAULT 2,
            Notes_OpaqueWhenFocused             INTEGER DEFAULT 1,
            Notes_TransparentOpacity            REAL    DEFAULT 0.8,
            Notes_OpaqueOpacity                 REAL    DEFAULT 1.0,

            Editor_UseMonoFont                  INTEGER DEFAULT 0,
            Editor_MonoFontFamily               TEXT    DEFAULT 'Consolas',
            Editor_SpellCheck                   INTEGER DEFAULT 1,
            Editor_AutoIndent                   INTEGER DEFAULT 1,
            Editor_NewLineAtEnd                 INTEGER DEFAULT 1,
            Editor_KeepNewLineVisible           INTEGER DEFAULT 1,
            Editor_TabsToSpaces                 INTEGER DEFAULT 0,
            Editor_ConvertIndentationOnPaste    INTEGER DEFAULT 0,
            Editor_TabWidth                     INTEGER DEFAULT 4,
            Editor_MiddleClickPaste             INTEGER DEFAULT 1,
            Editor_TrimPastedText               INTEGER DEFAULT 1,
            Editor_TrimCopiedText               INTEGER DEFAULT 1,
            Editor_CopyHighlightedText          INTEGER DEFAULT 0,

            Tool_Base64State                    INTEGER DEFAULT 0,
            Tool_BracketState                   INTEGER DEFAULT 0,
            Tool_CaseState                      INTEGER DEFAULT 0,
            Tool_DateTimeState                  INTEGER DEFAULT 0,
            Tool_GibberishState                 INTEGER DEFAULT 0,
            Tool_HashState                      INTEGER DEFAULT 0,
            Tool_HtmlEntityState                INTEGER DEFAULT 0,
            Tool_IndentState                    INTEGER DEFAULT 0,
            Tool_JoinState                      INTEGER DEFAULT 0,
            Tool_JsonState                      INTEGER DEFAULT 0,
            Tool_ListState                      INTEGER DEFAULT 0,
            Tool_QuoteState                     INTEGER DEFAULT 0,
            Tool_RemoveState                    INTEGER DEFAULT 0,
            Tool_SlashState                     INTEGER DEFAULT 0,
            Tool_SortState                      INTEGER DEFAULT 0,
            Tool_SplitState                     INTEGER DEFAULT 0,
            Tool_TrimState                      INTEGER DEFAULT 0
        )
    ";

    public SettingsModel GetById(int id)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        using SqliteDataReader reader = ExecuteReader(
            connection,
            @"
                SELECT
                    *
                FROM
                    Settings
                WHERE
                    Id = @id;
            ",
            parameters: [
                new("@id", id)
            ]
        );
        if (!reader.Read())
            throw new Exception($"Could not find settings with id: {id}.");

        return new()
        {
            Id = GetInt(reader, "Id"),

            Application_TrayIcon = GetBool(reader, "Application_TrayIcon"),
            Application_NotesInTaskbar = GetBool(reader, "Application_NotesInTaskbar"),
            Application_CheckForUpdates = GetBool(reader, "Application_CheckForUpdates"),

            Notes_DefaultWidth = GetInt(reader, "Notes_DefaultWidth"),
            Notes_DefaultHeight = GetInt(reader, "Notes_DefaultHeight"),
            Notes_StartupPosition = GetEnum<StartupPositions>(reader, "Notes_StartupPosition"),
            Notes_MinimizeMode = GetEnum<MinimizeModes>(reader, "Notes_MinimizeMode"),
            Notes_HideTitleBar = GetBool(reader, "Notes_HideTitleBar"),
            Notes_DefaultThemeColorKey = GetString(reader, "Notes_DefaultThemeColorKey"),
            Notes_ColorMode = GetEnum<ColorModes>(reader, "Notes_ColorMode"),
            Notes_TransparencyMode = GetEnum<TransparencyModes>(reader, "Notes_TransparencyMode"),
            Notes_OpaqueWhenFocused = GetBool(reader, "Notes_OpaqueWhenFocused"),
            Notes_TransparentOpacity = GetDouble(reader, "Notes_TransparentOpacity"),
            Notes_OpaqueOpacity = GetDouble(reader, "Notes_OpaqueOpacity"),

            Editor_UseMonoFont = GetBool(reader, "Editor_UseMonoFont"),
            Editor_MonoFontFamily = GetString(reader, "Editor_MonoFontFamily"),
            Editor_SpellCheck = GetBool(reader, "Editor_SpellCheck"),
            Editor_AutoIndent = GetBool(reader, "Editor_AutoIndent"),
            Editor_NewLineAtEnd = GetBool(reader, "Editor_NewLineAtEnd"),
            Editor_KeepNewLineVisible = GetBool(reader, "Editor_KeepNewLineVisible"),
            Editor_TabsToSpaces = GetBool(reader, "Editor_TabsToSpaces"),
            Editor_ConvertIndentationOnPaste = GetBool(reader, "Editor_ConvertIndentationOnPaste"),
            Editor_TabWidth = GetInt(reader, "Editor_TabWidth"),
            Editor_MiddleClickPaste = GetBool(reader, "Editor_MiddleClickPaste"),
            Editor_TrimPastedText = GetBool(reader, "Editor_TrimPastedText"),
            Editor_TrimCopiedText = GetBool(reader, "Editor_TrimCopiedText"),
            Editor_CopyHighlightedText = GetBool(reader, "Editor_CopyHighlightedText"),

            Tool_Base64State = GetEnum<ToolStates>(reader, "Tool_Base64State"),
            Tool_BracketState = GetEnum<ToolStates>(reader, "Tool_BracketState"),
            Tool_CaseState = GetEnum<ToolStates>(reader, "Tool_CaseState"),
            Tool_DateTimeState = GetEnum<ToolStates>(reader, "Tool_DateTimeState"),
            Tool_GibberishState = GetEnum<ToolStates>(reader, "Tool_GibberishState"),
            Tool_HashState = GetEnum<ToolStates>(reader, "Tool_HashState"),
            Tool_HtmlEntityState = GetEnum<ToolStates>(reader, "Tool_HtmlEntityState"),
            Tool_IndentState = GetEnum<ToolStates>(reader, "Tool_IndentState"),
            Tool_JoinState = GetEnum<ToolStates>(reader, "Tool_JoinState"),
            Tool_JsonState = GetEnum<ToolStates>(reader, "Tool_JsonState"),
            Tool_ListState = GetEnum<ToolStates>(reader, "Tool_JsonState"),
            Tool_QuoteState = GetEnum<ToolStates>(reader, "Tool_QuoteState"),
            Tool_RemoveState = GetEnum<ToolStates>(reader, "Tool_RemoveState"),
            Tool_SlashState = GetEnum<ToolStates>(reader, "Tool_SlashState"),
            Tool_SortState = GetEnum<ToolStates>(reader, "Tool_SortState"),
            Tool_SplitState = GetEnum<ToolStates>(reader, "Tool_SplitState"),
            Tool_TrimState = GetEnum<ToolStates>(reader, "Tool_TrimState"),
        };
    }

    public void Update(SettingsModel settings)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        ExecuteNonQuery(
            connection,
            @"
                UPDATE
                    Settings
                SET
                    Application_TrayIcon        = @application_TrayIcon,
                    Application_NotesInTaskbar  = @application_NotesInTaskbar,
                    Application_CheckForUpdates = @application_CheckForUpdates,

                    Notes_DefaultWidth          = @notes_DefaultWidth,
                    Notes_DefaultHeight         = @notes_DefaultHeight,
                    Notes_StartupPosition       = @notes_StartupPosition,
                    Notes_MinimizeMode          = @notes_MinimizeMode,
                    Notes_HideTitleBar          = @notes_HideTitleBar,
                    Notes_DefaultThemeColorKey  = @notes_DefaultThemeColorKey,
                    Notes_ColorMode             = @notes_ColorMode,
                    Notes_TransparencyMode      = @notes_TransparencyMode,
                    Notes_OpaqueWhenFocused     = @notes_OpaqueWhenFocused,
                    Notes_TransparentOpacity    = @notes_TransparentOpacity,
                    Notes_OpaqueOpacity         = @notes_OpaqueOpacity,

                    Editor_UseMonoFont                  = @editor_UseMonoFont,
                    Editor_MonoFontFamily               = @editor_MonoFontFamily,
                    Editor_SpellCheck                   = @editor_SpellCheck,
                    Editor_AutoIndent                   = @editor_AutoIndent,
                    Editor_NewLineAtEnd                 = @editor_NewLineAtEnd,
                    Editor_KeepNewLineVisible           = @editor_KeepNewLineVisible,
                    Editor_TabsToSpaces                 = @editor_TabsToSpaces,
                    Editor_ConvertIndentationOnPaste    = @editor_ConvertIndentationOnPaste,
                    Editor_TabWidth                     = @editor_TabWidth,
                    Editor_MiddleClickPaste             = @editor_MiddleClickPaste,
                    Editor_TrimPastedText               = @editor_TrimPastedText,
                    Editor_TrimCopiedText               = @editor_TrimCopiedText,
                    Editor_CopyHighlightedText          = @editor_CopyHighlightedText,

                    Tool_Base64State        = @tool_Base64State,
                    Tool_BracketState       = @tool_BracketState,
                    Tool_CaseState          = @tool_CaseState,
                    Tool_DateTimeState      = @tool_DateTimeState,
                    Tool_GibberishState     = @tool_GibberishState,
                    Tool_HashState          = @tool_HashState,
                    Tool_HtmlEntityState    = @tool_HtmlEntityState,
                    Tool_IndentState        = @tool_IndentState,
                    Tool_JoinState          = @tool_JoinState,
                    Tool_JsonState          = @tool_JsonState,
                    Tool_ListState          = @tool_ListState,
                    Tool_QuoteState         = @tool_QuoteState,
                    Tool_RemoveState        = @tool_RemoveState,
                    Tool_SlashState         = @tool_SlashState,
                    Tool_SortState          = @tool_SortState,
                    Tool_SplitState         = @tool_SplitState,
                    Tool_TrimState          = @tool_TrimState
                WHERE
                    Id = @id
            ",
            parameters: [
                new("@application_TrayIcon", settings.Application_TrayIcon),
                new("@application_NotesInTaskbar", settings.Application_NotesInTaskbar),
                new("@application_CheckForUpdates", settings.Application_CheckForUpdates),

                new("@notes_DefaultWidth", settings.Notes_DefaultWidth),
                new("@notes_DefaultHeight", settings.Notes_DefaultHeight),
                new("@notes_StartupPosition", settings.Notes_StartupPosition),
                new("@notes_MinimizeMode", settings.Notes_MinimizeMode),
                new("@notes_HideTitleBar", settings.Notes_HideTitleBar),
                new("@notes_DefaultThemeColorKey", settings.Notes_DefaultThemeColorKey),
                new("@notes_ColorMode", settings.Notes_ColorMode),
                new("@notes_TransparencyMode", settings.Notes_TransparencyMode),
                new("@notes_OpaqueWhenFocused", settings.Notes_OpaqueWhenFocused),
                new("@notes_TransparentOpacity", settings.Notes_TransparentOpacity),
                new("@notes_OpaqueOpacity", settings.Notes_OpaqueOpacity),

                new("@editor_UseMonoFont", settings.Editor_UseMonoFont),
                new("@editor_MonoFontFamily", settings.Editor_MonoFontFamily),
                new("@editor_SpellCheck", settings.Editor_SpellCheck),
                new("@editor_AutoIndent", settings.Editor_AutoIndent),
                new("@editor_NewLineAtEnd", settings.Editor_NewLineAtEnd),
                new("@editor_KeepNewLineVisible", settings.Editor_KeepNewLineVisible),
                new("@editor_TabsToSpaces", settings.Editor_TabsToSpaces),
                new("@editor_ConvertIndentationOnPaste", settings.Editor_ConvertIndentationOnPaste),
                new("@editor_TabWidth", settings.Editor_TabWidth),
                new("@editor_MiddleClickPaste", settings.Editor_MiddleClickPaste),
                new("@editor_TrimPastedText", settings.Editor_TrimPastedText),
                new("@editor_TrimCopiedText", settings.Editor_TrimCopiedText),
                new("@editor_CopyHighlightedText", settings.Editor_CopyHighlightedText),

                new("@tool_Base64State", settings.Tool_Base64State),
                new("@tool_BracketState", settings.Tool_BracketState),
                new("@tool_CaseState", settings.Tool_CaseState),
                new("@tool_DateTimeState", settings.Tool_DateTimeState),
                new("@tool_GibberishState", settings.Tool_GibberishState),
                new("@tool_HashState", settings.Tool_HashState),
                new("@tool_HtmlEntityState", settings.Tool_HtmlEntityState),
                new("@tool_IndentState", settings.Tool_IndentState),
                new("@tool_JoinState", settings.Tool_JoinState),
                new("@tool_JsonState", settings.Tool_JsonState),
                new("@tool_ListState", settings.Tool_ListState),
                new("@tool_QuoteState", settings.Tool_QuoteState),
                new("@tool_RemoveState", settings.Tool_RemoveState),
                new("@tool_SlashState", settings.Tool_SlashState),
                new("@tool_SortState", settings.Tool_SortState),
                new("@tool_SplitState", settings.Tool_SplitState),
                new("@tool_TrimState", settings.Tool_TrimState),

                new("@id", settings.Id)
            ]
        );
    }
}
