using System;
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

    public event EventHandler? NotesChanged;

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

    public void OnNotesChanged(object? sender, EventArgs e)
    {
        NotesChanged?.Invoke(sender, e);
    }

    public void OnManagementWindowClosed(object? sender, EventArgs e)
    {
        _window = null;
    }

    public IEnumerable<NoteModel> GetNotes()
    {
        return _applicationManager.NoteService.GetNotes();
    }

    public void ActivateNote(int noteId)
    {
        _applicationManager.NoteService.OpenExistingNote(noteId);
    }

    public void CreateNewNote(int? groupId = null)
    {
        _applicationManager.NoteService.OpenNewNote(groupId: groupId);
    }

    public void CloseNote(int noteId)
    {
        _applicationManager.NoteService.CloseNote(noteId);
    }

    public void DeleteNote(int noteId)
    {
        _applicationManager.NoteService.DeleteNote(noteId);
    }
}
