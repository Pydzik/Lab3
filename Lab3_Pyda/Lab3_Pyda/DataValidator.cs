using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Lab3_Pyda
{
    public static class DataValidator
    {

        /// <summary>
        /// Stops the handeling of the <see cref="System.Windows.Controls.TextBox"/> input
        /// if the new character is not a digit
        /// </summary>
        /// <param name="e"></param>
        public static void HandleOnlyDigits(TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
                if (!Char.IsDigit(ch))
                    e.Handled = true;
        }

        /// <summary>
        /// Stops the handeling of the <see cref="System.Windows.Controls.TextBox"/> input
        /// if the new character is not a letter
        /// </summary>
        /// <param name="e"></param>
        public static void HandleOnlyOneWord(TextCompositionEventArgs e)
        {
                
            foreach (char ch in e.Text)
            {
                if (!Char.IsLetter(ch) || Char.IsWhiteSpace(ch))
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Stops the handeling of the <see cref="System.Windows.Controls.TextBox"/> input
        /// if the new character is not a letter
        /// </summary>
        /// <param name="e"></param>
        public static void HandleFindingWhiteSpace(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

        }

        /// <summary>
        /// Stops the handeling of the <see cref="System.Windows.Controls.TextBox"/> input
        /// if the new character is not a letter
        /// </summary>
        /// <param name="e"></param>
        public static void HandleOnlyWords(TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
                if (!Char.IsLetter(ch))
                    e.Handled = true;
        }
    }
}
