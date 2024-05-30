using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JocDame.Commands
{
    public class NonGenericCommand : ICommand
    {
        private Action m_commandTask;
        private Predicate<object> m_canExecuteTask;

        public NonGenericCommand(Action commandTask): this(commandTask, DefaultCanExecute)
        {
            m_commandTask = commandTask;
        }

        public NonGenericCommand(Action commandTask, Predicate<object> canExecuteTask)
        {
            this.m_commandTask = commandTask;
            this.m_canExecuteTask = canExecuteTask;
        }
        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }

        public bool CanExecute(object parameter)
        {
            return m_canExecuteTask != null && m_canExecuteTask(parameter);
        }

        public void Execute(object parameter)
        {
               m_commandTask();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
