using System.Threading.Tasks;

using PinnyNotes.Core.DataTransferObjects;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Services;

public class AppMetadataService(DatabaseService databaseService)
{
    private readonly DatabaseService _databaseService = databaseService;

    public AppMetadataModel Metadata => _metadata;
    private AppMetadataModel _metadata = null!;

    public async Task Load()
    {
        AppMetadataDataDto appMetadata = await _databaseService.GetAppMetadata(1);

        _metadata = new()
        {
            LastUpdateCheck = appMetadata.LastUpdateCheck,
            ThemeColor = appMetadata.ThemeColor
        };
    }

    public async Task Save()
    {
        _ = await _databaseService.UpdateAppMetadata(
            new AppMetadataDataDto(
                Id: 1,

                LastUpdateCheck: _metadata.LastUpdateCheck,
                ThemeColor: _metadata.ThemeColor
            )
        );
    }
}
