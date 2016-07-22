using System;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel
{
    class CollectionChangeCommand : ICommand
    {
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public CollectionChangeCommand(Action<object> execute)
            : this(execute, null)
        {

        }

        public CollectionChangeCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute), "Execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
