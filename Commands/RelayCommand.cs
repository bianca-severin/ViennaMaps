using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaMaps.Commands
{
    /// <summary>
    /// A command with the sole purpose of conveying its functionality to other objects by invoking delegates.
    /// The default return value for the CanExecute method is 'true'.
    /// </summary>
    internal class RelayCommand : System.Windows.Input.ICommand
    {

        // internal variables, each storing a delegate (reference to a method)
        private readonly Action m_execute;
        private readonly Func<bool> m_canExecute;

        // dispatched when RaiseCanExecuteChanged is called.
        public event EventHandler CanExecuteChanged;

        // creates a new command that can always be run.
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        // creates a new command 
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            m_execute = execute;
            m_canExecute = canExecute;
        }

        // determines whether the RelayCommand can be executed in the current state
        // if the command does not require data passing, this object can be set to NULL
        // returns True if this command can be run, False otherwise.
        public bool CanExecute(object parameter)
        {
            return m_canExecute == null ? true : m_canExecute();
        }

        // executes the RelayCommand in the current command target.
        // parameters: the data used by the command.
        // if the command does not require data passing, this object can be set to NULL.
        public void Execute(object parameter)
        {
            m_execute();
        }

        // method used to invoke the CanExecuteChanged event to indicate that
        // the return value of the CanExecute method has changed
        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
