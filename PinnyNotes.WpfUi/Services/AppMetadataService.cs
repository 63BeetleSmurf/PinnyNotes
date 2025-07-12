using PinnyNotes.DataAccess.Models;

namespace PinnyNotes.WpfUi.Services;

public class AppMetadataService
{
    private readonly DatabaseService _databaseService;

    public AppMetadataDataModel AppData { get; }

    public AppMetadataService(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AppData = _databaseService.AppMetadataRepository.Get();
    }

    public void Save()
    {
        _databaseService.AppMetadataRepository.Update(AppData);
    }
}
