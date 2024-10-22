using System.Windows;

using Pinny_Notes.Components;
using Pinny_Notes.Properties;
using Pinny_Notes.Views;

namespace Pinny_Notes;

public partial class App : Application
{
    private SettingsWindow? _settingsWindow;
    private NotifyIconComponent? NotifyIcon;

    protected override void OnStartup(StartupEventArgs e)
    {
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

    public void ShowSettingsWindow()
    {
        if (_settingsWindow == null || !_settingsWindow.IsLoaded)
            _settingsWindow = new SettingsWindow();

        if (_settingsWindow.IsVisible)
            _settingsWindow.Activate();
        else
            _settingsWindow.Show();
    }
}
