using Microsoft.Data.Sqlite;

using PinnyNotes.Core.Configurations;
using PinnyNotes.Core.DataTransferObjects;

namespace PinnyNotes.Core.Repositories;

public class NoteRepository(DatabaseConfiguration databaseConfiguration) : BaseRepository(databaseConfiguration)
{
    public static readonly string TableName = "Notes";

    public static readonly string TableSchema = @"
        (
            Id                  INTEGER     NOT NULL    PRIMARY KEY AUTOINCREMENT,

            Content             TEXT        NOT NULL,

            X                   REAL        NOT NULL,
            Y                   REAL        NOT NULL,
            Width               REAL        NOT NULL,
            Height              REAL        NOT NULL,

            GravityX            INTEGER     NOT NULL,
            GravityY            INTEGER     NOT NULL,

            ThemeColourScheme   TEXT        NOT NULL,

            IsPinned            INTEGER     NOT NULL,
            IsOpen              INTEGER     NOT NULL
        )
    ";

    public async Task<int> Create(NoteDto note)
    {
        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        await ExecuteNonQuery(
            connection,
            @"
                INSERT INTO Notes
                (
                    Content,

                    X,
                    Y,
                    Width,
                    Height,

                    GravityX,
                    GravityY,

                    ThemeColourScheme,

                    IsPinned,
                    IsOpen
                )
                VALUES
                (
                    @content,

                    @x,
                    @y,
                    @width,
                    @height,

                    @gravityX,
                    @gravityY,

                    @themeColourScheme,

                    @isPinned,
                    @isOpen
                );
            ",
            parameters: [
                new("@content", note.Content),

                new("@x", note.X),
                new("@y", note.Y),
                new("@width", note.Width),
                new("@height", note.Height),

                new("@gravityX", note.GravityX),
                new("@gravityY", note.GravityY),

                new("@themeColourScheme", note.ThemeColourScheme),

                new("@isPinned", note.IsPinned),
                new("@isOpen", note.IsOpen)
            ]
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

    public async Task<IEnumerable<NoteDto>> GetOpen()
    {
        List<NoteDto> notes = [];

        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        using SqliteDataReader reader = await ExecuteReader(
            connection,
            @"
                SELECT *
                FROM Notes
                WHERE IsOpen = 1;
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

                    ThemeColourScheme = @themeColourScheme,

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

                new("@themeColourScheme", note.ThemeColourScheme),

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

            GetString(reader, "ThemeColourScheme"),

            GetBool(reader, "IsPinned"),
            GetBool(reader, "IsOpen")
        );

        return noteDto;
    }
}
