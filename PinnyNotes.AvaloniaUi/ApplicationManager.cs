using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.Input;
using System;

namespace PinnyNotes.AvaloniaUi;

public partial class ApplicationManager : IDisposable
{
    private readonly IClassicDesktopStyleApplicationLifetime _applicationLifetime;

    private TrayIcon? _trayIcon;

    public ApplicationManager(IClassicDesktopStyleApplicationLifetime applicationLifetime)
    {
        _applicationLifetime = applicationLifetime;

        CreateTrayIcon();
    }

    public void Dispose()
    {
        _trayIcon?.Dispose();
    }

    [RelayCommand]
    private void Shutdown()
    {
        Dispose();

        _applicationLifetime.Shutdown();
    }

    private void CreateTrayIcon()
    {
        _trayIcon = new TrayIcon
        {
            Icon = new(AssetLoader.Open(new Uri("avares://PinnyNotes.AvaloniaUi/Assets/icon.ico"))),
            ToolTipText = "Pinny Notes"
        };

        NativeMenu trayMenu = new();

        trayMenu.Items.Add(
            new NativeMenuItem("Exit")
            {
                Command = ShutdownCommand
            }
        );

        _trayIcon.Menu = trayMenu;
    }

    private void OnShutdownRequested(object? sender, EventArgs e)
        => Dispose();
}
