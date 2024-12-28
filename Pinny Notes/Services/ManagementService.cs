using System.Collections.Generic;

using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Services;

public class ManagementService
{
    private ManagementWindow? _window = null;

    private readonly ApplicationManager _applicationManager;

    public ManagementService(ApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
    }

    public void OpenManagementWindow()
    {
        if (_window == null)
            _window = new(
                this,
                new ManagementModel()
            );
        else
            _window.Activate();
    }

    public void CloseManagementWindow()
    {
        _window?.Close();
        _window = null;
    }

    public IEnumerable<NoteModel> GetNotes()
    {
        return _applicationManager.NoteService.GetNotes();
    }
}
