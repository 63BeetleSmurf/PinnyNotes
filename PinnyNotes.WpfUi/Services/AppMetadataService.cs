using PinnyNotes.Core.DataTransferObjects;
using PinnyNotes.Core.Repositories;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Services;

public class AppMetadataService(AppMetadataRepository appMetadataRepository)
{
    private readonly AppMetadataRepository _appMetadataRepository = appMetadataRepository;

    public AppMetadataModel Metadata => _metadata;
    private AppMetadataModel _metadata = null!;

    public async Task Load()
    {
        AppMetadataDataDto appMetadata = await _appMetadataRepository.GetById(1);

        _metadata = new()
        {
            LastUpdateCheck = appMetadata.LastUpdateCheck,
            ColorScheme = appMetadata.ColorScheme
        };
    }

    public async Task Save()
    {
        _ = await _appMetadataRepository.Update(
            new AppMetadataDataDto(
                Id: 1,

                LastUpdateCheck: _metadata.LastUpdateCheck,
                ColorScheme: _metadata.ColorScheme
            )
        );
    }
}
