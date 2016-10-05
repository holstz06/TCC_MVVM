using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.PanelCommands
{
    class AddCommand : ICommand
    {
        PanelVM viewmodel;
        public AddCommand(PanelVM viewmodel)
        {
            this.viewmodel = viewmodel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            viewmodel.Add(viewmodel.RoomNumber, viewmodel.DefaultColor, viewmodel.DefaultDepth);
        }
    }
}
