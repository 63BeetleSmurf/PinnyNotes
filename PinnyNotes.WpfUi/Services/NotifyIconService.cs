using H.NotifyIcon;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Services;

public class NotifyIconService : IDisposable
{
    private readonly MessengerService _messengerService;
    private readonly ApplicationSettingsModel _applicationSettings;

    private TaskbarIcon? _notifyIcon;

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

        _notifyIcon = new TaskbarIcon
        {
            IconSource = new System.Windows.Media.Imaging.BitmapImage(
                new Uri("pack://application:,,,/Images/icon.ico")
            ),
            ToolTipText = "Pinny Notes",
            Visibility = Visibility.Visible
        };

        _notifyIcon.TrayLeftMouseDown += NotifyIcon_TrayMouseLeftButtonDown;
        _notifyIcon.TrayLeftMouseDoubleClick += NotifyIcon_TrayLeftMouseDoubleClick;

        MenuItem newNoteItem = new()
        {
            Header = "New Note"
        };
        newNoteItem.Click += NewNote_Click;

        MenuItem settingsItem = new()
        {
            Header = "Settings"
        };
        settingsItem.Click += Settings_Click;

        MenuItem exitItem = new()
        {
            Header = "Exit"
        };
        exitItem.Click += Exit_Click;

        ContextMenu contextMenu = new();
        contextMenu.Items.Add(newNoteItem);
        contextMenu.Items.Add(new Separator());
        contextMenu.Items.Add(settingsItem);
        contextMenu.Items.Add(new Separator());
        contextMenu.Items.Add(exitItem);

        _notifyIcon.ContextMenu = contextMenu;

        _notifyIcon.ForceCreate();
    }

    private void DisposeNotifyIcon()
    {
        if (_notifyIcon == null)
            return;

        _notifyIcon.TrayLeftMouseDown -= NotifyIcon_TrayMouseLeftButtonDown;
        _notifyIcon.TrayLeftMouseDoubleClick -= NotifyIcon_TrayLeftMouseDoubleClick;

        _notifyIcon.Dispose();
        _notifyIcon = null;
    }

    private void NotifyIcon_TrayMouseLeftButtonDown(object? sender, RoutedEventArgs e)
    {
        _messengerService.Publish(new WindowActionMessage(WindowActions.Activate));
    }

    private void NotifyIcon_TrayLeftMouseDoubleClick(object? sender, RoutedEventArgs e)
    {
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
