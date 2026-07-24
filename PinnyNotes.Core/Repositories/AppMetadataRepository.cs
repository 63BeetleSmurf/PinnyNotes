using Microsoft.Data.Sqlite;

using PinnyNotes.Core.Configurations;
using PinnyNotes.Core.DataTransferObjects;

namespace PinnyNotes.Core.Repositories;

public class AppMetadataRepository(DatabaseConfiguration databaseConfiguration) : BaseRepository(databaseConfiguration)
{
    public static readonly string TableName = "ApplicationData";

    public static readonly string TableSchema = @"
        (
            Id              INTEGER NOT NULL    PRIMARY KEY AUTOINCREMENT,

            LastUpdateCheck INTEGER NULL,
            ColourScheme    TEXT    NULL
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
            ColourScheme: GetStringNullable(reader, "ColourScheme")
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
                    ColourScheme    = @colourScheme
                WHERE Id = @id
            ",
            parameters: [
                new("@lastUpdateCheck", applicationData.LastUpdateCheck),
                new("@colourScheme", applicationData.ColourScheme),

                new("@id", applicationData.Id)
            ]
        );
    }
}
