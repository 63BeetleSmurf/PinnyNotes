using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Windows;

using PinnyNotes.WpfUi.Components;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Factories;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Properties;
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

    public static IServiceProvider Services { get; private set; } = null!;

    private EventWaitHandle _eventWaitHandle = null!;

    private MessengerService _messenger = null!;

    private NotifyIconComponent? NotifyIcon;

    protected override void OnStartup(StartupEventArgs e)
    {
        _ = new Mutex(true, UniqueMutexName, out bool createdNew);
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

        _messenger = Services.GetRequiredService<MessengerService>();
        _messenger.Subscribe<ApplicationActionMessage>(OnApplicationActionMessage);
        _ = Services.GetRequiredService<WindowService>();

        // Spawn a thread which will be waiting for our event
        Thread thread = new(
            () => {
                while (_eventWaitHandle.WaitOne())
                    Current.Dispatcher.BeginInvoke(
                        () => _messenger.Publish(new CreateNewNoteMessage())
                    );
            }
        )
        {
            // It is important mark it as background otherwise it will prevent app from exiting.
            IsBackground = true
        };

        thread.Start();

        if (Settings.Default.ShowTrayIcon)
        {
            NotifyIcon = new();
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        _messenger.Publish(new CreateNewNoteMessage());

        CheckForNewRelease();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MessengerService>();
        services.AddSingleton<WindowService>();

        services.AddTransient<SettingsWindow>();
        services.AddTransient<SettingsViewModel>();

        services.AddSingleton<NoteViewModelFactory>();
        services.AddSingleton<NoteWindowFactory>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        NotifyIcon?.Dispose();
        base.OnExit(e);
    }

    private void OnApplicationActionMessage(ApplicationActionMessage message)
    {
        if (message.Action == ApplicationActions.Close)
            Shutdown();
    }

    private static async void CheckForNewRelease()
    {
        DateTimeOffset date = DateTimeOffset.UtcNow;

        if (Settings.Default.CheckForUpdates && Settings.Default.LastUpdateCheck < date.AddDays(-7).ToUnixTimeSeconds())
        {
            Settings.Default.LastUpdateCheck = date.ToUnixTimeSeconds();
            Settings.Default.Save();

            if (await VersionHelper.IsNewVersionAvailable())
                MessageBox.Show(
                    $"A new version of Pinny Notes is available;{Environment.NewLine}https://github.com/63BeetleSmurf/PinnyNotes/releases/latest",
                    "Update available",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
        }
    }
}
