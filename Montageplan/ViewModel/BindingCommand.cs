using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Montageplan.ViewModel
{
    public class BindingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> action;

        public BindingCommand(Action<object> action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (this.action != null)
                this.action(parameter);
        }

    }
}
