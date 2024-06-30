using Microsoft.Win32;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTestTask.Additional;
using WpfTestTask.Controllers;
using WpfTestTask.Models;

namespace WpfTestTask.Views
{
    /// <summary>
    /// Логика взаимодействия для EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window
    {
        byte[] _cover = null;
        public EditBookWindow(Book book)
        {
            InitializeComponent();
            InitializeFormToEndState(book);
        }

        private void InitializeFormToEndState(Book book)
        {
            TextBoxId.Text = book.Id.ToString();
            TextBoxName.Text = book.Name;
            TextBoxLastName.Text = book.LastName;
            TextBoxFirstName.Text = book.FirstName;
            TextBoxMiddleName.Text = book.MiddleName;
            TextBoxISBN.Text = book.ISBN;
            TextBoxShortcut.Text = book.Shortcut;
            SliderYearOfProduction.Value = book.YearOfProduction;
            LabelCoverName.Content += book.CoverText;
            ListBoxGenres.ItemsSource = GenreController.SelectGenresFromGenresOfBookData(book.Id);

            List<Genre> genres = GenreController.SelectGenresData(false);
            ComboBoxGenresAdd.ItemsSource = genres;
            ComboBoxGenresRemove.ItemsSource = genres;
        }

        private void ButtonEditCover_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new() { AddExtension = true, DefaultExt = ".png", Filter = "PNG (.png)|*.png|JPG (.jpg)|*.jpg|JPEG (.jpeg)|*.jpeg" };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                //ПОПЫТКИ СОХРАНИТЬ ИЗОБРАЖЕНИЕ В ВИДЕ БАЙТОВОГО МАССИВА

                //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                //encoder.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(dialog.FileName))));
                //using (MemoryStream MS = new MemoryStream())
                //{
                //    encoder.Save(MS);
                //    _cover = MS.ToArray();
                //}
                //using (FileStream pgFileStream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read))
                //{
                //    using (BinaryReader pgReader = new BinaryReader(new BufferedStream(pgFileStream)))
                //    {
                //        _cover = pgReader.ReadBytes(Convert.ToInt32(pgFileStream.Length));
                //    }
                //}

                _cover = File.ReadAllBytes(dialog.FileName);
                LabelCoverName.Content = "Путь: " + dialog.FileName;
                LabelCoverName.Visibility = Visibility.Visible;
            }
        }

        private void ButtonEditBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBoxError.Visibility = Visibility.Hidden;
                if (!Guid.TryParse(TextBoxId.Text, out Guid id)) return;
                DateTime lastModified = DateTime.Now;
                if (!int.TryParse(TextBoxYearOfProduction.Text, out int yearOfProduction)) yearOfProduction = DateTime.Now.Year;
                string name, firstName, lastName, middleName, isbn, shortcut, genresOnRow, coverText;
                name = TextBoxName.Text;
                firstName = TextBoxFirstName.Text;
                lastName = TextBoxLastName.Text;
                middleName = TextBoxMiddleName.Text;
                isbn = TextBoxISBN.Text;
                shortcut = TextBoxShortcut.Text;
                List<Genre> genres = GenreController.SelectGenresFromListBox(ListBoxGenres.Items);
                List<GenreOfBook> genresOfBook = new List<GenreOfBook>();
                foreach (Genre genre in genres)
                {
                    GenreOfBook genreOfBook = GenreOfBookController.SelectGenreOfBookData(id, genre.Id);
                    if (genreOfBook == null)
                        genreOfBook = new GenreOfBook(Guid.Empty, lastModified, id, genre.Id);
                    genresOfBook.Add(genreOfBook);
                }
                genresOnRow = GenreOfBookController.ConvertGenresOfBookToGenresOnRow(genresOfBook);
                coverText = ((string)LabelCoverName.Content).Replace("Путь: ", "");
                Book book = new Book(id, lastModified, name, lastName, firstName, middleName, yearOfProduction, isbn, shortcut, genresOfBook, genresOnRow, coverText, _cover);
                BookController.UpdateDataBooks(book);
                //Запихнуть весь следующий код в
                //GenreOfBookController.UpdateGenresOfBookData(genresOfBook);
                List<GenreOfBook> genresOfBookOriginal = GenreOfBookController.SelectGenresOfBookData(book.Id);
                foreach (GenreOfBook genreOfBook in genresOfBook)
                {
                    if (genreOfBook.Id == Guid.Empty)
                        genreOfBook.Id = Guid.NewGuid();
                    else
                    {
                        GenreOfBookController.UpdateGenreOfBookData(genreOfBook);
                        genresOfBook.Remove(genreOfBook);
                        genresOfBookOriginal.Remove(genreOfBook);
                    }
                }
                genresOfBookOriginal.RemoveRange(genresOfBook);
                GenreOfBookController.DeleteGenresOfBookData(genresOfBookOriginal);
                GenreOfBookController.InsertGenresOfBookData(genresOfBook);
                
                
                
                //CoverController.UpdateCovers(book); //Научиться сохранять байты в PostgreSQL, затем раскомментировать
                CoverController.UpdateCoversWithoutImage(book);
            }
            catch (Exception ex)
            {
                SetTextBoxErrorContent(ex.Message);
            }

        }

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

        private void TextBoxError_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxError.Text = string.Empty;
            TextBoxError.BorderBrush = Brushes.White;
        }

        private void ComboBoxGenresAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Genre genre = (Genre)ComboBoxGenresAdd.SelectedItem;
            if (genre != null) UpdateListBoxGenres(genre, true);
        }

        private void ComboBoxGenresRemove_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Genre genre = (Genre)ComboBoxGenresRemove.SelectedItem;
            if (genre != null) UpdateListBoxGenres(genre, false);
        }

        private void UpdateListBoxGenres(Genre genre, bool isAdd)
        {
            List<Genre> genres = GenreController.SelectGenresFromListBox(ListBoxGenres.Items);
            switch (isAdd)
            {
                case true:
                    {
                        ComboBoxGenresAdd.SelectedItem = null;
                        if (!genres.Any(g => g.Id == genre.Id))
                            genres.Add(genre);
                        break;
                    }
                case false:
                    {
                        ComboBoxGenresRemove.SelectedItem = null;
                        if (genres.Any(g => g.Id == genre.Id))
                            genres.Remove(genres.First(g => g.Id == genre.Id));
                        break;
                    }
            }
            ListBoxGenres.ItemsSource = genres;
        }

        private void CheckBoxGenreUndefined_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckBoxGenreUndefined.IsChecked)
            {
                ListBoxGenres.ItemsSource = new List<Genre>();
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
                if (Guid.TryParse(TextBoxId.Text, out Guid bookId))
                    ListBoxGenres.ItemsSource = GenreController.SelectGenresFromGenresOfBookData(bookId);
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
