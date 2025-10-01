﻿using Microsoft.Data.Sqlite;

using PinnyNotes.DataAccess.Migrations;
using PinnyNotes.DataAccess.Repositories;

namespace PinnyNotes.DataAccess;

public class DatabaseInitialiser
{
    public const int SchemaVersion = 2;

    public static void Initialise(string connectionString)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();

        if (!SchemaTableExists(connection))
        {
            CreateDatabase(connection);
            return;
        }

        int databaseSchemaVersion = GetSchemaVersion(connection);
        if (databaseSchemaVersion > SchemaVersion)
            throw new Exception("Database is from a newer version of Pinny Notes.");
        else if (databaseSchemaVersion < SchemaVersion)
            UpdateDatabase(connection, databaseSchemaVersion);
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

    private static void CreateDatabase(SqliteConnection connection)
    {
        BaseRepository.ExecuteNonQuery(
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
            ",
            parameters: [
                new("@schemaVersion", SchemaVersion)
            ]
        );
    }

    private static void UpdateDatabase(SqliteConnection connection, int databaseSchemaVersion)
    {
        Schema1To2Migration schema1To2Migration = new();

        Dictionary<int, SchemaMigration> migrations = new()
        {
            {schema1To2Migration.TargetSchemaVersion, schema1To2Migration}
        };

        using SqliteTransaction transaction = connection.BeginTransaction();
        try
        {
            int currentSchemaVersion = databaseSchemaVersion;
            while (currentSchemaVersion < SchemaVersion)
            {
                SchemaMigration migration = migrations[currentSchemaVersion];
                BaseRepository.ExecuteNonQuery(connection, migration.UpdateQuery);
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
