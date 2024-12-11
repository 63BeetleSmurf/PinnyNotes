using Microsoft.Data.Sqlite;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Models;
using System;
using System.Collections.Generic;

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

                Applicaiton_TrayIcon                INTEGER,
                Applicaiton_NotesInTaskbar          INTEGER,
                Applicaiton_CheckForUpdates         INTEGER,

                Notes_StartupPosition               INTEGER,
                Notes_MinimizeMode                  INTEGER,
                Notes_HideTitleBar                  INTEGER,
                Notes_DefaultColor                  INTEGER,
                Notes_ColorMode                     INTEGER,
                Notes_TransparencyMode              INTEGER,
                Notes_OpaqueWhenFocused             INTEGER,

                Editor_MonoFont                     INTEGER,
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

    public SettingsModel? GetSettingsById(int id)
    {
        using var connection = new SqliteConnection(_applicationManager.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT
                Id,

                Applicaiton_TrayIcon,
                Applicaiton_NotesInTaskbar,
                Applicaiton_CheckForUpdates,

                Notes_StartupPosition,
                Notes_MinimizeMode,
                Notes_HideTitleBar,
                Notes_DefaultColor,
                Notes_ColorMode,
                Notes_TransparencyMode,
                Notes_OpaqueWhenFocused,

                Editor_MonoFont,
                Editor_SpellCheck,
                Editor_AutoIndent,
                Editor_NewLineAtEnd,
                Editor_KeepNewLineVisible,
                Editor_TabsToSpaces,
                Editor_ConvertIndentationOnPaste,
                Editor_TabWidth,
                Editor_MiddleClickPaste,
                Editor_TrimPastedText,
                Editor_TrimCopiedText,
                Editor_CopyHighlightedText
            FROM
                Settings
            WHERE
                Id = @id
        ";
        command.Parameters.AddWithValue("@id", id);

        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.Read())
            return null;

        return new SettingsModel() {
            Id = GetInt(reader, "Id"),

            Applicaiton_TrayIcon = GetBoolNullable(reader, "Applicaiton_TrayIcon"),
            Applicaiton_NotesInTaskbar = GetBoolNullable(reader, "Applicaiton_NotesInTaskbar"),
            Applicaiton_CheckForUpdates = GetBoolNullable(reader, "Applicaiton_CheckForUpdates"),

            Notes_StartupPosition = GetEnumNullable<StartupPositions>(reader, "Notes_StartupPosition"),
            Notes_MinimizeMode = GetEnumNullable<MinimizeModes>(reader, "Notes_MinimizeMode"),
            Notes_HideTitleBar = GetBoolNullable(reader, "Notes_HideTitleBar"),
            Notes_DefaultColor = GetStringNullable(reader, "Notes_DefaultColor"),
            Notes_ColorMode = GetEnumNullable<ColorModes>(reader, "Notes_ColorMode"),
            Notes_TransparencyMode = GetEnumNullable<TransparencyModes>(reader, "Notes_TransparencyMode"),
            Notes_OpaqueWhenFocused = GetBoolNullable(reader, "Notes_OpaqueWhenFocused"),

            Editor_MonoFont = GetBoolNullable(reader, "Editor_MonoFont"),
            Editor_SpellCheck = GetBoolNullable(reader, "Editor_SpellCheck"),
            Editor_AutoIndent = GetBoolNullable(reader, "Editor_AutoIndent"),
            Editor_NewLineAtEnd = GetBoolNullable(reader, "Editor_NewLineAtEnd"),
            Editor_KeepNewLineVisible = GetBoolNullable(reader, "Editor_KeepNewLineVisible"),
            Editor_TabsToSpaces = GetBoolNullable(reader, "Editor_TabsToSpaces"),
            Editor_ConvertIndentationOnPaste = GetBoolNullable(reader, "Editor_ConvertIndentationOnPaste"),
            Editor_TabWidth = GetIntNullable(reader, "Editor_TabWidth"),
            Editor_MiddleClickPaste = GetBoolNullable(reader, "Editor_MiddleClickPaste"),
            Editor_TrimPastedText = GetBoolNullable(reader, "Editor_TrimPastedText"),
            Editor_TrimCopiedText = GetBoolNullable(reader, "Editor_TrimCopiedText"),
            Editor_CopyHighlightedText = GetBoolNullable(reader, "Editor_CopyHighlightedText")
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
