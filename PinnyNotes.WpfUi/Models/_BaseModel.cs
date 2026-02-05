using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PinnyNotes.WpfUi.Models;

public abstract class BaseModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsSaved
    {
        get;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged(nameof(IsSaved));
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);

        IsSaved = false;

        return true;
    }
}
