using Microsoft.Data.Sqlite;

using PinnyNotes.Core.Configurations;
using PinnyNotes.Core.DataTransferObjects;

namespace PinnyNotes.Core.Repositories;

public class AppMetadataRepository(DatabaseConfiguration databaseConfiguration) : BaseRepository(databaseConfiguration)
{
    public static readonly string TableName = "ApplicationData";

    public static readonly string TableSchema = @"
        (
            Id              INTEGER PRIMARY KEY AUTOINCREMENT,

            LastUpdateCheck INTEGER DEFAULT NULL,
            ColorScheme     TEXT    DEFAULT NULL
        );
    ";

    public async Task<AppMetadataDataDto> GetById(int id)
    {
        using SqliteConnection connection = new(ConnectionString);
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

        return new AppMetadataDataDto(
            Id: GetInt(reader, "Id"),

            LastUpdateCheck: GetLongNullable(reader, "LastUpdateCheck"),
            ColorScheme: GetStringNullable(reader, "ColorScheme")
        );
    }

    public async Task<int> Update(AppMetadataDataDto applicationData)
    {
        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        return await ExecuteNonQuery(
            connection,
            @"
                UPDATE ApplicationData
                SET
                    LastUpdateCheck = @lastUpdateCheck,
                    ColorScheme     = @colorScheme
                WHERE Id = @id
            ",
            parameters: [
                new("@lastUpdateCheck", applicationData.LastUpdateCheck),
                new("@colorScheme", applicationData.ColorScheme),

                new("@id", applicationData.Id)
            ]
        );
    }
}
