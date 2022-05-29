using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaMaps.Commands
{
    internal class RelayCommand : System.Windows.Input.ICommand
    {


        private readonly Action m_execute;
        private readonly Func<bool> m_canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            m_execute = execute;
            m_canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return m_canExecute == null ? true : m_canExecute();
        }

        public void Execute(object parameter)
        {
            m_execute();
        }

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
