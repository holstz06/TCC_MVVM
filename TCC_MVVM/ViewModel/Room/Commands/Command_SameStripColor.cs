using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Room.Commands
{
    class Command_SameStripColor : ICommand
    {
        private RoomVM ViewModel;
        public Command_SameStripColor(RoomVM viewmodel)
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
            if (ViewModel.IsStripSameColor)
                ViewModel.IsStripSameColor = false;
            else
                ViewModel.IsStripSameColor = true;
        }
    }
}
