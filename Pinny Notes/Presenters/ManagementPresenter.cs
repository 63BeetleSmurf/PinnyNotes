using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Views;
using PinnyNotes.WpfUi.Views.Controls.ContextMenus;

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

        _managementService.NotesChanged += OnNotesChanged;

        _view.Closed += managementService.OnManagementWindowClosed;
        _view.NotesListView.MouseDoubleClick += OnNoteItemDoubleclick;
        _view.NotesListView.MouseRightButtonUp += OnNoteViewMouseRightButtonUp;

        Initialize();

        _view.Show();
    }

    private void Initialize()
    {
        UpdateNotes();
    }

    public void ShowWindow()
    {
        _view.Show();
        _view.Activate();
    }

    private void OnNotesChanged(object? sender, EventArgs e)
    {
        UpdateNotes();
    }

    private void OnNoteItemDoubleclick(object? sender, EventArgs e)
    {
        ListView? noteListView = sender as ListView;
        if (noteListView == null)
            return;

        NoteModel? selectedNote = noteListView.SelectedItem as NoteModel;
        if (selectedNote == null)
            return;

        _managementService.ActivateNote(selectedNote.Id);
    }

    private void OnNoteViewMouseRightButtonUp(object? sender, MouseButtonEventArgs e)
    {
        // Hit testing to determine where the right-click occurred, allowing right
        // click in whitespace to deselect notes and show different menu.
        var hitTestResult = VisualTreeHelper.HitTest(_view.NotesListView, e.GetPosition(_view.NotesListView));
        var item = hitTestResult?.VisualHit as FrameworkElement;
        var listViewItem = item?.DataContext as NoteModel;

        if (listViewItem == null && !KeyboardHelper.IsShiftPressed() && !KeyboardHelper.IsControlPressed())
        {
            _view.NotesListView.UnselectAll();

            _view.NotesListView.ContextMenu = new ManageNotesNoneContextMenu(
                OnCreateNoteClicked,
                OnShowAllClicked,
                OnCloseAllClicked
            );
        }
        else
        {
            List<int> selectedNoteIds = [];
            foreach (NoteModel note in _view.NotesListView.SelectedItems)
                selectedNoteIds.Add(note.Id);

            _view.NotesListView.ContextMenu = new ManageNotesSelectionContextMenu(
                selectedNoteIds,
                OnOpenClicked,
                OnCloseClicked,
                OnDeleteClicked
            );
        }
    }

    private void OnCreateNoteClicked(object? sender, EventArgs e)
    {
        _managementService.CreateNewNote();
    }

    private void OnShowAllClicked(object? sender, EventArgs e)
    {
        foreach (NoteModel noteModel in _model.Notes)
            _managementService.ActivateNote(noteModel.Id);
    }

    private void OnCloseAllClicked(object? sender, EventArgs e)
    {
        foreach (NoteModel noteModel in _model.Notes)
            _managementService.CloseNote(noteModel.Id);
    }

    private void OnOpenClicked(object? sender, EventArgs e)
    {
        IEnumerable<int>? noteIds = GetIdsFromMenuItemTag(sender);
        if (noteIds == null)
            return;

        foreach (int noteId in noteIds)
            _managementService.ActivateNote(noteId);
    }

    private void OnCloseClicked(object? sender, EventArgs e)
    {
        IEnumerable<int>? noteIds = GetIdsFromMenuItemTag(sender);
        if (noteIds == null)
            return;

        foreach (int noteId in noteIds)
            _managementService.CloseNote(noteId);
    }

    private void OnDeleteClicked(object? sender, EventArgs e)
    {
        IEnumerable<int>? noteIds = GetIdsFromMenuItemTag(sender);
        if (noteIds == null)
            return;

        foreach (int noteId in noteIds)
            _managementService.DeleteNote(noteId);
    }

    private IEnumerable<int>? GetIdsFromMenuItemTag(object? sender)
    {
        MenuItem? menuItem = sender as MenuItem;
        if (menuItem == null)
            return null;

        return menuItem.Tag as IEnumerable<int>;
    }

    private void UpdateNotes()
    {
        _model.Notes = _managementService.GetNotes();
        _view.DisplayNotes(_model.Notes);
    }
}
