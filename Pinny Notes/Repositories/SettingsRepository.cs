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
                Id                              INTEGER PRIMARY KEY AUTOINCREMENT,

                ApplicaitonTrayIcon             INTEGER,
                ApplicaitonNotesInTaskbar       INTEGER,
                ApplicaitonCheckForUpdates      INTEGER,

                NotesStartupPosition            INTEGER,
                NotesMinimizeMode               INTEGER,
                NotesHideTitleBar               INTEGER,
                NotesDefaultColor               INTEGER,
                NotesColorMode                  INTEGER,
                NotesTransparencyMode           INTEGER,
                NotesOpaqueWhenFocused          INTEGER,

                EditorMonoFont                  INTEGER,
                EditorSpellCheck                INTEGER,
                EditorAutoIndent                INTEGER,
                EditorNewLineAtEnd              INTEGER,
                EditorKeepNewLineVisible        INTEGER,
                EditorTabsToSpaces              INTEGER,
                EditorConvertIndentationOnPaste INTEGER,
                EditorTabWidth                  INTEGER,
                EditorMiddleClickPaste          INTEGER,
                EditorTrimPastedText            INTEGER,
                EditorTrimCopiedText            INTEGER,
                EditorCopyHighlightedText       INTEGER
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

                ApplicaitonTrayIcon,
                ApplicaitonNotesInTaskbar,
                ApplicaitonCheckForUpdates,

                NotesStartupPosition,
                NotesMinimizeMode,
                NotesHideTitleBar,
                NotesDefaultColor,
                NotesColorMode,
                NotesTransparencyMode,
                NotesOpaqueWhenFocused,

                EditorMonoFont,
                EditorSpellCheck,
                EditorAutoIndent,
                EditorNewLineAtEnd,
                EditorKeepNewLineVisible,
                EditorTabsToSpaces,
                EditorConvertIndentationOnPaste,
                EditorTabWidth,
                EditorMiddleClickPaste,
                EditorTrimPastedText,
                EditorTrimCopiedText,
                EditorCopyHighlightedText
            FROM
                Settings
            WHERE Id = @id
        ";
        command.Parameters.AddWithValue("@id", id);

        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.Read())
            return null;

        return new SettingsModel() {
            Id = GetInt(reader, "Id"),

            ShowTrayIcon = GetBoolNullable(reader, "ApplicaitonTrayIcon"),
            ShowNotesInTaskbar = GetBoolNullable(reader, "ApplicaitonNotesInTaskbar"),
            CheckForUpdates = GetBoolNullable(reader, "ApplicaitonCheckForUpdates"),

            StartupPosition = GetEnumNullable<StartupPositions>(reader, "NotesStartupPosition"),
            MinimizeMode = GetEnumNullable<MinimizeModes>(reader, "NotesMinimizeMode"),
            HideTitleBar = GetBoolNullable(reader, "NotesHideTitleBar"),
            DefaultColor = GetStringNullable(reader, "NotesDefaultColor"),
            ColorMode = GetEnumNullable<ColorModes>(reader, "NotesColorMode"),
            TransparencyMode = GetEnumNullable<TransparencyModes>(reader, "NotesTransparencyMode"),
            OpaqueWhenFocused = GetBoolNullable(reader, "NotesOpaqueWhenFocused"),

            UseMonoFont = GetBoolNullable(reader, "EditorMonoFont"),
            SpellChecker = GetBoolNullable(reader, "EditorSpellCheck"),
            AutoIndent = GetBoolNullable(reader, "EditorAutoIndent"),
            NewLineAtEnd = GetBoolNullable(reader, "EditorNewLineAtEnd"),
            KeepNewLineAtEndVisible = GetBoolNullable(reader, "EditorKeepNewLineVisible"),
            TabSpaces = GetBoolNullable(reader, "EditorTabsToSpaces"),
            ConvertIndentation = GetBoolNullable(reader, "EditorConvertIndentationOnPaste"),
            TabWidth = GetIntNullable(reader, "EditorTabWidth"),
            MiddleClickPaste = GetBoolNullable(reader, "EditorMiddleClickPaste"),
            TrimPastedText = GetBoolNullable(reader, "EditorTrimPastedText"),
            TrimCopiedText = GetBoolNullable(reader, "EditorTrimCopiedText"),
            AutoCopy = GetBoolNullable(reader, "EditorCopyHighlightedText")
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
