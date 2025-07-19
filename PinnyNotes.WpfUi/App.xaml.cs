using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Windows;

using PinnyNotes.WpfUi.Components;
using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Factories;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Views;
using PinnyNotes.WpfUi.ViewModels;

namespace PinnyNotes.WpfUi;

public partial class App : Application
{
#if DEBUG
    public const bool IsDebugMode = true;
#else
    public const bool IsDebugMode = false;
#endif

    private const string UniqueEventName = (IsDebugMode) ? "176fc692-28c2-4ed0-ba64-60fbd7165018" : "b1bc1a95-e142-4031-a239-dd0e14568a3c";
    private const string UniqueMutexName = (IsDebugMode) ? "e21c6456-5a11-4f37-a08d-83661b642abe" : "a46c6290-525a-40d8-9880-c95d35a49057";

    private Mutex _mutex = null!;

    public static IServiceProvider Services { get; private set; } = null!;

    private AppMetadataService _appMetadataService = null!;
    private SettingsService _settingsService = null!;

    private EventWaitHandle _eventWaitHandle = null!;

    private NotifyIconComponent? _notifyIcon;

    protected override async void OnStartup(StartupEventArgs e)
    {
        // _mutex is required to keep it in memory, using _ = new Mutex() will not work as garbage collector will dispose of it eventually.
        _mutex = new Mutex(true, UniqueMutexName, out bool createdNew);
        _eventWaitHandle = new(false, EventResetMode.AutoReset, UniqueEventName);

        if (!createdNew)
        {
            _eventWaitHandle.Set();
            Shutdown();
            return;
        }

        base.OnStartup(e);

        ServiceCollection services = new();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();


        _settingsService = Services.GetRequiredService<SettingsService>();
        _appMetadataService = Services.GetRequiredService<AppMetadataService>();
        MessengerService messengerService = Services.GetRequiredService<MessengerService>();
        messengerService.Subscribe<ApplicationActionMessage>(OnApplicationActionMessage);
        messengerService.Subscribe<SettingChangedMessage>(OnSettingChangedMessage);
        _ = Services.GetRequiredService<WindowService>();

        // Spawn a thread which will be waiting for our event
        Thread thread = new(
            () => {
                while (_eventWaitHandle.WaitOne())
                    Current.Dispatcher.BeginInvoke(
                        () => messengerService.Publish(new CreateNewNoteMessage())
                    );
            }
        )
        {
            // It is important mark it as background otherwise it will prevent app from exiting.
            IsBackground = true
        };

        thread.Start();

        if (_settingsService.ApplicationSettings.ShowNotifiyIcon)
            ShowNotifyIcon();

        messengerService.Publish(new CreateNewNoteMessage());

        if (_settingsService.ApplicationSettings.CheckForUpdates)
        {
            DateTimeOffset date = DateTimeOffset.UtcNow;
            if (await VersionHelper.CheckForNewRelease(_appMetadataService.AppData.LastUpdateCheck, date))
                _appMetadataService.AppData.LastUpdateCheck = date.ToUnixTimeSeconds();
        }
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<DatabaseService>();
        services.AddSingleton<AppMetadataService>();
        services.AddSingleton<SettingsService>();

        services.AddSingleton<MessengerService>();
        services.AddSingleton<WindowService>();

        services.AddTransient<NotifyIconComponent>();

        services.AddTransient<SettingsWindow>();
        services.AddTransient<SettingsViewModel>();

        services.AddSingleton<NoteViewModelFactory>();
        services.AddSingleton<NoteWindowFactory>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _appMetadataService.Save();
        _settingsService.Save();

        RemoveNotifyIcon();

        _mutex?.ReleaseMutex();
        _mutex?.Dispose();
        _eventWaitHandle?.Dispose();

        base.OnExit(e);
    }

    private void OnApplicationActionMessage(ApplicationActionMessage message)
    {
        if (message.Action == ApplicationActions.Close)
            Shutdown();
    }

    private void OnSettingChangedMessage(SettingChangedMessage message)
    {
        if (message.SettingName == "ShowTrayIcon")
        {
            if ((bool)message.NewValue)
            {
                ShowNotifyIcon();
            }
            else
            {
                RemoveNotifyIcon();
                ShutdownMode = ShutdownMode.OnLastWindowClose;
            }
        }
    }

    private void ShowNotifyIcon()
    {
        if (_notifyIcon != null)
            return;

        _notifyIcon = Services.GetRequiredService<NotifyIconComponent>();
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }

    private void RemoveNotifyIcon()
    {
        if (_notifyIcon == null)
            return;

        _notifyIcon.Dispose();
        _notifyIcon = null;
    }
}
