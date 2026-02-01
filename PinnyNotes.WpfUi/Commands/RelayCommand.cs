using System.Windows.Input;

namespace PinnyNotes.WpfUi.Commands;

public class RelayCommand<T>(Action<T> execute, Predicate<T>? canExecute = null) : ICommand
{
    private readonly Action<T> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Predicate<T>? _canExecute = canExecute;

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || (parameter is T t && _canExecute(t));
    }

    public void Execute(object? parameter)
    {
        if (parameter is T t)
        {
            _execute(t);
        }
    }

    public event EventHandler? CanExecuteChanged;

    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
{
    private readonly Action _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Func<bool>? _canExecute = canExecute;

    public bool CanExecute(object? parameter)
        => _canExecute == null || _canExecute();

    public void Execute(object? parameter)
        => _execute();

    public event EventHandler? CanExecuteChanged;

    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
