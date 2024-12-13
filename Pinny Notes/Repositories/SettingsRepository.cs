using Microsoft.Data.Sqlite;
using System;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Repositories;

public class SettingsRepository : BaseRepository
{
    private readonly ApplicationManager _applicationManager;

    public SettingsRepository(ApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
    }

    public SettingsModel GetApplicationSettings()
    {
        using var connection = new SqliteConnection(_applicationManager.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT
                *
            FROM
                Settings
            WHERE
                Id = 0;
        ";

        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.Read())
            throw new Exception("Error getting application settings.");

        return new SettingsModel() {
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
            Editor_CopyHighlightedText = GetBool(reader, "Editor_CopyHighlightedText")
        };
    }

    public void UpdateSettings(SettingsModel settings)
    {
        using var connection = new SqliteConnection(_applicationManager.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
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
                Editor_CopyHighlightedText          = @editor_CopyHighlightedText
            WHERE
                Id = @id
        ";
        command.Parameters.AddWithValue("@application_TrayIcon", settings.Application_TrayIcon);
        command.Parameters.AddWithValue("@application_NotesInTaskbar", settings.Application_NotesInTaskbar);
        command.Parameters.AddWithValue("@application_CheckForUpdates", settings.Application_CheckForUpdates);

        command.Parameters.AddWithValue("@notes_DefaultWidth", settings.Notes_DefaultWidth);
        command.Parameters.AddWithValue("@notes_DefaultHeight", settings.Notes_DefaultHeight);
        command.Parameters.AddWithValue("@notes_StartupPosition", settings.Notes_StartupPosition);
        command.Parameters.AddWithValue("@notes_MinimizeMode", settings.Notes_MinimizeMode);
        command.Parameters.AddWithValue("@notes_HideTitleBar", settings.Notes_HideTitleBar);
        command.Parameters.AddWithValue("@notes_DefaultThemeColorKey", settings.Notes_DefaultThemeColorKey);
        command.Parameters.AddWithValue("@notes_ColorMode", settings.Notes_ColorMode);
        command.Parameters.AddWithValue("@notes_TransparencyMode", settings.Notes_TransparencyMode);
        command.Parameters.AddWithValue("@notes_OpaqueWhenFocused", settings.Notes_OpaqueWhenFocused);
        command.Parameters.AddWithValue("@notes_TransparentOpacity", settings.Notes_TransparentOpacity);
        command.Parameters.AddWithValue("@notes_OpaqueOpacity", settings.Notes_OpaqueOpacity);

        command.Parameters.AddWithValue("@editor_UseMonoFont", settings.Editor_UseMonoFont);
        command.Parameters.AddWithValue("@editor_MonoFontFamily", settings.Editor_MonoFontFamily);
        command.Parameters.AddWithValue("@editor_SpellCheck", settings.Editor_SpellCheck);
        command.Parameters.AddWithValue("@editor_AutoIndent", settings.Editor_AutoIndent);
        command.Parameters.AddWithValue("@editor_NewLineAtEnd", settings.Editor_NewLineAtEnd);
        command.Parameters.AddWithValue("@editor_KeepNewLineVisible", settings.Editor_KeepNewLineVisible);
        command.Parameters.AddWithValue("@editor_TabsToSpaces", settings.Editor_TabsToSpaces);
        command.Parameters.AddWithValue("@editor_ConvertIndentationOnPaste", settings.Editor_ConvertIndentationOnPaste);
        command.Parameters.AddWithValue("@editor_TabWidth", settings.Editor_TabWidth);
        command.Parameters.AddWithValue("@editor_MiddleClickPaste", settings.Editor_MiddleClickPaste);
        command.Parameters.AddWithValue("@editor_TrimPastedText", settings.Editor_TrimPastedText);
        command.Parameters.AddWithValue("@editor_TrimCopiedText", settings.Editor_TrimCopiedText);
        command.Parameters.AddWithValue("@editor_CopyHighlightedText", settings.Editor_CopyHighlightedText);

        command.ExecuteNonQuery();
    }
}
