using System;
using System.Threading;
using System.Windows;

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
    private EventWaitHandle _eventWaitHandle = null!;

    public ApplicationManager ApplicationManager = null!;

    public event EventHandler? NewInstance;

    protected override void OnStartup(StartupEventArgs e)
    {
        _mutex = new(true, UniqueMutexName, out bool createdNew);
        _eventWaitHandle = new(false, EventResetMode.AutoReset, UniqueEventName);
        if (!createdNew)
        {
            _eventWaitHandle.Set();
            Shutdown();
            return;
        }

        base.OnStartup(e);

        ApplicationManager = new(this);
        ApplicationManager.Initialize();

        // Spawn a thread which will be waiting for our event
        Thread thread = new(
            () => {
                while (_eventWaitHandle.WaitOne())
                    Current.Dispatcher.BeginInvoke(RaiseNewInstanceEvent);
            }
        )
        {
            // It is important mark it as background otherwise it will prevent app from exiting.
            IsBackground = true
        };
        thread.Start();
    }

    private void RaiseNewInstanceEvent()
        => NewInstance?.Invoke(this, EventArgs.Empty);
}
