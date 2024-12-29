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

    public NoteService(ApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
        _noteRepository = new(_applicationManager.ConnectionString);
    }

    public event EventHandler? SettingsChanged;
    public event EventHandler? ActivateNotes;

    public void OpenNewNote(NoteModel? parent = null, int? groupId = null)
    {
        NoteModel model = new()
        {
            Id = _noteRepository.Create(),
            GroupId = parent?.GroupId ?? groupId,
        };
        model.Initialize(
            GetNoteSettings(model),
            parent
        );

        SaveNote(model);

        new NoteWindow(this, model);
    }

    public void OpenExistingNote(int noteId)
    {
        NoteModel model = _noteRepository.GetById(noteId);
        model.Initialize(
            GetNoteSettings(model)
        );

        new NoteWindow(this, model);
    }

    public void CloseNote(NoteModel model, NoteWindow window)
    {
        SaveNote(model);
        window.Close();
    }

    public void SaveNote(NoteModel model)
    {
        if (!model.IsSaved)
            _noteRepository.Update(model);
            model.IsSaved = true;
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
        return _noteRepository.GetAll();
    }

    private SettingsModel GetNoteSettings(NoteModel model)
    {
        if (model.SettingsId != null)
            return _applicationManager.SettingsService.GetSettings((int)model.SettingsId);

        SettingsModel? settings = null;

        if (model.GroupId != null)
            settings = _applicationManager.GroupService.GetGroupSettings((int)model.GroupId);

        return settings ?? _applicationManager.ApplicationSettings;
    }
}
