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

    public void OpenSettingsWindow(Window? owner = null)
    {
        if (_settingsPresenter == null)
        {
            _settingsPresenter = new(
                this,
                ApplicationSettings,
                new SettingsWindow()
            );
            _settingsPresenter.SettingsSaved += OnSettingsSaved;
        }

        _settingsPresenter.ShowWindow(owner);
    }

    public SettingsModel GetApplicationSettings()
    {
        return _settingsRepository.GetById(1);
    }

    public void SaveSettings(SettingsModel settings)
    {
        _settingsRepository.Update(settings);
    }
}
