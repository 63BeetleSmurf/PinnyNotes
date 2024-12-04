using System;
using System.Windows;

using PinnyNotes.WpfUi.Components;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Presenters;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi;

public class ApplicationManager
{
    private readonly App _app;

    private NotifyIconComponent? _notifyIcon;

    private SettingsPresenter? _settingsPresenter;

    public ApplicationManager(App app)
    {
        _app = app;
        _app.NewInstance += OnNewInstance;
        _app.Exit += OnAppExit;

        if (Settings.Default.ShowTrayIcon)
        {
            _notifyIcon = new(this);
            _app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        CreateNewNote();

        CheckForNewRelease();
    }

    public event EventHandler? SettingsChanged;

    public void CreateNewNote(NoteModel? parent = null)
    {
        NotePresenter presenter = new(
            this,
            new NoteModel(parent),
            new NoteWindow()
        );

        presenter.ShowWindow();
    }

    public void ActivateNotes()
    {
        foreach (Window window in _app.Windows)
        {
            window.WindowState = WindowState.Normal;
            window.Activate();
        }
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
        _notifyIcon?.Dispose();
    }

    private void OnSettingsSaved(object? sender, EventArgs e)
        => SettingsChanged?.Invoke(sender, e);

    private async void CheckForNewRelease()
    {
        DateTimeOffset date = DateTimeOffset.UtcNow;

        if (Settings.Default.CheckForUpdates && Settings.Default.LastUpdateCheck < date.AddDays(-7).ToUnixTimeSeconds())
        {
            Settings.Default.LastUpdateCheck = date.ToUnixTimeSeconds();
            Settings.Default.Save();

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
