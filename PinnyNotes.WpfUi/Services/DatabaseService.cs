using System;
using System.IO;
using System.Threading.Tasks;

using PinnyNotes.DataAccess;
using PinnyNotes.DataAccess.Models;
using PinnyNotes.DataAccess.Repositories;

namespace PinnyNotes.WpfUi.Services;

public class DatabaseService
{
    private readonly string _connectionString;

    private readonly AppMetadataRepository _appMetadataRepository;
    private readonly SettingsRepository _settingsRepository;

    public DatabaseService()
    {
        string dataPath;
        // Use exe dir for database if in Debug mode or is portable.
        if (App.IsDebugMode || File.Exists(Path.Combine(AppContext.BaseDirectory, "portable.txt")))
            dataPath = AppContext.BaseDirectory;
        else
            dataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Pinny Notes"
            );

        if (!Path.Exists(dataPath))
            Directory.CreateDirectory(dataPath);

        _connectionString = $"Data Source={Path.Combine(dataPath, "pinny_notes.sqlite")}";

        _appMetadataRepository = new(_connectionString);
        _settingsRepository = new(_connectionString);
    }

    public async Task Initialise()
        => await DatabaseInitialiser.Initialise(_connectionString);

    public async Task<AppMetadataDataModel> GetAppMetadata(int id)
        => await _appMetadataRepository.GetById(id);

    public async Task<int> UpdateAppMetadata(AppMetadataDataModel model)
        => await _appMetadataRepository.Update(model);

    public async Task<SettingsDataModel> GetSettings(int id)
        => await _settingsRepository.GetById(id);

    public async Task<int> UpdateSettings(SettingsDataModel model)
        => await _settingsRepository.Update(model);
}
