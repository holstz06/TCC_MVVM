using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.Room.Commands
{
    class Command_AddStrip : ICommand
    {
        private RoomVM ViewModel;
        public Command_AddStrip(RoomVM viewmodel)
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
            ViewModel.AddShelvingTo(Model.ShelvingType.Strip);
        }
    }
}
