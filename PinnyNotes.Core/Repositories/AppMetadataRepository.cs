using Microsoft.Data.Sqlite;

using PinnyNotes.Core.Enums;
using PinnyNotes.Core.Models;

namespace PinnyNotes.Core.Repositories;

public class AppMetadataRepository(string connectionString) : BaseRepository(connectionString)
{
    public static readonly string TableName = "ApplicationData";

    public static readonly string TableSchema = @"
        (
            Id              INTEGER PRIMARY KEY AUTOINCREMENT,

            LastUpdateCheck INTEGER DEFAULT NULL,
            ThemeColor      INTEGER DEFAULT 0
        );
    ";

    public async Task<AppMetadataDataModel> GetById(int id)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        using SqliteDataReader reader = await ExecuteReader(
            connection,
            @"
                SELECT *
                FROM ApplicationData
                WHERE Id = @id;
            ",
            parameters: [
                new("@id", id)
            ]
        );
        if (!reader.Read())
            throw new Exception($"Could not find application data with id: {id}.");

        return new AppMetadataDataModel(
            Id: GetInt(reader, "Id"),

            LastUpdateCheck: GetLongNullable(reader, "LastUpdateCheck"),
            ThemeColor: GetEnumNullable<ThemeColors>(reader, "ThemeColor")
        );
    }

    public async Task<int> Update(AppMetadataDataModel applicationData)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        return await ExecuteNonQuery(
            connection,
            @"
                UPDATE ApplicationData
                SET
                    LastUpdateCheck = @lastUpdateCheck,
                    ThemeColor      = @themeColor
                WHERE Id = @id
            ",
            parameters: [
                new("@lastUpdateCheck", applicationData.LastUpdateCheck),
                new("@themeColor", applicationData.ThemeColor),

                new("@id", applicationData.Id)
            ]
        );
    }
}
