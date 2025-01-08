using Avalonia;
using Avalonia.Threading;
using System;
using System.Threading;

namespace PinnyNotes.AvaloniaUi;

internal sealed class Program
{
    private static EventWaitHandle _eventWaitHandle = null!;

    private static Thread? _listenerThread;
    private static CancellationTokenSource _cancellationTokenSource = null!;
    private static CancellationToken _cancellationToken = default;

    public static event EventHandler? NewInstance;

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
#if DEBUG
        const string uniqueEventName = "176fc692-28c2-4ed0-ba64-60fbd7165018";
        const string uniqueMutexName = "e21c6456-5a11-4f37-a08d-83661b642abe";
#else
        const string uniqueEventName = "b1bc1a95-e142-4031-a239-dd0e14568a3c";
        const string uniqueMutexName = "a46c6290-525a-40d8-9880-c95d35a49057";
#endif

        using Mutex mutex = new(true, uniqueMutexName, out bool createdNew);
        _eventWaitHandle = new(false, EventResetMode.AutoReset, uniqueEventName);
        if (!createdNew)
        {
            // Notify the existing instance and exit
            _eventWaitHandle.Set();
            Dispose();
            Environment.Exit(0);
        }

        // Spawn a thread which will be waiting for our event
        StartListenerThread();

        // Proceed with starting the application if only instance
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        Dispose();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    public static void Dispose()
    {
        _cancellationTokenSource?.Cancel();
        _listenerThread?.Join();

        _eventWaitHandle?.Dispose();
        _cancellationTokenSource?.Dispose();
    }

    private static void StartListenerThread()
    {
        // Create a cancellation token source to control thread cancellation
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;

        _listenerThread = new Thread(() =>
        {
            while (true)
            {
                if (_cancellationToken.IsCancellationRequested)
                    break;

                bool waitSignaled = _eventWaitHandle.WaitOne(1000);
                if (waitSignaled)
                    Dispatcher.UIThread.InvokeAsync(RaiseNewInstanceEvent);
            }
        })
        {
            // It is important mark it as background otherwise it will prevent app from exiting.
            IsBackground = true
        };
        _listenerThread.Start();
    }

    private static void RaiseNewInstanceEvent()
        => NewInstance?.Invoke(null, EventArgs.Empty);
}
