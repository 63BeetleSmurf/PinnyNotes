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
}
