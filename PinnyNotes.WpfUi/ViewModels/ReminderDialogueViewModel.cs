using PinnyNotes.WpfUi.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PinnyNotes.WpfUi.ViewModels;

public class ReminderDialogueViewModel
{
    private bool _isUpdate = false;

    public ReminderDialogueViewModel(DateTime? reminderTrigger)
    {
        _isUpdate = (reminderTrigger is not null);

        DateTime displayDate = reminderTrigger ?? DateTime.Now;

        TriggerDate = displayDate; // DateOnly.FromDateTime(displayDate).AddDays(1);
        TriggerHour = displayDate.Hour;
        TriggerMinute = displayDate.Minute;

        OkCommand = new(() => { CloseAction?.Invoke(true); });
        CancelCommand = new(() => { CloseAction?.Invoke(false); });
    }

    public RelayCommand OkCommand { get; }
    public RelayCommand CancelCommand { get; }

    public Action<bool>? CloseAction { get; set; }

    public string WindowTitle => (_isUpdate) ? "Update Reminder" : "Set Reminder";
    public string OkButtonText => (_isUpdate) ? "Update" : "Set";

    public DateTime TriggerDate { get; set => SetProperty(ref field, value); }
    public int TriggerHour { get; set => SetProperty(ref field, value); }
    public int TriggerMinute { get; set => SetProperty(ref field, value); }

    public DateTime ReminderTrigger
        => DateOnly.FromDateTime(TriggerDate).ToDateTime(new TimeOnly(TriggerHour, TriggerMinute));

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);

        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
