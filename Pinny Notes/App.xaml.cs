using System;
using System.Threading;
using System.Windows;

using Pinny_Notes.Components;
using Pinny_Notes.Properties;
using Pinny_Notes.Views;

namespace Pinny_Notes;

public partial class App : Application
{
    private const string UniqueEventName = "b1bc1a95-e142-4031-a239-dd0e14568a3c";
    private const string UniqueMutexName = "a46c6290-525a-40d8-9880-c95d35a49057";

    private Mutex _mutex = null!;
    private EventWaitHandle _eventWaitHandle = null!;

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
            NotifyIcon = new();
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        CreateNewNote();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        NotifyIcon?.Dispose();
        base.OnExit(e);
    }

    public void CreateNewNote()
    {
        new NoteWindow().Show();
    }

    public void ShowSettingsWindow(Window? owner = null)
    {
        if (_settingsWindow == null || !_settingsWindow.IsLoaded)
            _settingsWindow = new SettingsWindow();

        _settingsWindow.Owner = owner;

        if (_settingsWindow.IsVisible)
            _settingsWindow.Activate();
        else
            _settingsWindow.Show();
    }
}
