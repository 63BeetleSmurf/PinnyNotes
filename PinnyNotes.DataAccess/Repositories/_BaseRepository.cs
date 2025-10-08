using Microsoft.Data.Sqlite;

namespace PinnyNotes.DataAccess.Repositories;

public abstract class BaseRepository(string connectionString)
{
    protected readonly string _connectionString = connectionString;

    private static SqliteCommand CreateCommand(SqliteConnection connection, string query, IEnumerable<KeyValuePair<string, object?>>? parameters = null)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = query;
        if (parameters != null)
            foreach (KeyValuePair<string, object?> parameter in parameters)
                command.Parameters.AddWithValue(
                    parameter.Key,
                    parameter.Value ?? DBNull.Value
                );

        return command;
    }

    protected async static Task<object?> ExecuteScalar(SqliteConnection connection, string query, IEnumerable<KeyValuePair<string, object?>>? parameters = null)
    {
        using SqliteCommand command = CreateCommand(connection, query, parameters);
        return await command.ExecuteScalarAsync();
    }

    public async static Task<int> ExecuteNonQuery(SqliteConnection connection, string query, IEnumerable<KeyValuePair<string, object?>>? parameters = null)
    {
        using SqliteCommand command = CreateCommand(connection, query, parameters);
        return await command.ExecuteNonQueryAsync();
    }

    protected async static Task<SqliteDataReader> ExecuteReader(SqliteConnection connection, string query, IEnumerable<KeyValuePair<string, object?>>? parameters = null)
    {
        SqliteCommand command = CreateCommand(connection, query, parameters);
        return await command.ExecuteReaderAsync();
    }

    protected async static Task<int> GetLastInsertRowId(SqliteConnection connection)
        => Convert.ToInt32(
            await ExecuteScalar(connection, "SELECT last_insert_rowid();")
        );

    protected static bool GetBool(SqliteDataReader reader, int ordinal)
        => reader.GetBoolean(ordinal);
    protected static bool GetBool(SqliteDataReader reader, string columnName)
        => GetBool(reader, reader.GetOrdinal(columnName));
    protected static bool? GetBoolNullable(SqliteDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return null;
        return GetBool(reader, ordinal);
    }

    protected static int GetInt(SqliteDataReader reader, int ordinal)
        => reader.GetInt32(ordinal);
    protected static int GetInt(SqliteDataReader reader, string columnName)
        => GetInt(reader, reader.GetOrdinal(columnName));
    protected static int? GetIntNullable(SqliteDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return null;
        return GetInt(reader, ordinal);
    }

    protected static long GetLong(SqliteDataReader reader, int ordinal)
        => reader.GetInt64(ordinal);
    protected static long GetLong(SqliteDataReader reader, string columnName)
        => GetLong(reader, reader.GetOrdinal(columnName));
    protected static long? GetLongNullable(SqliteDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return null;
        return GetLong(reader, ordinal);
    }

    protected static double GetDouble(SqliteDataReader reader, int ordinal)
        => reader.GetDouble(ordinal);
    protected static double GetDouble(SqliteDataReader reader, string columnName)
        => GetDouble(reader, reader.GetOrdinal(columnName));
    protected static double? GetDoubleNullable(SqliteDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return null;
        return GetDouble(reader, ordinal);
    }

    protected static string GetString(SqliteDataReader reader, int ordinal)
        => reader.GetString(ordinal);
    protected static string GetString(SqliteDataReader reader, string columnName)
        => GetString(reader, reader.GetOrdinal(columnName));
    protected static string? GetStringNullable(SqliteDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return null;
        return GetString(reader, ordinal);
    }

    protected static T GetEnum<T>(SqliteDataReader reader, int ordinal) where T : Enum
    {
        Type enumType = typeof(T);
        int value = reader.GetInt32(ordinal);

        if (!Enum.IsDefined(enumType, value))
            throw new ArgumentException($"'{value}' is not defined in {enumType.Name}.");

        return (T)Enum.ToObject(enumType, value);
    }
    protected static T GetEnum<T>(SqliteDataReader reader, string columnName) where T : Enum
        => GetEnum<T>(reader, reader.GetOrdinal(columnName));
    protected static T? GetEnumNullable<T>(SqliteDataReader reader, string columnName) where T : Enum
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            return default;
        return GetEnum<T>(reader, ordinal);
    }

    protected static Type GetValueType(SqliteDataReader reader, int ordinal)
    {
        string value = reader.GetString(ordinal);

        return value switch
        {
            "bool" => typeof(bool),
            "int" => typeof(int),
            "long" => typeof(long),
            "double" => typeof(double),
            "string" => typeof(string),
            _ => throw new ArgumentException($"Unknown type: {value}"),
        };
    }
    protected static Type GetValueType(SqliteDataReader reader, string columnName)
        => GetValueType(reader, reader.GetOrdinal(columnName));
}
