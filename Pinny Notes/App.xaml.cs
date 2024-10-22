using System;
using System.Threading;
using System.Windows;

using Pinny_Notes.Components;
using Pinny_Notes.Properties;
using Pinny_Notes.Views;

namespace Pinny_Notes;

public partial class App : Application
{
    private Mutex _mutex = null!;

    private SettingsWindow? _settingsWindow;
    private NotifyIconComponent? NotifyIcon;

    protected override void OnStartup(StartupEventArgs e)
    {
        // Ensure only once instance of application is run at a time
        _mutex = new Mutex(true, "PinnyNotesMutex", out bool createdNew); // createdNew defined here
        if (!createdNew)
            Environment.Exit(0);

        base.OnStartup(e);

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
