using System;
using System.IO;

using PinnyNotes.DataAccess;
using PinnyNotes.DataAccess.Repositories;

namespace PinnyNotes.WpfUi.Services;

public class DatabaseService
{
    private readonly string _connectionString;

    public AppMetadataRepository AppMetadataRepository { get; }
    public SettingsRepository SettingsRepository { get; }

    public DatabaseService()
    {
        string dataPath;
        if (File.Exists(Path.Combine(AppContext.BaseDirectory, "portable.txt")))
            dataPath = AppContext.BaseDirectory;
        else
            dataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Pinny Notes"
            );

        if (!Path.Exists(dataPath))
            Directory.CreateDirectory(dataPath);

        _connectionString = $"Data Source={Path.Combine(dataPath, "pinny_notes.sqlite")}";

        DatabaseInitialiser.Initialise(_connectionString);
        AppMetadataRepository = new(_connectionString);
        SettingsRepository = new(_connectionString);
    }
}
