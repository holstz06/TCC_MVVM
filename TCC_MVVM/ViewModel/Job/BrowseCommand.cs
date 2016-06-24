using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Job
{
    class BrowseCommand : ICommand
    {
        private JobVM ViewModel;
        public BrowseCommand(JobVM ViewModel)
        {
            this.ViewModel = ViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            // There is no instance where this command cannot be ran
            return true;
        }

        public void Execute(object parameter)
        {

        }
    }
}
