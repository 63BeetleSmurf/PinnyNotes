using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Services;

public class NotifyIconService : IDisposable
{
    private readonly MessengerService _messengerService;
    private readonly ApplicationSettingsModel _applicationSettings;

    private NotifyIcon? _notifyIcon;

    private bool _disposed;

    public NotifyIconService(MessengerService messengerService, SettingsService settingsService)
    {
        _messengerService = messengerService;

        _applicationSettings = settingsService.ApplicationSettings;
        _applicationSettings.PropertyChanged += OnApplicationSettingsChanged;

        if (_applicationSettings.ShowNotifiyIcon)
            InitializeNotifyIcon();
    }

    private void OnApplicationSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ApplicationSettingsModel.ShowNotifiyIcon))
        {
            if (_applicationSettings.ShowNotifiyIcon && _notifyIcon == null)
                InitializeNotifyIcon();
            else if (!_applicationSettings.ShowNotifiyIcon && _notifyIcon != null)
                DisposeNotifyIcon();
        }
    }

    private void InitializeNotifyIcon()
    {
        if (_notifyIcon != null)
            return;

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
        contextMenu.Items.Add("Settings", null, Settings_Click);
        contextMenu.Items.Add("-");
        contextMenu.Items.Add("Exit", null, Exit_Click);

        _notifyIcon.ContextMenuStrip = contextMenu;
    }

    private void DisposeNotifyIcon()
    {
        if (_notifyIcon == null)
            return;

        _notifyIcon.MouseClick -= NotifyIcon_MouseClick;
        _notifyIcon.MouseDoubleClick -= NotifyIcon_MouseDoubleClick;

        _notifyIcon.Dispose();
        _notifyIcon = null;
    }

    private void NotifyIcon_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            _messengerService.Publish(new WindowActionMessage(WindowActions.Activate));
    }

    private void NotifyIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            NewNote_Click(null, e);
    }

    private void NewNote_Click(object? sender, EventArgs e)
    {
        _messengerService.Publish(new CreateNewNoteMessage());
    }

    private void Settings_Click(object? sender, EventArgs e)
    {
        _messengerService.Publish(new OpenSettingsWindowMessage());
    }

    private void Exit_Click(object? sender, EventArgs e)
    {
        _messengerService.Publish(new ApplicationActionMessage(ApplicationActions.Close));
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _applicationSettings.PropertyChanged -= OnApplicationSettingsChanged;

        DisposeNotifyIcon();

        _disposed = true;
    }
}
