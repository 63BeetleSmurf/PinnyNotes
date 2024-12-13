using Microsoft.Data.Sqlite;
using System;

using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Repositories;

public class ApplicationDataRepository : BaseRepository
{
    private readonly ApplicationManager _applicationManager;

    public ApplicationDataRepository(ApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
    }

    public ApplicationDataModel GetApplicationData()
    {
        using var connection = new SqliteConnection(_applicationManager.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
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
        using var connection = new SqliteConnection(_applicationManager.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE
                ApplicationData
            SET
                LastUpdateCheck     = @lastUpdateCheck,
                LastThemeColorKey   = @lastThemeColorKey
            WHERE
                Id = @id
        ";
        command.Parameters.AddWithValue("@lastUpdateCheck", applicationData.LastUpdateCheck);
        command.Parameters.AddWithValue("@lastThemeColorKey", applicationData.LastThemeColorKey);

        command.ExecuteNonQuery();
    }
}
