using Microsoft.Extensions.DependencyInjection;

using PinnyNotes.Core.Repositories;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.ViewModels;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Services;

public class WindowService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MessengerService _messengerService;
    private readonly AppMetadataService _appMetadataService;
    private readonly SettingsService _settingsService;
    private readonly NoteRepository _noteRepository;
    private SettingsWindow? _settingsWindow;

    public WindowService(IServiceProvider serviceProvider, MessengerService messengerService, AppMetadataService appMetadataService, SettingsService settingsService, NoteRepository noteRepository)
    {
        _serviceProvider = serviceProvider;
        _messengerService = messengerService;
        _appMetadataService = appMetadataService;
        _settingsService = settingsService;
        _noteRepository = noteRepository;

        _messengerService.Subscribe<OpenNoteWindowMessage>(OnOpenNoteWindowMessage);
        _messengerService.Subscribe<OpenSettingsWindowMessage>(OnOpenSettingsWindowMessage);
    }

    private void OnOpenNoteWindowMessage(OpenNoteWindowMessage message)
        => _ = OpenNoteWindow(message.NoteId, message.ParentNote);

    private async Task OpenNoteWindow(int? noteId = null, NoteModel? parentNote = null)
    {
        NoteViewModel viewModel = new(_noteRepository, _appMetadataService, _settingsService, _messengerService);
        await viewModel.Initialize(noteId, parentNote);
        NoteWindow newNote = new(_settingsService, _messengerService, viewModel);

        newNote.Show();
    }

    private void OnOpenSettingsWindowMessage(OpenSettingsWindowMessage message)
    {
        if (_settingsWindow == null || !_settingsWindow.IsLoaded)
        {
            _settingsWindow = _serviceProvider.GetRequiredService<SettingsWindow>();
            _settingsWindow.Closed += (s, e) => _settingsWindow = null;
        }

        _settingsWindow.Owner = message.Owner;

        if (!_settingsWindow.IsVisible)
            _settingsWindow.Show();

        _settingsWindow.Activate();
    }
}
