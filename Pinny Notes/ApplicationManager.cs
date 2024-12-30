using System;
using System.Windows;

using PinnyNotes.WpfUi.Components;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Repositories;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi;

public class ApplicationManager
{
    private readonly App _app;

    private NotifyIconComponent _notifyIcon;

    private readonly ApplicationDataRepository _applicationDataRepository;

    public readonly string ConnectionString;

    public readonly ApplicationDataModel ApplicationData;
    public readonly SettingsModel ApplicationSettings;

    public SettingsService SettingsService { get; }
    public ManagementService ManagementService { get; }
    public GroupService GroupService { get; }
    public NoteService NoteService { get; }

    public ApplicationManager(App app)
    {
        _app = app;
        _app.NewInstance += OnNewInstance;
        _app.Exit += OnAppExit;

        ConnectionString = DatabaseHelper.GetConnectionString();
        DatabaseHelper.CheckDatabase(ConnectionString);

        _applicationDataRepository = new(ConnectionString);
        SettingsService = new(this);
        SettingsService.SettingsSaved += OnSettingsSaved;

        ApplicationData = _applicationDataRepository.GetApplicationData();
        ApplicationSettings = SettingsService.GetApplicationSettings();

        ManagementService = new(this);
        GroupService = new(this);
        NoteService = new(this);
        SettingsService.SettingsSaved += NoteService.OnSettingsSaved;

        _notifyIcon = new(this);
        _notifyIcon.ActivateNotes += NoteService.OnActivateNotes;
    }

    public void Initialize()
    {
        UpdateTrayIcon();
        NoteService.OpenNewNote();
        CheckForNewRelease();
    }

    public void CloseApplication()
    {
        _app.Shutdown();
    }

    private void OnNewInstance(object? sender, EventArgs e)
    {
        // TO DO: Add setting to choose what happens here.
        // e.g. new note, show management window, activate notes...
        NoteService.OpenNewNote();
    }

    private void OnAppExit(object? sender, EventArgs e)
    {
        _applicationDataRepository.UpdateApplicationData(ApplicationData);

        _notifyIcon?.Dispose();
    }

    private void OnSettingsSaved(object? sender, EventArgs e)
    {
        UpdateTrayIcon();
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
