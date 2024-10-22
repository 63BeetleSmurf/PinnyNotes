using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

using Pinny_Notes.Views;

namespace Pinny_Notes;

public partial class App : System.Windows.Application
{
    private SettingsWindow? _settingsWindow;
    private NotifyIcon _notifyIcon = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _notifyIcon = new()
        {
            Icon = new Icon(
            GetResourceStream(new Uri("pack://application:,,,/Images/icon.ico")).Stream
            ),
            Text = "Pinny Notes",
            Visible = true
        };

        _notifyIcon.MouseClick += NotifyIcon_MouseClick;
        _notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;

        ContextMenuStrip contextMenu = new();
        contextMenu.Items.Add("New Note", null, NewNote_Click);
        contextMenu.Items.Add("-");
        contextMenu.Items.Add("Setings", null, Settings_Click);
        contextMenu.Items.Add("-");
        contextMenu.Items.Add("Exit", null, Exit_Click);

        _notifyIcon.ContextMenuStrip = contextMenu;

        NewNote_Click(null, e);
    }

    private void NotifyIcon_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            foreach (Window window in Current.Windows)
            {
                window.WindowState = WindowState.Normal;
                window.Activate();
            }
        }
    }

    private void NotifyIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            NewNote_Click(null, e);
    }

    private void NewNote_Click(object? sender, EventArgs e)
    {
        new NoteWindow().Show();
    }

    private void Settings_Click(object? sender, EventArgs e)
    {
        if (_settingsWindow == null || !_settingsWindow.IsLoaded)
            _settingsWindow = new SettingsWindow();

        if (_settingsWindow.IsVisible)
            _settingsWindow.Activate();
        else
            _settingsWindow.Show();
    }

    private void Exit_Click(object? sender, EventArgs e)
    {
        Shutdown();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _notifyIcon.Dispose();
        base.OnExit(e);
    }
}
