using PinnyNotes.Core.Enums;
using PinnyNotes.Core.Repositories;
using PinnyNotes.WpfUi.Messages;
using System.Windows.Threading;

namespace PinnyNotes.WpfUi.Services;

public class ReminderService
{
    private readonly MessengerService _messengerService;
    private readonly NoteRepository _noteRepository;

    private DateTime? _nextReminderTrigger = null;
    private DispatcherTimer? _timer = null;

    public ReminderService(MessengerService messengerService, NoteRepository noteRepository)
    {
        _messengerService = messengerService;
        _noteRepository = noteRepository;

        _messengerService.Subscribe<ApplicationActionMessage>(OnApplicationActionMessage);
        _messengerService.Subscribe<ReminderSetMessage>(OnReminderSetMessage);
    }

    private void OnReminderSetMessage(ReminderSetMessage message)
    {
        if (_nextReminderTrigger is not null && message.ReminderTrigger > _nextReminderTrigger)
        {
            return;
        }

        SetReminder(message.ReminderTrigger);
    }

    private void OnApplicationActionMessage(ApplicationActionMessage message)
    {
        if (message.Action == ApplicationAction.Start)
        {
            _ = CheckReminders();
            _ = SetNextReminderTrigger();
        }
    }

    private async void OnTimerElapsed(object? sender, EventArgs e)
    {
        _timer?.Stop();

        if (_nextReminderTrigger is not null)
        {
            await CheckReminders(_nextReminderTrigger);

            await SetNextReminderTrigger();
        }

        if (_nextReminderTrigger is null)
        {
            ClearTimer();
        }
    }

    private async Task CheckReminders(DateTime? targetDateTime = null)
    {
        int[] reminderNoteIds = await _noteRepository.PopReminderTriggers(targetDateTime);

        if (reminderNoteIds.Length > 0)
        {
            _messengerService.Publish(
                new ReminderTriggerMessage(reminderNoteIds)
            );
        }
    }

    private async Task SetNextReminderTrigger()
    {
        DateTime? nextReminderTrigger = await _noteRepository.GetNextReminderTrigger();

        if (nextReminderTrigger is not null)
        {
            SetReminder(nextReminderTrigger.Value);
        }
    }

    private void SetReminder(DateTime reminderTrigger)
    {
        if (_timer is not null)
        {
            ClearTimer();
        }

        _nextReminderTrigger = reminderTrigger;

        TimeSpan timerDelay = reminderTrigger - DateTime.Now;
        _timer = new()
        {
            Interval = timerDelay
        };
        _timer.Tick += OnTimerElapsed;
        _timer.Start();
    }

    private void ClearTimer()
    {
        _nextReminderTrigger = null;

        _timer?.Stop();
        _timer = null;
    }
}
