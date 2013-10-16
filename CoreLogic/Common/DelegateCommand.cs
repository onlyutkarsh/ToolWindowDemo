using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic.Common
{
    public class DelegateCommand : System.Windows.Input.ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _method;
        public event EventHandler CanExecuteChanged;
        public DelegateCommand(Action<object> method)
            : this(method, null)
        {
        }
        public DelegateCommand(Action<object> method, Predicate<object> canExecute)
        {
            _method = method;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute(parameter);
        }
        public void Execute(object parameter)
        {
            _method.Invoke(parameter);
        }
        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            var canExecuteChanged = CanExecuteChanged;
            if (canExecuteChanged != null)
                canExecuteChanged(this, e);
        }
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }
    } 
}
