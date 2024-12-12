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

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using SqliteConnection connection = new(_applicationManager.ConnectionString);
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Settings (
                Id                                  INTEGER PRIMARY KEY AUTOINCREMENT,

                Application_TrayIcon                INTEGER,
                Application_NotesInTaskbar          INTEGER,
                Application_CheckForUpdates         INTEGER,

                Notes_StartupPosition               INTEGER,
                Notes_MinimizeMode                  INTEGER,
                Notes_HideTitleBar                  INTEGER,
                Notes_DefaultColor                  INTEGER,
                Notes_ColorMode                     INTEGER,
                Notes_TransparencyMode              INTEGER,
                Notes_OpaqueWhenFocused             INTEGER,

                Editor_UseMonoFont                  INTEGER,
                Editor_MonoFontFamily               TEXT,
                Editor_SpellCheck                   INTEGER,
                Editor_AutoIndent                   INTEGER,
                Editor_NewLineAtEnd                 INTEGER,
                Editor_KeepNewLineVisible           INTEGER,
                Editor_TabsToSpaces                 INTEGER,
                Editor_ConvertIndentationOnPaste    INTEGER,
                Editor_TabWidth                     INTEGER,
                Editor_MiddleClickPaste             INTEGER,
                Editor_TrimPastedText               INTEGER,
                Editor_TrimCopiedText               INTEGER,
                Editor_CopyHighlightedText          INTEGER
            );

            CREATE TABLE IF NOT EXISTS Groups (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,

                SettingsId  INTEGER NOT NULL,

                Name        TEXT    NOT NULL,
                Color       INTEGER,

                FOREIGN KEY(""SettingsId"") REFERENCES ""Settings""(""Id""),
            );

            CREATE TABLE IF NOT EXISTS Notes (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,

                SettingsId  INTEGER NOT NULL,
                GroupId     INTEGER NOT NULL,

                Title       TEXT    NOT NULL,
                Content     TEXT    NOT NULL,

                X           INTEGER NOT NULL,
                Y           INTEGER NOT NULL,
                Width       INTEGER NOT NULL,
                Height      INTEGER NOT NULL,

                GravityX    INTEGER NOT NULL,
                GracityY    INTEGER NOT NULL,

                Color       INTEGER NOT NULL,

                IsPinned    INTEGER NOT NULL,

                FOREIGN KEY(""SettingsId"") REFERENCES ""Settings""(""Id""),
                FOREIGN KEY(""GroupId"")    REFERENCES ""Groups""(""Id""),
            );
        ";
        command.ExecuteNonQuery();
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

            Notes_StartupPosition = GetEnum<StartupPositions>(reader, "Notes_StartupPosition"),
            Notes_MinimizeMode = GetEnum<MinimizeModes>(reader, "Notes_MinimizeMode"),
            Notes_HideTitleBar = GetBool(reader, "Notes_HideTitleBar"),
            Notes_DefaultColor = GetString(reader, "Notes_DefaultColor"),
            Notes_ColorMode = GetEnum<ColorModes>(reader, "Notes_ColorMode"),
            Notes_TransparencyMode = GetEnum<TransparencyModes>(reader, "Notes_TransparencyMode"),
            Notes_OpaqueWhenFocused = GetBool(reader, "Notes_OpaqueWhenFocused"),

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
