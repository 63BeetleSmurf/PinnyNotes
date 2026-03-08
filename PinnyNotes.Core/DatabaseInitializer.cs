using Microsoft.Data.Sqlite;

using PinnyNotes.Core.Migrations;
using PinnyNotes.Core.Repositories;

namespace PinnyNotes.Core;

public class DatabaseInitialiser
{
    public const int SchemaVersion = 4;

    public static async Task Initialise(string connectionString)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();

        if (!SchemaTableExists(connection))
        {
            await CreateDatabase(connection);
            return;
        }

        int databaseSchemaVersion = GetSchemaVersion(connection);
        if (databaseSchemaVersion > SchemaVersion)
            throw new Exception("Database is from a newer version of Pinny Notes.");
        else if (databaseSchemaVersion < SchemaVersion)
            await UpdateDatabase(connection, databaseSchemaVersion);
    }

    private static bool SchemaTableExists(SqliteConnection connection)
    {
        string query = @"
            SELECT name
            FROM sqlite_master
            WHERE
                type = 'table'
                AND name = 'SchemaInfo';
        ";

        using SqliteCommand command = new(query, connection);
        if (command.ExecuteScalar() == null)
            return false;

        return true;
    }

    private static int GetSchemaVersion(SqliteConnection connection)
    {
        string query = @"
            SELECT Version
            FROM SchemaInfo
            WHERE Id = 0;
        ";

        using SqliteCommand command = new(query, connection);
        object result = command.ExecuteScalar()
            ?? throw new Exception("Invalid database schema version.");

        int schemaVersion = Convert.ToInt32(result);

        return schemaVersion;
    }

    private static async Task CreateDatabase(SqliteConnection connection)
    {
        await BaseRepository.ExecuteNonQuery(
            connection,
            $@"
                CREATE TABLE IF NOT EXISTS SchemaInfo (
                    Id      INTEGER PRIMARY KEY AUTOINCREMENT,
                    Version INTEGER NOT NULL
                );
                INSERT INTO SchemaInfo (Id, Version) VALUES
                    (0, @schemaVersion);

                CREATE TABLE IF NOT EXISTS {AppMetadataRepository.TableName}
                    {AppMetadataRepository.TableSchema};
                INSERT INTO {AppMetadataRepository.TableName} DEFAULT VALUES;

                CREATE TABLE IF NOT EXISTS {SettingsRepository.TableName}
                    {SettingsRepository.TableSchema};
                INSERT INTO {SettingsRepository.TableName} DEFAULT VALUES;

                CREATE TABLE IF NOT EXISTS {NoteRepository.TableName}
                    {NoteRepository.TableSchema};
            ",
            parameters: [
                new("@schemaVersion", SchemaVersion)
            ]
        );
    }

    private static async Task UpdateDatabase(SqliteConnection connection, int databaseSchemaVersion)
    {
        Schema1To2Migration schema1To2Migration = new();
        Schema2To3Migration schema2To3Migration = new();
        Schema3To4Migration schema3To4Migration = new();

        Dictionary<int, SchemaMigration> migrations = new()
        {
            {schema1To2Migration.TargetSchemaVersion, schema1To2Migration},
            {schema2To3Migration.TargetSchemaVersion, schema2To3Migration},
            {schema3To4Migration.TargetSchemaVersion, schema3To4Migration}
        };

        using SqliteTransaction transaction = connection.BeginTransaction();
        try
        {
            int currentSchemaVersion = databaseSchemaVersion;
            while (currentSchemaVersion < SchemaVersion)
            {
                SchemaMigration migration = migrations[currentSchemaVersion];
                await BaseRepository.ExecuteNonQuery(connection, migration.UpdateQuery);
                currentSchemaVersion = migration.ResultingSchemaVersion;
            }
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
