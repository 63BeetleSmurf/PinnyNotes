using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Repositories;

namespace PinnyNotes.WpfUi.Services;

public class GroupService
{
    private readonly ApplicationManager _applicationManager;
    private readonly GroupRepository _groupRepository;

    public GroupService(ApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
        _groupRepository = new(_applicationManager.ConnectionString);
    }

    public SettingsModel GetGroupSettings(int groupId)
    {
        GroupModel? group = _groupRepository.GetById(groupId);
        if (group?.SettingsId != null)
            return _applicationManager.SettingsService.GetSettings((int)group.SettingsId);

        return _applicationManager.ApplicationSettings;
    }
}
