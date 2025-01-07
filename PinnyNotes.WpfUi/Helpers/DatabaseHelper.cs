using Microsoft.Data.Sqlite;
using System;
using System.IO;

using PinnyNotes.WpfUi.Repositories;

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
            BaseRepository.ExecuteNonQuery(
                connection,
                $@"
                    CREATE TABLE IF NOT EXISTS SchemaInfo (
                        Id      INTEGER PRIMARY KEY AUTOINCREMENT,
                        Version INTEGER NOT NULL
                    );
                    INSERT INTO SchemaInfo (Id, Version) VALUES
                        (0, @schemaVersion);

                    CREATE TABLE IF NOT EXISTS {ApplicationDataRepository.TableName}
                        {ApplicationDataRepository.TableSchema};
                    INSERT INTO {ApplicationDataRepository.TableName} DEFAULT VALUES;

                    CREATE TABLE IF NOT EXISTS {SettingsRepository.TableName}
                        {SettingsRepository.TableSchema};
                    INSERT INTO {SettingsRepository.TableName} DEFAULT VALUES;

                    CREATE TABLE IF NOT EXISTS {GroupRepository.TableName}
                        {GroupRepository.TableSchema};

                    CREATE TABLE IF NOT EXISTS {NoteRepository.TableName}
                        {NoteRepository.TableSchema};
                ",
                parameters: [
                    new("@schemaVersion", SchemaVersion)
                ]
            );

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
}
