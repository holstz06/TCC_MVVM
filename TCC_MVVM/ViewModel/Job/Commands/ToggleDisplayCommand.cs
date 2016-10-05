using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Job.Commands
{
    class ToggleDisplayCommand : ICommand
    {
        JobVM viewmodel;
        public ToggleDisplayCommand(JobVM viewmodel)
        {
            this.viewmodel = viewmodel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            switch(viewmodel.DisplayState)
            {
                case 0: viewmodel.DisplayState = 1; break;
                case 1: viewmodel.DisplayState = 2; break;
                case 2: viewmodel.DisplayState = 0; break;
            }

            switch (viewmodel.DisplayState)
            {
                case 0:
                    viewmodel.IsProposalDisplayVisible = Visibility.Visible;
                    viewmodel.IsTabControlDisplayVisible = Visibility.Visible;
                    viewmodel.ProposalDisplayColumn = 1;
                    viewmodel.ProposalDisplayColumnSpan = 1;
                    viewmodel.TabControlColumnSpan = 1;
                    break;

                case 1:
                    viewmodel.IsProposalDisplayVisible = Visibility.Hidden;
                    viewmodel.IsTabControlDisplayVisible = Visibility.Visible;
                    viewmodel.ProposalDisplayColumnSpan = 1;
                    viewmodel.ProposalDisplayColumn = 1;
                    viewmodel.TabControlColumnSpan = 2;
                    break;

                case 2:
                    viewmodel.IsProposalDisplayVisible = Visibility.Visible;
                    viewmodel.IsTabControlDisplayVisible = Visibility.Hidden;
                    viewmodel.ProposalDisplayColumnSpan = 2;
                    viewmodel.ProposalDisplayColumn = 0;
                    viewmodel.TabControlColumnSpan = 1;
                    break;
            }  
        }
    }
}
