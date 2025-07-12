using PinnyNotes.DataAccess.Models;

namespace PinnyNotes.WpfUi.Services;

public class SettingsService
{
    private readonly DatabaseService _databaseService;

    public SettingsDataModel AppSettings { get; }

    public SettingsService(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AppSettings = _databaseService.SettingsRepository.GetById(1);
    }

    public void Save()
    {
        _databaseService.SettingsRepository.Update(AppSettings);
    }
}
