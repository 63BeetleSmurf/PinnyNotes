using System;
using System.Collections.Generic;

using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Repositories;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Services;

public class NoteService
{
    private readonly ApplicationManager _applicationManager;
    private readonly NoteRepository _noteRepository;

    private readonly Dictionary<int, NoteWindow> _openNotes = [];

    public NoteService(ApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
        _noteRepository = new(_applicationManager.ConnectionString);
    }

    public event EventHandler? NotesChanged;

    public event EventHandler? SettingsChanged;
    public event EventHandler? ActivateNotes;

    public void OpenNewNote(NoteModel? parent = null, int? groupId = null)
    {
        NoteModel model = new()
        {
            Id = _noteRepository.Create(),
            GroupId = parent?.GroupId ?? groupId,
            Settings = GetNoteSettings(groupId: groupId)
        };
        model.Initialize(parent);

        SaveNote(model);

        _openNotes.Add(
            model.Id,
            new NoteWindow(this, model)
        );
    }

    public void OpenExistingNote(int noteId)
    {
        if (_openNotes.ContainsKey(noteId))
        {
            _openNotes[noteId].Activate();
            return;
        }

        NoteModel model = _noteRepository.GetById(noteId);
        model.Settings = GetNoteSettings(model.SettingsId, model.GroupId);

        _openNotes.Add(
            model.Id,
            new NoteWindow(this, model)
        );
    }

    public void CloseNote(NoteModel model, NoteWindow window)
    {
        window.Close();

        if (string.IsNullOrWhiteSpace(model.Text))
            DeleteNote(model.Id);
        else
            SaveNote(model);

        _openNotes.Remove(model.Id);
    }

    public void SaveNote(NoteModel model)
    {
        if (model.IsSaved)
            return;

        _noteRepository.Update(model);
        model.IsSaved = true;

        NotesChanged?.Invoke(null, EventArgs.Empty);
    }

    public void DeleteNote(int noteId)
    {
        _noteRepository.Delete(noteId);

        NotesChanged?.Invoke(null, EventArgs.Empty);
    }

    public void OpenSettingsWindowOnNote(NoteWindow window)
    {
        _applicationManager.SettingsService.OpenSettingsWindow(window);
    }

    public void OnActivateNotes(object? sender, EventArgs e)
    {
        ActivateNotes?.Invoke(sender, e);
    }

    public void OnSettingsSaved(object? sender, EventArgs e)
    {
        SettingsChanged?.Invoke(sender, e);
    }

    public IEnumerable<NoteModel> GetNotes()
    {
        IEnumerable<NoteModel> notes = _noteRepository.GetAll();
        foreach (NoteModel note in notes)
            note.Settings = GetNoteSettings(note.SettingsId, note.GroupId);
        return notes;
    }

    private SettingsModel GetNoteSettings(int? settingsId = null, int? groupId = null)
    {
        if (settingsId != null)
            return _applicationManager.SettingsService.GetSettings((int)settingsId);

        SettingsModel? settings = null;

        if (groupId != null)
            settings = _applicationManager.GroupService.GetGroupSettings((int)groupId);

        return settings ?? _applicationManager.ApplicationSettings;
    }
}
