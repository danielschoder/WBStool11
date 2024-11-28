using System.Windows.Input;

namespace WBStool11.Helpers;

public class AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null) : ICommand
{
    private readonly Func<Task> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Func<bool> _canExecute = canExecute;

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
        => _canExecute == null || _canExecute();

    public async void Execute(object parameter)
        => await _execute();

    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
