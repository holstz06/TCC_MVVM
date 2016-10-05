using System;
using System.Windows;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Job.Commands
{
    public class CreateJobCommand : ICommand
    {
        private JobVM ViewModel;
        private Model.Job Job;
        public CreateJobCommand(JobVM ViewModel)
        {
            this.ViewModel = ViewModel;
            Job = this.ViewModel.Job;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
        }
    }
}
