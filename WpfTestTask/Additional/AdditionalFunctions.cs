using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WpfTestTask.Additional
{
    /// <summary>
    /// Дополнительные функции.
    /// </summary>
    public static class AdditionalFunctions
    {
        public static readonly Regex regexNumbers = new Regex("[^0-9]+");
        public static readonly Regex regexLetters = new Regex(@"^[a-zA-Zа-яА-Я]+$");
        public static readonly Regex regexLettersLowerCase = new Regex(@"^[a-zа-я]+$");
        public static readonly Regex regexLettersUpperCase = new Regex(@"^[A-ZА-Я]+$");
        public static readonly Regex regexISBN = new Regex("^(?=(?:\\D*\\d){10}(?:(?:\\D*\\d){3})?$)[\\d-]+$");
        
        public static void NumberPreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (sender as TextBox);
            if (e.Key == Key.Back)
                if (textBox.Text.Remove(textBox.Text.Length - 1, 1).Length <= 0) e.Handled = true;
        }

        public static void NumberPreviewTextInput(object sender, TextCompositionEventArgs e, int _pageCount)
        {
            TextBox textBox = (sender as TextBox);
            e.Handled = regexNumbers.IsMatch(e.Text);
            if (!e.Handled)
                if (int.Parse(textBox.Text + e.Text) > _pageCount) e.Handled = true;
        }

        public static void AuthorPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = (sender as TextBox).Text;
            e.Handled = !regexLetters.IsMatch(e.Text);
            if (e.Handled) return;
            bool isMatchedLowerCase = regexLettersLowerCase.IsMatch(e.Text);
            if (text.Length == 0)
            { if (isMatchedLowerCase) e.Handled = true; }
            else if (text.Remove(0, text.Length - 1) == " " && isMatchedLowerCase) e.Handled = true;
        }

        public static void TextPreviewKeyDown(object sender, KeyEventArgs e)
        {
            string text = (sender as TextBox).Text;
            if (e.Key == Key.Space)
                if (text.Length == 0) e.Handled = true;
                else if (text.Remove(0, text.Length - 1) == " ") e.Handled = true;
        }

        public static void ISBNPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regexNumbers.IsMatch(e.Text);
            TextBox textBox = (sender as TextBox);
            string text = textBox.Text;
            if (text.Length >= 17)
            {
                textBox.Text = text.Remove(17);
                e.Handled = true;
                return;
            }
            if (e.Handled) return;
            if (text.Length == 3 || text.Length == 5 || text.Length == 10 || text.Length == 15)
                textBox.Text += "-";
            textBox.SelectionStart = textBox.Text.Length;
        }

        public static void ISBNPreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (sender as TextBox);
            string text = textBox.Text;
            if (e.Key == Key.Back)
                if (text.Length == 4 || text.Length == 6 || text.Length == 11 || text.Length == 16)
                    (sender as TextBox).Text = text.Remove(text.Length - 1, 1);
            textBox.SelectionStart = textBox.Text.Length;
        }

        public static void AddRandomBooks(int count)
        {
            for (int i = 0; i < count; i++)
            {

            }
        }
    }
}
