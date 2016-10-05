using System;
using System.Windows.Input;

namespace TCC_MVVM.ViewModel.PanelCommands
{
    class RemoveCommand : ICommand
    {
        PanelVM viewmodel;
        public RemoveCommand(PanelVM viewmodel)
        {
            this.viewmodel = viewmodel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            viewmodel.Remove(parameter as Model.Panel);

            // Attempt to select the next item
            if (viewmodel.SelectedPanelIndex < viewmodel.Panels.Count - 1)
                viewmodel.SelectedPanelIndex += 1;
            else
            {
                // If not select the previous one
                if (viewmodel.SelectedPanelIndex > 0)
                    viewmodel.SelectedPanelIndex += 1;
            }
        }
    }
}
