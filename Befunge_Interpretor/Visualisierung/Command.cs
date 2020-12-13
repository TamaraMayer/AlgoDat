using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Visualisierung
{
    public class Command : ICommand
    {
        private Action<object> action;

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> action)
        {
            this.action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
