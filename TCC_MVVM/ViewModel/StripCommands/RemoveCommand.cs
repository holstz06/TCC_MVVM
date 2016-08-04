using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TCC_MVVM.Model;

namespace TCC_MVVM.ViewModel.StripCommands
{
    /// <summary>
    /// Command to remove a strip from its collection
    /// </summary>
    class RemoveCommand : ICommand
    {
        /// <summary>
        /// A reference to the viewmodel that contains this command
        /// </summary>
        StripVM viewmodel;
        /// <summary>
        /// Creates a new instance of a remove command
        /// </summary>
        /// <param name="viewmodel"></param>
        public RemoveCommand(StripVM viewmodel)
        {
            this.viewmodel = viewmodel;
        }
        /// <summary>
        /// Can execute event
        /// </summary>
        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// Determines if the command can be executed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>True, if it can be executed. False, if it cannot</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            viewmodel.Remove(parameter as Strip);
        }
    }
}
