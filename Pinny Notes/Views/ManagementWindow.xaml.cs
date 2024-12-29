using System.Collections.Generic;
using System.Windows;

using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Presenters;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.Views;

public partial class ManagementWindow : Window
{
    private readonly ManagementPresenter _presenter;

    public ManagementWindow(ManagementService managementService, ManagementModel model)
    {
        InitializeComponent();

        _presenter = new(managementService, model, this);
    }

    public void DisplayNotes(IEnumerable<NoteModel> notes)
    {
        NotesListView.ItemsSource = notes;
    }
}
