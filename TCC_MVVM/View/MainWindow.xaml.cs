﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace TCC_MVVM.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Create new instance of MainWindow class
        /// </summary>
        public MainWindow()
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
