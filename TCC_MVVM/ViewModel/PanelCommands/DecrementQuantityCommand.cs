using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.PanelCommands
{
    class DecrementQuantityCommand : ICommand
    {
        PanelVM viewmodel;
        public DecrementQuantityCommand(PanelVM viewmodel)
        {
            this.viewmodel = viewmodel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if(viewmodel.Panels[(int)parameter].Quantity > 1 && (int)parameter >= 0)
                viewmodel.Panels[(int)parameter].Quantity -= 1;
        }
    }
}