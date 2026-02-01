using Microsoft.Extensions.DependencyInjection;
using System;

using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.ViewModels;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Services;

public class WindowService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MessengerService _messengerService;
    private readonly AppMetadataService _appMetadataService;
    private readonly SettingsService _settingsService;
    private SettingsWindow? _settingsWindow;

    public WindowService(IServiceProvider serviceProvider, MessengerService messengerService, AppMetadataService appMetadataService, SettingsService settingsService)
    {
        _serviceProvider = serviceProvider;
        _messengerService = messengerService;
        _appMetadataService = appMetadataService;
        _settingsService = settingsService;

        _messengerService.Subscribe<CreateNewNoteMessage>(OnCreateNewNoteMessage);
        _messengerService.Subscribe<OpenSettingsWindowMessage>(OnOpenSettingsWindowMessage);
    }

    private void OnCreateNewNoteMessage(CreateNewNoteMessage message)
    {
        NoteViewModel viewModel = new(_appMetadataService, _settingsService, _messengerService, message.ParentViewModel);
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
