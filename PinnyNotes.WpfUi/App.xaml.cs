using System;
using System.Threading;
using System.Windows;

using PinnyNotes.WpfUi.Components;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi;

public partial class App : Application
{
#if DEBUG
    public const bool IsDebugMode = true;
#else
    public const bool IsDebugMode = false;
#endif

    private const string UniqueEventName = (IsDebugMode) ? "176fc692-28c2-4ed0-ba64-60fbd7165018" : "b1bc1a95-e142-4031-a239-dd0e14568a3c";
    private const string UniqueMutexName = (IsDebugMode) ? "e21c6456-5a11-4f37-a08d-83661b642abe" : "a46c6290-525a-40d8-9880-c95d35a49057";

    private Mutex _mutex = null!;
    private EventWaitHandle _eventWaitHandle = null!;

    private readonly MessengerService _messenger = new();

    private SettingsWindow? _settingsWindow;
    private NotifyIconComponent? NotifyIcon;

    protected override void OnStartup(StartupEventArgs e)
    {
        _mutex = new(true, UniqueMutexName, out bool createdNew);
        _eventWaitHandle = new(false, EventResetMode.AutoReset, UniqueEventName);

        if (!createdNew)
        {
            _eventWaitHandle.Set();
            Shutdown();
            return;
        }

        base.OnStartup(e);

        // Spawn a thread which will be waiting for our event
        Thread thread = new(
            () => {
                while (_eventWaitHandle.WaitOne())
                    Current.Dispatcher.BeginInvoke(
                        () => CreateNewNote()
                    );
            }
        )
        {
            // It is important mark it as background otherwise it will prevent app from exiting.
            IsBackground = true
        };

        thread.Start();

        if (Settings.Default.ShowTrayIcon)
        {
            NotifyIcon = new(_messenger);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        CreateNewNote();

        CheckForNewRelease();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        NotifyIcon?.Dispose();
        base.OnExit(e);
    }

    public void CreateNewNote()
    {
        new NoteWindow(_messenger).Show();
    }

    public void ShowSettingsWindow(Window? owner = null)
    {
        if (_settingsWindow == null || !_settingsWindow.IsLoaded)
            _settingsWindow = new SettingsWindow(_messenger);

        _settingsWindow.Owner = owner;

        if (_settingsWindow.IsVisible)
            _settingsWindow.Activate();
        else
            _settingsWindow.Show();
    }

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
