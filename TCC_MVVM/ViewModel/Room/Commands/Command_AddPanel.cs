using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Room.Commands
{
    class Command_AddPanel : ICommand
    {
        private RoomVM ViewModel;
        public Command_AddPanel(RoomVM viewmodel)
        {
            ViewModel = viewmodel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ViewModel.AddShelvingTo(Model.ShelvingType.Panel);
        }
    }
}
