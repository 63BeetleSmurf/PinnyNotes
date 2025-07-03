using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Windows.Forms;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.Components;

public class NotifyIconComponent : IDisposable
{
    private readonly MessengerService _messenger = App.Services.GetRequiredService<MessengerService>();

    private NotifyIcon _notifyIcon;

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
            _messenger.Publish(new WindowActionMessage(WindowActions.Activate));
    }

    private void NotifyIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            NewNote_Click(null, e);
    }

    private void NewNote_Click(object? sender, EventArgs e)
    {
        _messenger.Publish(new CreateNewNoteMessage());
    }

    private void Settings_Click(object? sender, EventArgs e)
    {
        _messenger.Publish(new OpenSettingsWindowMessage());
    }

    private void Exit_Click(object? sender, EventArgs e)
    {
        _messenger.Publish(new ApplicationActionMessage(ApplicationActions.Close));
    }

    public void Dispose()
    {
        _notifyIcon.Dispose();
    }
}
