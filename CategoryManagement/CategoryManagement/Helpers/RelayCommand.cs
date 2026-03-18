using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CategoryManagement.Helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }
        public void Execute(object? parameter)
        {
            _execute();
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (parameter is T typed)
                return _canExecute?.Invoke(typed) ?? true;

            if (parameter != null)
            {
                try
                {
                    var converted = (T)Convert.ChangeType(parameter, typeof(T));
                    return _canExecute?.Invoke(converted) ?? true;
                }
                catch { }
            }

            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is T typed)
            {
                _execute(typed);
            }
            else if (parameter != null)
            {
                try
                {
                    var converted = (T)Convert.ChangeType(parameter, typeof(T));
                    _execute(converted);
                }
                catch { }
            }
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
