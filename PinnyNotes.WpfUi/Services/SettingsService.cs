using System;
using System.Windows;

using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Repositories;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Services;

public class SettingsService
{
    private SettingsWindow? _window = null;

    private readonly ApplicationManager _applicationManager;
    private readonly SettingsRepository _settingsRepository;

    public SettingsService(ApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
        _settingsRepository = new(_applicationManager.ConnectionString);
    }

    public event EventHandler? SettingsSaved;

    public void OpenSettingsWindow(Window? owner = null)
    {
        if (_window == null)
        {
            _window = new(
                this,
                _applicationManager.ApplicationSettings
            )
            {
                Owner = owner
            };
        }
    }

    public SettingsModel GetApplicationSettings()
    {
        return GetSettings(1);
    }

    public SettingsModel GetSettings(int settingsId)
    {
        return _settingsRepository.GetById(settingsId);
    }

    public void SaveSettings(SettingsModel settings)
    {
        _settingsRepository.Update(settings);
        SettingsSaved?.Invoke(null, EventArgs.Empty);
    }

    public void OnSettingsWindowClosed(object? sender, EventArgs e)
    {
        _window = null;
    }
}
