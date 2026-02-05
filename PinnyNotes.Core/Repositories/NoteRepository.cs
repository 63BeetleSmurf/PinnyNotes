using Microsoft.Data.Sqlite;

using PinnyNotes.Core.Configurations;
using PinnyNotes.Core.DataTransferObjects;

namespace PinnyNotes.Core.Repositories;

public class NoteRepository(DatabaseConfiguration databaseConfiguration) : BaseRepository(databaseConfiguration)
{
    public static readonly string TableName = "Notes";

    public static readonly string TableSchema = @"
        (
            Id                  INTEGER PRIMARY KEY AUTOINCREMENT,

            Content             TEXT    DEFAULT '',

            X                   REAL    DEFAULT 0,
            Y                   REAL    DEFAULT 0,
            Width               REAL    DEFAULT 300,
            Height              REAL    DEFAULT 300,

            GravityX            INTEGER DEFAULT 1,
            GravityY            INTEGER DEFAULT 1,

            ThemeColorScheme    TEXT    DEFAULT NULL,

            IsPinned            INTEGER DEFAULT 0,
            IsOpen              INTEGER DEFAULT 0
        )
    ";

    public async Task<int> Create()
    {
        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        await ExecuteNonQuery(
            connection,
            @"
                INSERT INTO Notes
                DEFAULT VALUES;
            "
        );

        int newId = await GetLastInsertRowId(connection);

        return newId;
    }

    public async Task<NoteDto> GetById(int id)
    {
        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        using SqliteDataReader reader = await ExecuteReader(
            connection,
            @"
                SELECT *
                FROM Notes
                WHERE Id = @id;
            ",
            parameters: [
                new("@id", id)
            ]
        );
        if (!reader.Read())
            throw new Exception($"Could not find note with id: {id}.");

        NoteDto dto = GetNoteDtoFromReader(reader);

        return dto;
    }

    public async Task<IEnumerable<NoteDto>> GetAll()
    {
        List<NoteDto> notes = [];

        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        using SqliteDataReader reader = await ExecuteReader(
            connection,
            @"
                SELECT *
                FROM Notes;
            "
        );

        while (reader.Read())
            notes.Add(GetNoteDtoFromReader(reader));

        return notes;
    }

    public async Task Update(NoteDto note)
    {
        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        int affectedRows = await ExecuteNonQuery(
            connection,
            @"
                UPDATE Notes
                SET
                    Content = @content,

                    X = @x,
                    Y = @y,
                    Width = @width,
                    Height = @height,

                    GravityX = @gravityX,
                    GravityY = @gravityY,

                    ThemeColorScheme = @themeColorScheme,

                    IsPinned = @isPinned,
                    IsOpen = @isOpen
                WHERE Id = @id;
            ",
            parameters: [
                new("@content", note.Content),

                new("@x", note.X),
                new("@y", note.Y),
                new("@width", note.Width),
                new("@height", note.Height),

                new("@gravityX", note.GravityX),
                new("@gravityY", note.GravityY),

                new("@themeColorScheme", note.ThemeColorScheme),

                new("@isPinned", note.IsPinned),
                new("@isOpen", note.IsOpen),

                new("@id", note.Id)
            ]
        );

        if (affectedRows == 0)
            throw new Exception($"Error updating note width id {note.Id}.");
    }

    public async Task Delete(int id)
    {
        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        await ExecuteNonQuery(
            connection,
            @"
                DELETE FROM Notes
                WHERE Id = @id;
            ",
            parameters: [
                new("@id", id)
            ]
        );
    }

    private static NoteDto GetNoteDtoFromReader(SqliteDataReader reader)
    {
        NoteDto noteDto = new(
            GetInt(reader, "Id"),

            GetString(reader, "Content"),

            GetDouble(reader, "X"),
            GetDouble(reader, "Y"),
            GetDouble(reader, "Width"),
            GetDouble(reader, "Height"),

            GetInt(reader, "GravityX"),
            GetInt(reader, "GravityY"),

            GetStringNullable(reader, "ThemeColorScheme"),

            GetBool(reader, "IsPinned"),
            GetBool(reader, "IsOpen")
        );

        return noteDto;
    }
}
