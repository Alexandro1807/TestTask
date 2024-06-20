using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using WpfTestTask.Models;

namespace WpfTestTask
{
    /// <summary>
    /// Логика взаимодействия для AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        byte[] bookCoverImageByte = null;
        string coverText = string.Empty;
        //public delegate void MyDelegate(string expectionMessage);
        public AddBookWindow()
        {
            InitializeComponent();
        }
       
        private void ButtonAddCover_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new() { DefaultExt = ".png" };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                coverText = dialog.FileName;
                using (FileStream pgFileStream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader pgReader = new BinaryReader(new BufferedStream(pgFileStream)))
                    {
                        bookCoverImageByte = pgReader.ReadBytes(Convert.ToInt32(pgFileStream.Length));
                        LabelCoverName.Content = coverText;
                        LabelCoverName.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void ButtonAddBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LabelError.Visibility = Visibility.Hidden;
                Guid id = Guid.NewGuid();
                DateTime lastModified = DateTime.Now;
                int yearOfProduction = 0;
                if (!int.TryParse(TextBoxYearOfProduction.Text, out yearOfProduction)) yearOfProduction = DateTime.Now.Year;
                string name, firstName, lastName, middleName, isbn, shortcut, genres;
                name = TextBoxName.Text;
                firstName = TextBoxFirstName.Text;
                lastName = TextBoxLastName.Text;
                middleName = TextBoxMiddleName.Text;
                isbn = TextBoxISBN.Text;
                shortcut = TextBoxShortcut.Text;
                genres = TextBoxShortcut.Text;
                //Придумать с coverText
                //Формировать экземпляр класса Book и вызывать метод SaveDataBooks(Book)
                Book book = new Book(id, lastModified, name, firstName, lastName, middleName, yearOfProduction, isbn, shortcut, genres, coverText, bookCoverImageByte);
                WpfTestTask.Controllers.BookController.SaveDataBooks(book);
            }
            catch (Exception ex)
            {
                new Thread(() => { SetLabelErrorContentAsync(ex.Message); }).Start(); //Переделать под вызов нового потока с передачей переменной
            }
            
        }

        #region События изменений на форме
        /// <summary>
        /// Перенос значения элемента slider в элемент Textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderYearOfProduction_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextBoxYearOfProduction.Text = SliderYearOfProduction.Value.ToString();
        }

        /// <summary>
        /// Прибавление значения элемента slider на 1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPlusOneToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value += 1;
        }

        /// <summary>
        /// Прибавление значения элемента slider на 10.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPlusTenToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value += 10;
        }

        /// <summary>
        /// Прибавление значения элемента slider на 100.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPlusOneHundredToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value += 100;
        }

        /// <summary>
        /// Уменьшение значения элемента slider на 1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMinusOneToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value -= 1;
        }

        /// <summary>
        /// Уменьшение значения элемента slider на 10.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMinusTenToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value -= 10;
        }

        /// <summary>
        /// Уменьшение значения элемента slider на 100.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMinusOneHundredToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value -= 100;
        }

        /// <summary>
        /// Появление ошибки. Размещение информации об ошибке.
        /// </summary>
        /// <param name="ex"></param>
        private async void SetLabelErrorContentAsync(string expectionMessage) //Переделать под вызов нового потока
        {
            LabelError.Content = ((string)LabelError.Content).Replace("неизвестно", expectionMessage);
            LabelError.Visibility = Visibility.Visible;
            Thread.Sleep(10); //Увеличить до 10000
            LabelError.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
