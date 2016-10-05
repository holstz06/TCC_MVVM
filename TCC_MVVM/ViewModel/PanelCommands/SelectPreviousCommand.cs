using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.PanelCommands
{
    class SelectPreviousCommand : ICommand
    {
        PanelVM viewmodel;
        public SelectPreviousCommand(PanelVM viewmodel)
        {
            this.viewmodel = viewmodel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if ((int)parameter > 0)
                viewmodel.SelectedPanel = viewmodel.Panels[(int)parameter - 1];
        }
    }
}
