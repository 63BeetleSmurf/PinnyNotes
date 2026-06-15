using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;

using PinnyNotes.Core;
using PinnyNotes.Core.Configurations;
using PinnyNotes.Core.Enums;
using PinnyNotes.Core.Repositories;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.ViewModels;
using PinnyNotes.WpfUi.Views;
using System.IO;

namespace PinnyNotes.WpfUi;

public partial class App : Application
{
#if DEBUG
    private const bool IsDebugMode = true;
#else
    private const bool IsDebugMode = false;
#endif

    private const string UniqueEventName = (IsDebugMode) ? "176fc692-28c2-4ed0-ba64-60fbd7165018" : "b1bc1a95-e142-4031-a239-dd0e14568a3c";
    private const string UniqueMutexName = (IsDebugMode) ? "e21c6456-5a11-4f37-a08d-83661b642abe" : "a46c6290-525a-40d8-9880-c95d35a49057";

    private Mutex _mutex = null!;

    private AppMetadataService _appMetadataService = null!;
    private SettingsService _settingsService = null!;
    private NotifyIconService _notifyIconService = null!;

    private EventWaitHandle _eventWaitHandle = null!;

    private ApplicationSettingsModel _applicationSettings = null!;

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

        DatabaseConfiguration databaseConfiguration = Services.GetRequiredService<DatabaseConfiguration>();
        await DatabaseInitialiser.Initialise(databaseConfiguration.ConnectionString);

        _settingsService = Services.GetRequiredService<SettingsService>();
        await _settingsService.Load();
        _applicationSettings = _settingsService.ApplicationSettings;
        _applicationSettings.PropertyChanged += OnApplicationSettingsChanged;

        _appMetadataService = Services.GetRequiredService<AppMetadataService>();
        await _appMetadataService.Load();
        _ = Services.GetRequiredService<WindowService>();
        _notifyIconService = Services.GetRequiredService<NotifyIconService>();

        MessengerService messengerService = Services.GetRequiredService<MessengerService>();
        messengerService.Subscribe<ApplicationActionMessage>(OnApplicationActionMessage);

        // Spawn a thread which will be waiting for our event
        Thread thread = new(
            () => {
                while (_eventWaitHandle.WaitOne())
                    Current.Dispatcher.BeginInvoke(
                        () => messengerService.Publish(new ApplicationActionMessage(ApplicationAction.NewInstance))
                    );
            }
        )
        {
            // It is important mark it as background otherwise it will prevent app from exiting.
            IsBackground = true
        };

        thread.Start();

        messengerService.Publish(new ApplicationActionMessage(ApplicationAction.Start));

        ShutdownMode = (_applicationSettings.ShowNotifyIcon) ? ShutdownMode.OnExplicitShutdown : ShutdownMode.OnLastWindowClose;

        if (_settingsService.ApplicationSettings.CheckForUpdates)
        {
            DateTimeOffset date = DateTimeOffset.UtcNow;
            if (await VersionHelper.CheckForNewRelease(_appMetadataService.Metadata.LastUpdateCheck, date))
                _appMetadataService.Metadata.LastUpdateCheck = date.ToUnixTimeSeconds();
        }
    }

    public ApplicationMode ApplicationMode
    {
        get
        {
            if (_applicationMode is null)
            {
                if (IsDebugMode)
                {
                    _applicationMode = ApplicationMode.Debug;
                }
                else if (File.Exists(Path.Combine(AppContext.BaseDirectory, "portable.txt")))
                {
                    _applicationMode = ApplicationMode.Portable;
                }
                else
                {
                    _applicationMode = ApplicationMode.Normal;
                }
            }

            return _applicationMode.Value;
        }
    }
    private ApplicationMode? _applicationMode;

    public static IServiceProvider Services { get; private set; } = null!;

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(
            provider => new DatabaseConfiguration(ApplicationMode)
        );

        services.AddSingleton<SettingsRepository>();
        services.AddSingleton<AppMetadataRepository>();
        services.AddSingleton<NoteRepository>();

        services.AddSingleton<AppMetadataService>();
        services.AddSingleton<SettingsService>();

        services.AddSingleton<MessengerService>();
        services.AddSingleton<WindowService>();
        services.AddTransient<NotifyIconService>();
        services.AddSingleton<ThemeService>();

        services.AddTransient<SettingsWindow>();
        services.AddTransient<SettingsViewModel>();

        services.AddTransient<ManagementWindow>();
        services.AddTransient<ManagementViewModel>();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _appMetadataService.Save();
        await _settingsService.Save();
        _notifyIconService.Dispose();

        _mutex?.ReleaseMutex();
        _mutex?.Dispose();
        _eventWaitHandle?.Dispose();

        base.OnExit(e);
    }

    private void OnApplicationActionMessage(ApplicationActionMessage message)
    {
        if (message.Action == ApplicationAction.Close)
            Shutdown();
    }

    private void OnApplicationSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ApplicationSettingsModel.ShowNotifyIcon):
                ShutdownMode = (_applicationSettings.ShowNotifyIcon) ? ShutdownMode.OnExplicitShutdown : ShutdownMode.OnLastWindowClose;
                break;
            case nameof(ApplicationSettingsModel.StartWithWindows):
                if (_applicationSettings.StartWithWindows)
                {
                    RegistryHelper.AddToStartup();
                }
                else
                {
                    RegistryHelper.RemoveFromStartup();
                }
                break;
        }
    }
}
