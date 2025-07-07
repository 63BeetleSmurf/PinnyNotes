using System.ComponentModel;
using System.Runtime.CompilerServices;

using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    protected SettingsService Settings;
    protected readonly MessengerService Messenger;

    public event PropertyChangedEventHandler? PropertyChanged;

    public BaseViewModel(SettingsService settings, MessengerService messenger)
    {
        Settings = settings;
        Messenger = messenger;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);

        return true;
    }

    protected void SaveSettings()
    {
        Settings.SaveSettings();
    }
}
