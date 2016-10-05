using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TCC_MVVM.View
{
    /// <summary>
    /// Interaction logic for StripControl.xaml
    /// </summary>
    public partial class StripControl : UserControl
    {
        public StripControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Selects the entire textbox text on keyboard focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox)?.SelectAll();
        }

        /// <summary>
        /// Selects the entire textbox text on mouse click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textbox = (sender as TextBox);

            if (textbox != null)
            {
                if (!textbox.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    textbox.Focus();
                }
            }
        }
    }
}
