using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using PinnyNotes.WpfUi.Helpers;
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

        NotesListView.MouseUp += OnListViewMouseUp;
    }

    public void DisplayNotes(IEnumerable<NoteModel> notes)
    {
        NotesListView.ItemsSource = notes;
    }

    private void OnListViewMouseUp(object? sender, MouseButtonEventArgs e)
    {
        // Hit testing to determine where the right-click occurred
        var hitTestResult = VisualTreeHelper.HitTest(NotesListView, e.GetPosition(NotesListView));
        var item = hitTestResult?.VisualHit as FrameworkElement;
        var listViewItem = item?.DataContext as NoteModel;

        if (listViewItem == null && !KeyboardHelper.IsShiftPressed() && !KeyboardHelper.IsControlPressed())
            NotesListView.UnselectAll();
    }
}
