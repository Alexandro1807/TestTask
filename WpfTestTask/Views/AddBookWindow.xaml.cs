using Microsoft.Win32;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTestTask.Additional;
using WpfTestTask.Controllers;
using WpfTestTask.Models;

namespace WpfTestTask
{
    /// <summary>
    /// Логика взаимодействия для AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        byte[] _cover = null;
        BitmapImage BM;
        public AddBookWindow()
        {
            InitializeComponent();

            List<Genre> genres = GenreController.SelectGenresToListOfGuidAndString();
            ComboBoxGenresAdd.ItemsSource = genres;
            ComboBoxGenresRemove.ItemsSource = genres;
            ListBoxGenres.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Genre", System.ComponentModel.ListSortDirection.Ascending));
        }

        #region Функции обработок кнопок
        private void ButtonAddCover_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new() { AddExtension = true, DefaultExt = ".png", Filter = "PNG (.png)|*.png|JPG (.jpg)|*.jpg|JPEG (.jpeg)|*.jpeg" };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                //encoder.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(dialog.FileName))));
                //using (MemoryStream MS = new MemoryStream())
                //{
                //    encoder.Save(MS);
                //    _cover = MS.ToArray();
                //}
                
                _cover = File.ReadAllBytes(dialog.FileName);
                //using (FileStream pgFileStream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read))
                //{
                //    using (BinaryReader pgReader = new BinaryReader(new BufferedStream(pgFileStream)))
                //    {
                //        _cover = pgReader.ReadBytes(Convert.ToInt32(pgFileStream.Length));
                //    }
                //}
                LabelCoverName.Content += dialog.FileName;
                LabelCoverName.Visibility = Visibility.Visible;
            }
        }

        private void ButtonAddBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBoxError.Visibility = Visibility.Hidden;
                Guid id = Guid.NewGuid();
                DateTime lastModified = DateTime.Now;
                int yearOfProduction = 0;
                if (!int.TryParse(TextBoxYearOfProduction.Text, out yearOfProduction)) yearOfProduction = DateTime.Now.Year;
                string name, firstName, lastName, middleName, isbn, shortcut, genresOnRow, coverText;
                List<Genre> genres = new List<Genre>();
                name = TextBoxName.Text;
                firstName = TextBoxFirstName.Text;
                lastName = TextBoxLastName.Text;
                middleName = TextBoxMiddleName.Text;
                isbn = TextBoxISBN.Text;
                shortcut = TextBoxShortcut.Text;
                genres = GenreOfBookController.SelectGenresOfBookFromListBox(ListBoxGenres.Items);
                genresOnRow = GenreOfBookController.ConvertGenresToGenresOnRow(genres);
                coverText = ((string)LabelCoverName.Content).Replace("Путь: ", "");
                Book book = new Book(id, lastModified, name, firstName, lastName, middleName, yearOfProduction, isbn, shortcut, genres, genresOnRow, coverText, _cover);
                BookController.InsertDataBooks(book);
                GenreOfBookController.InsertGenresOfBook(book);
                //CoverController.InsertCovers(book); //Научиться сохранять байты в PostgreSQL, затем раскомментировать
                CoverController.InsertCoversWithoutImage(book);
            }
            catch (Exception ex)
            {
                SetTextBoxErrorContent(ex.Message);
            }
            
        }
        #endregion

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
        private void SetTextBoxErrorContent(string expectionMessage) //Переделать под вызов нового потока
        {
            TextBoxError.Text = "Ошибка: " + expectionMessage.Replace("\r\n", ". ").Replace(" .", "") + ". Попробуйте ещё раз.";
            TextBoxError.Visibility = Visibility.Visible;
            SetTextBoxVisibilityCollapsedAsync(TextBoxError);
        }

        private async Task SetTextBoxVisibilityCollapsedAsync(TextBox textBox)
        {
            await Task.Delay(10000);
            textBox.Visibility = Visibility.Collapsed;
        }
        #endregion

        private void LabelCoverName_Loaded(object sender, RoutedEventArgs e)
        {
            LabelCoverName.Visibility = Visibility.Hidden;
        }

        private void TextBoxError_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxError.BorderBrush = Brushes.White;
        }

        private void ComboBoxGenresAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Genre item = (Genre)ComboBoxGenresAdd.SelectedItem;
            if (item != null) UpdateListBoxGenres(item, true);
        }

        private void ComboBoxGenresRemove_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Genre item = (Genre)ComboBoxGenresRemove.SelectedItem;
            if (item != null) UpdateListBoxGenres(item, false);
        }

        private void UpdateListBoxGenres(Genre item, bool isAdd)
        {
            switch (isAdd)
            {
                case true:
                    {
                        ComboBoxGenresAdd.SelectedItem = null;
                        if (ListBoxGenres.Items.IndexOf(item) == -1)
                            ListBoxGenres.Items.Add(item);
                        break;
                    }
                case false:
                    {
                        ComboBoxGenresRemove.SelectedItem = null;
                        if (ListBoxGenres.Items.IndexOf(item) != -1)
                            ListBoxGenres.Items.Remove(item);
                        break;
                    }
            }
        }

        private void CheckBoxGenreUndefined_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckBoxGenreUndefined.IsChecked)
            {
                ListBoxGenres.Items.Clear();
                ListBoxGenres.IsEnabled = false;
                ListBoxGenres.Visibility = Visibility.Hidden;
                LabelGenreAdd.Visibility = Visibility.Hidden;
                LabelGenreRemove.Visibility = Visibility.Hidden;
                ComboBoxGenresAdd.IsEnabled = false;
                ComboBoxGenresAdd.Visibility = Visibility.Hidden;
                ComboBoxGenresRemove.IsEnabled = false;
                ComboBoxGenresRemove.Visibility = Visibility.Hidden;
            }
            else
            {
                ListBoxGenres.IsEnabled = true;
                ListBoxGenres.Visibility = Visibility.Visible;
                LabelGenreAdd.Visibility = Visibility.Visible;
                LabelGenreRemove.Visibility = Visibility.Visible;
                ComboBoxGenresAdd.IsEnabled = true;
                ComboBoxGenresAdd.Visibility = Visibility.Visible;
                ComboBoxGenresRemove.IsEnabled = true;
                ComboBoxGenresRemove.Visibility = Visibility.Visible;
            }
        }
    }
}
