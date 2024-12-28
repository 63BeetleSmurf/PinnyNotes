using System;
using System.Windows;

using PinnyNotes.WpfUi.Components;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Presenters;
using PinnyNotes.WpfUi.Repositories;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi;

public class ApplicationManager
{
    private readonly App _app;

    private NotifyIconComponent _notifyIcon;

    private ManagementPresenter? _managementPresenter;
    private SettingsPresenter? _settingsPresenter;

    private readonly ApplicationDataRepository _applicationDataRepository;
    private readonly SettingsRepository _settingsRepository;

    public readonly string ConnectionString;

    public readonly ApplicationDataModel ApplicationData;
    public readonly SettingsModel ApplicationSettings;

    public NoteService NoteService {  get; }

    public ApplicationManager(App app)
    {
        _app = app;
        _app.NewInstance += OnNewInstance;
        _app.Exit += OnAppExit;

        ConnectionString = DatabaseHelper.GetConnectionString();
        DatabaseHelper.CheckDatabase(ConnectionString);
        _applicationDataRepository = new(ConnectionString);
        _settingsRepository = new(ConnectionString);

        ApplicationData = _applicationDataRepository.GetApplicationData();
        ApplicationSettings = _settingsRepository.GetApplicationSettings();

        NoteService = new(this);

        _notifyIcon = new(this);
        _notifyIcon.ActivateNotes += OnActivateNotes;
    }

    public void Initialize()
    {
        UpdateTrayIcon();
        NoteService.OpenNewNote();
        CheckForNewRelease();
    }

    public void ShowManagementWindow()
    {
        if (_managementPresenter == null)
            _managementPresenter = new(
                this,
                new ManagementModel(),
                new ManagementWindow()
            );

        _managementPresenter.ShowWindow();
    }

    public void CloseManagementWindow()
    {
        _managementPresenter = null;
    }

    public void ShowSettingsWindow(Window? owner = null)
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

    public void CloseApplication()
        => _app.Shutdown();

    private void OnNewInstance(object? sender, EventArgs e)
    {
        // TO DO: Add setting to choose what happens here.
        // e.g. new note, show management window, activate notes...
        NoteService.OpenNewNote();
    }

    private void OnAppExit(object? sender, EventArgs e)
    {
        _applicationDataRepository.UpdateApplicationData(ApplicationData);
        _settingsRepository.UpdateSettings(ApplicationSettings);

        _notifyIcon?.Dispose();
    }

    private void OnActivateNotes(object? sender, EventArgs e)
    {
        NoteService.ActivateNoteWindows();
    }

    private void OnSettingsSaved(object? sender, EventArgs e)
    {
        UpdateTrayIcon();

        NoteService.ReloadNoteSettings();
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
