using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTestTask.Models;

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

        /// <summary>
        /// Добавление случайных записей для наполнения базы
        /// </summary>
        /// <param name="count"></param>
        public static void AddRandomBooks(int count)
        {
            string[] users = System.IO.File.ReadAllLines("D:\\temp\\Рандом.csv");
            string[] names = System.IO.File.ReadAllLines("D:\\temp\\названия.txt");
            string[] shortcuts = System.IO.File.ReadAllLines("D:\\temp\\описания.txt");
            for (int i = 0; i < count; i++)
            {
                string[] user = users[(new Random().Next(count) + count) % 500].Split(';');
                Guid id = Guid.NewGuid();
                DateTime lastModified = DateTime.Parse($"{new Random().Next(1, 29)}.{new Random().Next(1, 13)}.{new Random().Next(1997, 2025)} {new Random().Next(0, 24)}:{new Random().Next(0, 60)}:{new Random().Next(0, 60)}");
                string name = names[(new Random().Next(count) + count) % 250];
                string lastName = user[0];
                string firstName = user[1];
                string middleName = user[2];
                int yearOfProduction = Math.Max(new Random().Next(2100), int.Parse(user[3]));
                string isbn = $"{new Random().Next(0, 10)}{new Random().Next(0, 10)}{new Random().Next(0, 10)}-{new Random().Next(0, 10)}-{new Random().Next(0, 10)}{new Random().Next(0, 10)}{new Random().Next(0, 10)}{new Random().Next(0, 10)}-{new Random().Next(0, 10)}{new Random().Next(0, 10)}{new Random().Next(0, 10)}{new Random().Next(0, 10)}-{new Random().Next(0, 10)}";
                string shortCut = "";
                for (int j = 0; j < 5; j++)
                    shortCut += new Random().Next(0, 1) == 0 ? shortcuts[new Random().Next(32)] : "";
                List<GenreOfBook> genresOfBook = null;
                string genresOnRow = null;
                string coverText = null;
                byte[] cover = null;
                Book book = new Book(id, lastModified, name, lastName, firstName, middleName, yearOfProduction, isbn, shortCut, genresOfBook, genresOnRow, coverText, cover);
            }
        }
    }
}
