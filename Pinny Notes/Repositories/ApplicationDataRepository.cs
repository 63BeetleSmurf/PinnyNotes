using Microsoft.Data.Sqlite;
using System;

using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Repositories;

public class ApplicationDataRepository(string connectionString) : BaseRepository(connectionString)
{
    public static readonly string TableName = "ApplicationData";

    public static readonly string TableSchema = @"
        (
            Id                  INTEGER PRIMARY KEY AUTOINCREMENT,

            LastUpdateCheck     INTEGER DEFAULT NULL,
            LastThemeColorKey   STRING  DEFAULT NULL
        );
    ";

    public ApplicationDataModel GetApplicationData()
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        using SqliteDataReader reader = ExecuteReader(
            connection,
            @"
                SELECT
                    *
                FROM
                    ApplicationData
                WHERE
                    Id = 1;
            "
        );
        if (!reader.Read())
            throw new Exception("Error getting application data.");

        return new()
        {
            Id = GetInt(reader, "Id"),

            LastUpdateCheck = GetLongNullable(reader, "LastUpdateCheck"),
            LastThemeColorKey = GetStringNullable(reader, "LastThemeColorKey")
        };
    }

    public void UpdateApplicationData(ApplicationDataModel applicationData)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        ExecuteNonQuery(
            connection,
            @"
                UPDATE
                    ApplicationData
                SET
                    LastUpdateCheck     = @lastUpdateCheck,
                    LastThemeColorKey   = @lastThemeColorKey
                WHERE
                    Id = @id
            ",
            parameters: [
                new("@lastUpdateCheck", applicationData.LastUpdateCheck),
                new("@lastThemeColorKey", applicationData.LastThemeColorKey),

                new("@id", applicationData.Id)
            ]
        );
    }
}
