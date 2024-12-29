using Microsoft.Data.Sqlite;
using System;

using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Repositories;

public class GroupRepository(string connectionString) : BaseRepository(connectionString)
{
    public static readonly string TableName = "Groups";

    public static readonly string TableSchema = @"
        (
            Id          INTEGER PRIMARY KEY AUTOINCREMENT,

            SettingsId  INTEGER DEFAULT NULL,

            Name        TEXT    NOT NULL DEFAULT '',
            Color       INTEGER DEFAULT NULL,

            FOREIGN KEY(""SettingsId"") REFERENCES ""Settings""(""Id"")
        )
    ";

    public GroupModel GetById(int id)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        using SqliteDataReader reader = ExecuteReader(
            connection,
            @"
                SELECT
                    *
                FROM
                    Groups
                WHERE
                    Id = @id;
            ",
            parameters: [
                new("@id", id)
            ]
        );
        if (!reader.Read())
            throw new Exception($"Could not find group with id: {id}.");

        return GetGroupModelFromReader(reader);
    }

    private static GroupModel GetGroupModelFromReader(SqliteDataReader reader)
    {
        return new()
        {
            Id = GetInt(reader, "Id"),

            SettingsId = GetIntNullable(reader, "SettingsId")
        };
    }
}
