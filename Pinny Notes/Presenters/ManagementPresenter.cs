using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class ManagementPresenter
{
    private readonly ApplicationManager _applicationManager;
    private readonly ManagementModel _model;
    private readonly ManagementWindow _view;

    public ManagementPresenter(ApplicationManager applicationManager, ManagementModel model, ManagementWindow view)
    {
        _applicationManager = applicationManager;
        _model = model;
        _view = view;

        Initialize();
    }

    private void Initialize()
    {
        foreach (NoteModel note in _applicationManager.GetNotes())
            _model.NotePreviews.Add(new(note));
        _view.DisplayNotes(_model.NotePreviews);
    }

    public void ShowWindow()
    {
        _view.Show();
        _view.Activate();
    }
}
