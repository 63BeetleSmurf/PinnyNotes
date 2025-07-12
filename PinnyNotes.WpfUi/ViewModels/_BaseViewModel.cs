using System.ComponentModel;
using System.Runtime.CompilerServices;

using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;

public abstract class BaseViewModel(ApplicationDataService applicationData, SettingsService settings, MessengerService messenger) : INotifyPropertyChanged
{
    protected ApplicationDataService ApplicationData = applicationData;
    protected SettingsService Settings = settings;
    protected readonly MessengerService Messenger = messenger;

    public event PropertyChangedEventHandler? PropertyChanged;

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
