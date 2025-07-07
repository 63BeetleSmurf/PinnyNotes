using Microsoft.Data.Sqlite;

using PinnyNotes.DataAccess.Repositories;

namespace PinnyNotes.DataAccess;

public class DatabaseInitialiser
{
    public const int SchemaVersion = 1;

    public static void Initialise(string connectionString)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();

        if (SchemaTableExists(connection))
            return;

        BaseRepository.ExecuteNonQuery(
            connection,
            $@"
                CREATE TABLE IF NOT EXISTS SchemaInfo (
                    Id      INTEGER PRIMARY KEY AUTOINCREMENT,
                    Version INTEGER NOT NULL
                );
                INSERT INTO SchemaInfo (Id, Version) VALUES
                    (0, @schemaVersion);

                CREATE TABLE IF NOT EXISTS {SettingsRepository.TableName}
                    {SettingsRepository.TableSchema};
                INSERT INTO {SettingsRepository.TableName} DEFAULT VALUES;
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
            SELECT name
            FROM sqlite_master
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
