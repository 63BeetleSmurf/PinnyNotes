using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class ManagementPresenter
{
    private readonly ManagementService _managementService;
    private readonly ManagementModel _model;
    private readonly ManagementWindow _view;

    public ManagementPresenter(ManagementService managementService, ManagementModel model, ManagementWindow view)
    {
        _managementService = managementService;
        _model = model;
        _view = view;

        _view.Closed += managementService.OnManagementWindowClosed;

        Initialize();

        _view.Show();
    }

    private void Initialize()
    {
        _model.Notes = _managementService.GetNotes();
        _view.DisplayNotes(_model.Notes);
    }

    public void ShowWindow()
    {
        _view.Show();
        _view.Activate();
    }
}
