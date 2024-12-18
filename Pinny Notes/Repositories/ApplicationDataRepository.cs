using Microsoft.Data.Sqlite;
using System;

using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Repositories;

public class ApplicationDataRepository : BaseRepository
{
    private readonly string _connectionString;

    public ApplicationDataRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ApplicationDataModel GetApplicationData()
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
            SELECT
                *
            FROM
                ApplicationData
            WHERE
                Id = 0;
        ";

        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.Read())
            throw new Exception("Error getting application data.");

        return new ApplicationDataModel() {
            Id = GetInt(reader, "Id"),

            LastUpdateCheck = GetLongNullable(reader, "LastUpdateCheck"),
            LastThemeColorKey = GetStringNullable(reader, "LastThemeColorKey")
        };
    }

    public void UpdateApplicationData(ApplicationDataModel applicationData)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE
                ApplicationData
            SET
                LastUpdateCheck     = @lastUpdateCheck,
                LastThemeColorKey   = @lastThemeColorKey
            WHERE
                Id = @id
        ";
        command.Parameters.AddWithValue("@id", applicationData.Id);

        command.Parameters.AddWithValue("@lastUpdateCheck", ToDbValue(applicationData.LastUpdateCheck));
        command.Parameters.AddWithValue("@lastThemeColorKey", ToDbValue(applicationData.LastThemeColorKey));

        command.ExecuteNonQuery();
    }
}
