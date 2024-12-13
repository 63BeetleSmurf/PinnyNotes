using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace PinnyNotes.WpfUi.Helpers;

public static class DatabaseHelper
{
    public const int SchemaVersion = 0;

    public static string GetConnectionString()
    {
        string usersAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string dataPath = Path.Combine(usersAppDataPath, "Pinny Notes");
        if (!Path.Exists(dataPath))
            Directory.CreateDirectory(dataPath);

        return $"Data Source={Path.Combine(dataPath, "pinny_notes.sqlite")}";
    }

    public static void CheckDatabase(string connectionString)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();

        if (!SchemaTableExists(connection))
            InitializeDatabase(connection);

        // Will add schema version checking and update scripts when needed.
    }

    private static bool SchemaTableExists(SqliteConnection connection)
    {
        string query = @"
            SELECT
                name 
            FROM
                sqlite_master 
            WHERE
                type='table'
                AND name='SchemaInfo';
        ";

        using SqliteCommand command = new(query, connection);
        if (command.ExecuteScalar() == null)
            return false;

        return true;
    }

    private static void InitializeDatabase(SqliteConnection connection)
    {
        string query = @"
            CREATE TABLE IF NOT EXISTS SchemaInfo (
                Id      INTEGER PRIMARY KEY AUTOINCREMENT,
                Version INTEGER
            );
            INSERT INTO SchemaInfo (Id, Version) VALUES
                (0, @schemaVersion);

            CREATE TABLE IF NOT EXISTS ApplicationData (
                Id                  INTEGER PRIMARY KEY AUTOINCREMENT,

                LastUpdateCheck     INTEGER,
                LastThemeColorKey   STRING
            );
            INSERT INTO ApplicationData (Id, LastUpdateCheck, LastThemeColorKey) VALUES
                (0, NULL, NULL);

            CREATE TABLE IF NOT EXISTS Settings (
                Id                                  INTEGER PRIMARY KEY AUTOINCREMENT,

                Application_TrayIcon                INTEGER,
                Application_NotesInTaskbar          INTEGER,
                Application_CheckForUpdates         INTEGER,

                Notes_DefaultWidth                  INTEGER,
                Notes_DefaultHeight                 INTEGER,
                Notes_StartupPosition               INTEGER,
                Notes_MinimizeMode                  INTEGER,
                Notes_HideTitleBar                  INTEGER,
                Notes_DefaultThemeColorKey          TEXT,
                Notes_ColorMode                     INTEGER,
                Notes_TransparencyMode              INTEGER,
                Notes_OpaqueWhenFocused             INTEGER,
                Notes_TransparentOpacity            REAL,
                Notes_OpaqueOpacity                 REAL,

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
            INSERT INTO Settings (
                Id,
                Application_TrayIcon, Application_NotesInTaskbar, Application_CheckForUpdates,
                Notes_DefaultWidth, Notes_DefaultHeight, Notes_StartupPosition, Notes_MinimizeMode, Notes_HideTitleBar, Notes_DefaultThemeColorKey, Notes_ColorMode, Notes_TransparencyMode, Notes_OpaqueWhenFocused, Notes_TransparentOpacity, Notes_OpaqueOpacity,
                Editor_UseMonoFont, Editor_MonoFontFamily, Editor_SpellCheck, Editor_AutoIndent, Editor_NewLineAtEnd, Editor_KeepNewLineVisible, Editor_TabsToSpaces, Editor_ConvertIndentationOnPaste, Editor_TabWidth, Editor_MiddleClickPaste, Editor_TrimPastedText, Editor_TrimCopiedText, Editor_CopyHighlightedText
            ) VALUES
                (
                    0,
                    1, 1, 0,
                    300, 300, 0, 0, 0, @cycleThemeKey, 0, 2, 1, 0.8, 1.0,
                    0, ""Consolas"", 1, 1, 1, 1, 0, 0, 4, 1, 1, 1, 0
                );

            CREATE TABLE IF NOT EXISTS Groups (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,

                SettingsId  INTEGER,

                Name        TEXT    NOT NULL,
                Color       INTEGER,

                FOREIGN KEY(""SettingsId"") REFERENCES ""Settings""(""Id"")
            );
            INSERT INTO Groups (Id, Name) VALUES
                (0, ""Ungrouped"");

            CREATE TABLE IF NOT EXISTS Notes (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,

                SettingsId  INTEGER,
                GroupId     INTEGER NOT NULL,

                Title       TEXT    NOT NULL,
                Content     TEXT    NOT NULL,

                X           INTEGER,
                Y           INTEGER,
                Width       INTEGER,
                Height      INTEGER,

                GravityX    INTEGER,
                GracityY    INTEGER,

                Color       INTEGER,

                IsPinned    INTEGER,

                FOREIGN KEY(""SettingsId"") REFERENCES ""Settings""(""Id""),
                FOREIGN KEY(""GroupId"")    REFERENCES ""Groups""(""Id"")
            );
            INSERT INTO Notes (Id, GroupId, Title, Content) VALUES
                (1, 0, ""New Note"", ""Welcome to Pinny Notes!"");
        ";

        using SqliteCommand command = new(query, connection);
        command.Parameters.AddWithValue("@cycleThemeKey", ThemeHelper.CycleThemeKey);
        command.Parameters.AddWithValue("@schemaVersion", SchemaVersion);

        command.ExecuteNonQuery();
    }
}
