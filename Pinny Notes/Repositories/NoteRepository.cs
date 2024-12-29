using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Repositories;

public class NoteRepository(string connectionString) : BaseRepository(connectionString)
{
    public static readonly string TableName = "Notes";

    public static readonly string TableSchema = @"
        (
            Id          INTEGER PRIMARY KEY AUTOINCREMENT,

            SettingsId  INTEGER DEFAULT NULL,
            GroupId     INTEGER DEFAULT NULL,

            Title       TEXT    DEFAULT '',
            Content     TEXT    DEFAULT '',

            X           INTEGER DEFAULT NULL,
            Y           INTEGER DEFAULT NULL,
            Width       INTEGER DEFAULT NULL,
            Height      INTEGER DEFAULT NULL,

            GravityX    INTEGER DEFAULT NULL,
            GravityY    INTEGER DEFAULT NULL,

            ThemeKey    TEXT    DEFAULT NULL,

            IsPinned    INTEGER DEFAULT 0,

            FOREIGN KEY(""SettingsId"") REFERENCES ""Settings""(""Id""),
            FOREIGN KEY(""GroupId"")    REFERENCES ""Groups""(""Id"")
        )
    ";

    public int Create()
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        ExecuteNonQuery(
            connection,
            @"
                INSERT INTO
                    Notes
                DEFAULT VALUES;
            "
        );

        return GetLastInsertRowId(connection);
    }

    public NoteModel GetById(int id)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        using SqliteDataReader reader = ExecuteReader(
            connection,
            @"
                SELECT
                    *
                FROM
                    Notes
                WHERE
                    Id = @id;
            ",
            parameters: [
                new("@id", id)
            ]
        );
        if (!reader.Read())
            throw new Exception($"Could not find note with id: {id}.");

        return GetNoteModelFromReader(reader);
    }

    public IEnumerable<NoteModel> GetAll()
    {
        List<NoteModel> notes = [];

        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        using SqliteDataReader reader = ExecuteReader(
            connection,
            @"
                SELECT
                    *
                FROM
                    Notes;
            "
        );

        while (reader.Read())
            notes.Add(GetNoteModelFromReader(reader));

        return notes;
    }

    public void Update(NoteModel model)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        int affectedRows = ExecuteNonQuery(
            connection,
            @"
                UPDATE
                    Notes
                SET
                    Content = @content,

                    X = @x,
                    Y = @y,
                    Width = @width,
                    Height = @height,

                    GravityX = @gravityX,
                    GravityY = @gravityY,

                    ThemeKey = @themeKey,

                    IsPinned = @isPinned
                WHERE
                    Id = @id;
            ",
            parameters: [
                new("@content", model.Text),

                new("@x", model.X),
                new("@y", model.Y),
                new("@width", model.Width),
                new("@height", model.Height),

                new("@gravityX", model.GravityX),
                new("@gravityY", model.GravityY),

                new("@themeKey", model.Theme.Key),

                new("@isPinned", model.IsPinned),

                new("@id", model.Id)
            ]
        );

        if (affectedRows == 0)
            throw new Exception($"Error updating note width id {model.Id}.");
    }

    public void Delete(int id)
    {
        using SqliteConnection connection = new(_connectionString);
        connection.Open();

        ExecuteNonQuery(
            connection,
            @"
                DELETE FROM
                    Notes
                WHERE
                    Id = @id;
            ",
            parameters: [
                new("@id", id)
            ]
        );
    }

    private static NoteModel GetNoteModelFromReader(SqliteDataReader reader)
    {
        return new()
        {
            Id = GetInt(reader, "Id"),

            SettingsId = GetIntNullable(reader, "SettingsId"),
            GroupId = GetIntNullable(reader, "GroupId"),

            Text = GetString(reader, "Content"),

            X = GetInt(reader, "X"),
            Y = GetInt(reader, "Y"),
            Width = GetInt(reader, "Width"),
            Height = GetInt(reader, "Height"),

            GravityX = GetInt(reader, "GravityX"),
            GravityY = GetInt(reader, "GravityY"),

            Theme = ThemeHelper.GetThemeOrDefault(GetString(reader, "ThemeKey")),

            IsPinned = GetBool(reader, "IsPinned")
        };
    }
}
