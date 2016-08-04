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
    /// Command to add another strip to the collection
    /// </summary>
    class AddCommand : ICommand
    {
        /// <summary>
        /// Reference to the strip view model
        /// </summary>
        StripVM viewmodel;
        /// <summary>
        /// Creates a new instance of a add command
        /// </summary>
        /// <param name="viewmodel">Reference to the view model</param>
        public AddCommand(StripVM viewmodel)
        {
            this.viewmodel = viewmodel;
        }
        /// <summary>
        /// Can execute changed event
        /// </summary>
        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// Determines if the command can be executed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>True, if the command can be executed. False, if it cannot</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
        /// <summary>
        /// Calls the 'AddStrip' command from the viewmodel
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            viewmodel.AddStrip();
        }
    }
}
