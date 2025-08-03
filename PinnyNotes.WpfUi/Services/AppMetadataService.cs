using PinnyNotes.DataAccess.Models;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Services;

public class AppMetadataService
{
    private readonly DatabaseService _databaseService;

    public AppMetadataModel Metadata { get; }

    public AppMetadataService(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AppMetadataDataModel appMetadata = _databaseService.AppMetadataRepository.GetById(1);

        Metadata = new()
        {
            LastUpdateCheck = appMetadata.LastUpdateCheck,
            ThemeColor = appMetadata.ThemeColor
        };
    }

    public void Save()
    {
        _databaseService.AppMetadataRepository.Update(
            new AppMetadataDataModel(
                Id: 1,

                LastUpdateCheck: Metadata.LastUpdateCheck,
                ThemeColor: Metadata.ThemeColor
            )
        );
    }
}
