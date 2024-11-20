using PinnyNotes.WpfUi.Views;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace PinnyNotes.WpfUi.Components;

public class NotifyIconComponent : IDisposable
{
    private NotifyIcon _notifyIcon;
    private App _app = (App)System.Windows.Application.Current;

    public NotifyIconComponent()
    {
        _notifyIcon = new()
        {
            Icon = new Icon(
                    App.GetResourceStream(new Uri("pack://application:,,,/Images/icon.ico")).Stream
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
    }

    private void NotifyIcon_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            foreach (Window window in _app.Windows)
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
        _app.ShowSettingsWindow();
    }

    private void Exit_Click(object? sender, EventArgs e)
    {
        _app.Shutdown();
    }

    public void Dispose()
    {
        _notifyIcon.Dispose();
    }
}
