using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Command
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<bool> _canExecute;
        private readonly Action _executeWithoutParameter;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute)
        {
            _executeWithoutParameter = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }
            else
            {
                _executeWithoutParameter?.Invoke();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
