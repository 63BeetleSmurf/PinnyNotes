using System;
using System.Windows;

using PinnyNotes.WpfUi.Components;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Presenters;
using PinnyNotes.WpfUi.Repositories;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi;

public class ApplicationManager
{
    private readonly App _app;

    private NotifyIconComponent _notifyIcon;

    private SettingsPresenter? _settingsPresenter;

    public readonly string ConnectionString;

    private readonly ApplicationDataRepository _applicationDataRepository;
    private readonly SettingsRepository _settingsRepository;

    public readonly ApplicationDataModel ApplicationData;
    public readonly SettingsModel ApplicationSettings;

    public ApplicationManager(App app)
    {
        _app = app;
        _app.NewInstance += OnNewInstance;
        _app.Exit += OnAppExit;

        ConnectionString = DatabaseHelper.GetConnectionString();
        DatabaseHelper.CheckDatabase(ConnectionString);
        _applicationDataRepository = new(this);
        ApplicationData = _applicationDataRepository.GetApplicationData();
        _settingsRepository = new(this);
        ApplicationSettings = _settingsRepository.GetApplicationSettings();

        _notifyIcon = new(this);
        _notifyIcon.ActivateNotes += OnActivateNotes;

        UpdateTrayIcon();
        CreateNewNote();
        CheckForNewRelease();
    }

    public event EventHandler? SettingsChanged;
    public event EventHandler? ActivateNotes;

    public void CreateNewNote(NoteModel? parent = null)
    {
        NotePresenter presenter = new(
            this,
            new NoteModel(ApplicationData, ApplicationSettings, parent),
            new NoteWindow()
        );

        presenter.ShowWindow();
    }

    public void ShowSettingsWindow(Window? owner = null)
    {
        if (_settingsPresenter == null)
        {
            _settingsPresenter = new(
                this,
                new SettingsModel(),
                new SettingsWindow()
            );
            _settingsPresenter.SettingsSaved += OnSettingsSaved;
        }

        _settingsPresenter.ShowWindow(owner);
    }

    public void CloseApplication()
        => _app.Shutdown();

    private void OnNewInstance(object? sender, EventArgs e)
    {
        CreateNewNote();
    }

    private void OnAppExit(object? sender, EventArgs e)
    {
        _applicationDataRepository.UpdateApplicationData(ApplicationData);
        _settingsRepository.UpdateSettings(ApplicationSettings);

        _notifyIcon?.Dispose();
    }

    private void OnActivateNotes(object? sender, EventArgs e)
    {
        ActivateNotes?.Invoke(sender, e);
    }

    private void OnSettingsSaved(object? sender, EventArgs e)
    {
        UpdateTrayIcon();

        SettingsChanged?.Invoke(sender, e);
    }

    private void UpdateTrayIcon()
    {
        bool isEnabled = ApplicationSettings.Application_TrayIcon;
        _notifyIcon.IsEnabled = isEnabled;
        _app.ShutdownMode = (isEnabled) ? ShutdownMode.OnExplicitShutdown : ShutdownMode.OnLastWindowClose;
    }

    private async void CheckForNewRelease()
    {
        DateTimeOffset date = DateTimeOffset.UtcNow;

        if (
            ApplicationSettings.Application_CheckForUpdates
            && (ApplicationData.LastUpdateCheck == null || ApplicationData.LastUpdateCheck < date.AddDays(-7).ToUnixTimeSeconds())
        )
        {
            ApplicationData.LastUpdateCheck = date.ToUnixTimeSeconds();

            if (await VersionHelper.IsNewVersionAvailable())
                MessageBox.Show(
                    $"A new version of Pinny Notes is available;{Environment.NewLine}https://github.com/63BeetleSmurf/PinnyNotes/releases/latest",
                    "Update available",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
        }
    }
}
