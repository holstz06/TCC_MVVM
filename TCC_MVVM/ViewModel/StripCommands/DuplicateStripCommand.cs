using System;
using System.Windows.Input;
using TCC_MVVM.Model;

namespace TCC_MVVM.ViewModel.StripCommands
{
    /// <summary>
    /// Command to duplicate a strip in its collection
    /// </summary>
    class DuplicateStripCommand : ICommand
    {
        /// <summary>
        /// Reference to the strip view model
        /// </summary>
        StripVM viewmodel;
        /// <summary>
        /// Creates a new instance to a duplicate strip command
        /// </summary>
        /// <param name="viewmodel">Reference to the view model</param>
        public DuplicateStripCommand(StripVM viewmodel)
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
        /// Creates a duplicate of the strip and calls the 'Add' function
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            var newStrip = (parameter as Strip).Clone() as Strip;
            viewmodel.AddStrip(newStrip);
        }
    }
}
