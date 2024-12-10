using Microsoft.Data.Sqlite;
using System;

namespace PinnyNotes.WpfUi.Repositories;

public abstract class BaseRepository
{
    protected bool GetBool(SqliteDataReader reader, int ordinal)
        => reader.GetBoolean(ordinal);
    protected bool GetBool(SqliteDataReader reader, string columnName)
        => GetBool(reader, reader.GetOrdinal(columnName));
    protected bool? GetBoolNullable(SqliteDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return null;
        return GetBool(reader, ordinal);
    }

    protected int GetInt(SqliteDataReader reader, int ordinal)
        => reader.GetInt32(ordinal);
    protected int GetInt(SqliteDataReader reader, string columnName)
        => GetInt(reader, reader.GetOrdinal(columnName));
    protected int? GetIntNullable(SqliteDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return null;
        return GetInt(reader, ordinal);
    }

    protected string GetString(SqliteDataReader reader, int ordinal)
        => reader.GetString(ordinal);
    protected string GetString(SqliteDataReader reader, string columnName)
        => GetString(reader, reader.GetOrdinal(columnName));
    protected string? GetStringNullable(SqliteDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return null;
        return GetString(reader, ordinal);
    }

    protected T GetEnum<T>(SqliteDataReader reader, int ordinal) where T : Enum
    {
        Type enumType = typeof(T);
        int value = reader.GetInt32(ordinal);

        if (!Enum.IsDefined(enumType, value))
            throw new ArgumentException($"'{value}' is not defined in {enumType.Name}.");

        return (T)Enum.ToObject(enumType, value);
    }
    protected T GetEnum<T>(SqliteDataReader reader, string columnName) where T : Enum
        => GetEnum<T>(reader, reader.GetOrdinal(columnName));
    protected T? GetEnumNullable<T>(SqliteDataReader reader, string columnName) where T : Enum
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return default;
        return GetEnum<T>(reader, ordinal);
    }
}
