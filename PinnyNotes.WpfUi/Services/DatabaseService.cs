using System;
using System.IO;

using PinnyNotes.DataAccess;
using PinnyNotes.DataAccess.Repositories;

namespace PinnyNotes.WpfUi.Services;

public class DatabaseService
{
    private string _connectionString { get; }

    public ApplicationDataRepository ApplicationDataRepository { get; }
    public SettingsRepository SettingsRepository { get; }

    public DatabaseService()
    {
        string dataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Pinny Notes"
        );
        if (!Path.Exists(dataPath))
            Directory.CreateDirectory(dataPath);

        _connectionString = $"Data Source={Path.Combine(dataPath, "pinny_notes.sqlite")}";

        DatabaseInitialiser.Initialise(_connectionString);
        ApplicationDataRepository = new(_connectionString);
        SettingsRepository = new(_connectionString);
    }
}
