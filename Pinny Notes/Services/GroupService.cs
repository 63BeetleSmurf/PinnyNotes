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
}
