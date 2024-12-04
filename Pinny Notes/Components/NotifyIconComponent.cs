using System;
using System.Drawing;
using System.Windows.Forms;

namespace PinnyNotes.WpfUi.Components;

public class NotifyIconComponent : IDisposable
{
    private NotifyIcon? _notifyIcon;
    private ApplicationManager _applicationManager;

    public NotifyIconComponent(ApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
    }

    private bool isEnabled;
    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            if (isEnabled != value)
            {
                isEnabled = value;
                if (isEnabled)
                    EnableIcon();
                else
                    DisableIcon();
            }
        }
    }

    public void Dispose()
        => DisableIcon();

    private void EnableIcon()
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

    private void DisableIcon()
    {
        _notifyIcon?.Dispose();
        _notifyIcon = null;
    }

    private void NotifyIcon_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
            _applicationManager.ActivateNotes();
    }

    private void NotifyIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            NewNote_Click(null, e);
    }

    private void NewNote_Click(object? sender, EventArgs e)
        => _applicationManager.CreateNewNote();

    private void Settings_Click(object? sender, EventArgs e)
        => _applicationManager.ShowSettingsWindow();

    private void Exit_Click(object? sender, EventArgs e)
        => _applicationManager.CloseApplication();
}
