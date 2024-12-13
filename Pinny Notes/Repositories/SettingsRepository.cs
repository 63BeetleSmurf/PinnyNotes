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
                Id = 1;
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
            Notes_DefaultColor = GetString(reader, "Notes_DefaultColor"),
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

    public void SaveSetting(string key, string value)
    {
        using var connection = new SqliteConnection(_applicationManager.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
                INSERT INTO Settings (Key, Value) VALUES (@key, @value)
                ON CONFLICT(Key) DO UPDATE SET Value = excluded.Value;
            ";
        command.Parameters.AddWithValue("@key", key);
        command.Parameters.AddWithValue("@value", value);

        command.ExecuteNonQuery();
    }
}
