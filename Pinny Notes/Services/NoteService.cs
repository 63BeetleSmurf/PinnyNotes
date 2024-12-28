﻿using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Repositories;
using PinnyNotes.WpfUi.Views;
using System;

namespace PinnyNotes.WpfUi.Services;

public class NoteService
{
    private ApplicationManager _applicationManager;
    private NoteRepository _noteRepository;

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
        _applicationManager.ShowSettingsWindow(window);
    }

    public void ActivateNoteWindows()
    {
        ActivateNotes?.Invoke(null, EventArgs.Empty);
    }

    public void ReloadNoteSettings()
    {
        SettingsChanged?.Invoke(null, EventArgs.Empty);
    }

    private SettingsModel GetNoteSettings(NoteModel model)
    {
        if (model.SettingsId != null)
            return _applicationManager.SettingsService.GetSettings(model.SettingsId);

        SettingsModel? settings = null;

        if (model.GroupId != null)
            settings = _applicationManager.GroupService.GetGroupSettings(model.GroupId);

        return settings ?? _applicationManager.ApplicationSettings;
    }
}