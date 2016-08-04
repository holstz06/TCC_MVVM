using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TCC_MVVM.Model;

namespace TCC_MVVM.ViewModel.ShelfCommands
{
    public class AddAdjustableShelfCommand : ICommand
    {
        ShelfVM viewmodel;
        string shelfTypeName;
        public AddAdjustableShelfCommand(ShelfVM viewmodel)
        {
            this.viewmodel = viewmodel;
        }
        public event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Determines if this command can execute
        /// </summary>
        /// 
        /// <param name="parameter">
        ///     The input from 'CommandParameter'
        /// </param>
        /// 
        /// <returns>
        ///     <para>
        ///         True, if the command can execute
        ///     </para><para>
        ///         False, if the command can not execute
        ///     </para>
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        ///     Executes the command
        /// </summary>
        /// 
        /// <param name="parameter">
        ///     The input from 'CommandParameter'
        /// </param>
        public void Execute(object parameter)
        {
            Shelf shelf = (parameter as Shelf);
            Shelf newShelf = (shelf.Clone() as Shelf);
            bool isValid = false;

            switch (shelf.ShelfTypeName)
            {
                case "Fixed":
                    shelfTypeName = "Adjustable";
                    isValid = true;
                    break;

                case "Corner (Fixed)":
                    shelfTypeName = "Corner (Adj)";
                    isValid = true;
                    break;
            }

            if(isValid)
                foreach (var shelfType in viewmodel.ShelfTypes)
                {
                    if (shelfType.Name == shelfTypeName)
                    {
                        newShelf.ShelfTypeName = shelfTypeName;
                        viewmodel.Add(newShelf);
                    }
                }
        }
    }
}
