using Microsoft.Extensions.DependencyInjection;
using System;

using PinnyNotes.WpfUi.Factories;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Services;

public class WindowService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MessengerService _messengerService;
    private readonly NoteWindowFactory _noteWindowFactory;
    private SettingsWindow? _settingsWindow;

    public WindowService(IServiceProvider serviceProvider, MessengerService messengerService, NoteWindowFactory noteWindowFactory)
    {
        _serviceProvider = serviceProvider;
        _messengerService = messengerService;
        _noteWindowFactory = noteWindowFactory;

        _messengerService.Subscribe<CreateNewNoteMessage>(OnCreateNewNoteMessage);
        _messengerService.Subscribe<OpenSettingsWindowMessage>(OnOpenSettingsWindowMessage);
    }

    private void OnCreateNewNoteMessage(CreateNewNoteMessage message)
    {
        NoteWindow newNote = _noteWindowFactory.Create(message.ParentViewModel);
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
