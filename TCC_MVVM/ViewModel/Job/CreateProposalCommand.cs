using System;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Job
{
    class CreateProposalCommand : ICommand
    {
        JobVM ViewModel;
        public CreateProposalCommand(JobVM ViewModel)
        {
            this.ViewModel = ViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.CreateProposal();
        }
    }
}
