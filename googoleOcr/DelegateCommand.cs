using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace googoleOcr
{
    class DelegateCommand:ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        public void Execute(object parameter)
        {
            this.execute();
        }

        private Action execute;
        private Func<bool> canExecute;

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");

            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void Execute()
        {
            this.execute();
        }

        public bool CanExecute()
        {
            return this.canExecute();
        }
    }
}
