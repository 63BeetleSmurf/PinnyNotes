using PinnyNotes.DataAccess.Models;

namespace PinnyNotes.WpfUi.Services;

public class ApplicationDataService
{
    private readonly DatabaseService _databaseService;

    public ApplicationDataModel AppData { get; }

    public ApplicationDataService(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AppData = _databaseService.ApplicationDataRepository.Get();
    }

    public void SaveData()
    {
        _databaseService.ApplicationDataRepository.Update(AppData);
    }
}
