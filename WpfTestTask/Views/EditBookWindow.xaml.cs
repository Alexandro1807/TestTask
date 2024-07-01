using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        #region Инициализация формы
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
            LabelCoverName.Content = "Путь: " + book.CoverText;
            ListBoxGenres.ItemsSource = GenreController.SelectDataGenresFromGenresOfBook(book.Id);

            List<Genre> genres = GenreController.SelectDataGenres(false);
            ComboBoxGenresAdd.ItemsSource = genres;
            ComboBoxGenresRemove.ItemsSource = genres;
        }

        private void ElementsOfFormTurnOnOrOffAsync(bool isTurnOn)
        {
            ButtonEditBook.IsEnabled = ButtonMinusOneHundredToYearOfProduction.IsEnabled = ButtonMinusTenToYearOfProduction.IsEnabled = ButtonMinusOneToYearOfProduction.IsEnabled = ButtonPlusOneToYearOfProduction.IsEnabled = ButtonPlusTenToYearOfProduction.IsEnabled = ButtonPlusOneHundredToYearOfProduction.IsEnabled = ButtonEditCover.IsEnabled = isTurnOn;
            TextBoxName.IsEnabled = TextBoxLastName.IsEnabled = TextBoxFirstName.IsEnabled = TextBoxMiddleName.IsEnabled = TextBoxISBN.IsEnabled = TextBoxShortcut.IsEnabled = TextBoxYearOfProduction.IsEnabled = isTurnOn;
            SliderYearOfProduction.IsEnabled = isTurnOn;
            LabelCoverName.IsEnabled = isTurnOn;
            ListBoxGenres.IsEnabled = isTurnOn;
            CheckBoxGenreUndefined.IsEnabled = isTurnOn;
            ComboBoxGenresAdd.IsEnabled = ComboBoxGenresRemove.IsEnabled = isTurnOn;
        }

        private void SuccessForm()
        {
            SetTextBoxStateContent("Успешно. Повторить процедуру?", false);
            ElementsOfFormTurnOnOrOffAsync(false);
        }
        #endregion

        #region Функции нажатия кнопок
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
                TextBoxState.Visibility = Visibility.Hidden;
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
                    GenreOfBook genreOfBook = GenreOfBookController.SelectDataGenreOfBook(id, genre.Id);
                    if (genreOfBook == null)
                        genreOfBook = new GenreOfBook(Guid.Empty, lastModified, id, genre.Id);
                    genresOfBook.Add(genreOfBook);
                }
                genresOnRow = GenreOfBookController.ConvertGenresOfBookToGenresOnRow(genresOfBook);
                coverText = ((string)LabelCoverName.Content).Replace("Путь: ", "");
                Book book = new Book(id, lastModified, name, lastName, firstName, middleName, yearOfProduction, isbn, shortcut, genresOfBook, genresOnRow, coverText, _cover);
                BookController.UpdateDataBooks(book);
                GetGenresOfBookToUpdate(genresOfBook, id);
                //CoverController.InsertDataCover(book); //Научиться сохранять байты в PostgreSQL, затем раскомментировать
                CoverController.InsertDataCoverWithoutImage(book);
                SuccessForm();
            }
            catch (Exception ex)
            {
                SetTextBoxStateContent(ex.Message, true);
            }
        }

        private void ButtonReloadWindow_Click(object sender, RoutedEventArgs e)
        {
            Book book = BookController.SelectDataBook(Guid.Parse(TextBoxId.Text.ToString()));
            if (!TextBoxId.IsEnabled) ElementsOfFormTurnOnOrOffAsync(true);
            InitializeFormToEndState(book);
        }

        private void ButtonCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Дополнительные функции
        private void GetGenresOfBookToUpdate(List<GenreOfBook> genresOfBook, Guid id)
        {
            List<GenreOfBook> genresOfBookUpdate = new List<GenreOfBook>();
            List<GenreOfBook> genresOfBookInsert = new List<GenreOfBook>();
            List<GenreOfBook> genresOfBookOriginal = GenreOfBookController.SelectDataGenresOfBook(id);
            foreach (GenreOfBook genreOfBook in genresOfBook)
            {
                if (genreOfBook.Id == Guid.Empty)
                {
                    genreOfBook.Id = Guid.NewGuid();
                    genresOfBookInsert.Add(genreOfBook);
                }
                else
                {
                    genresOfBookUpdate.Add(genreOfBook);
                    genresOfBookOriginal.Remove(genreOfBook);
                }
            }
            List<GenreOfBook> genresOfBookDelete = genresOfBookOriginal.Except(genresOfBook).ToList();
            if (genresOfBookUpdate.Count > 0) GenreOfBookController.UpdateDataGenresOfBook(genresOfBookUpdate);
            if (genresOfBookDelete.Count > 0) GenreOfBookController.DeleteDataGenresOfBook(genresOfBookDelete);
            if (genresOfBook.Count > 0) GenreOfBookController.InsertDataGenresOfBook(genresOfBook);
        }

        /// <summary>
        /// Появление ошибки. Размещение информации об ошибке.
        /// </summary>
        /// <param name="ex"></param>
        private void SetTextBoxStateContent(string message, bool isError)
        {
            if (isError)
                TextBoxState.Text = "Ошибка: " + message.Replace("\r\n", ". ").Replace(" .", "") + ". Попробуйте ещё раз.";
            else
                TextBoxState.Text = message;
            TextBoxState.Visibility = Visibility.Visible;
            SetTextBoxVisibilityCollapsedAsync(TextBoxState);
        }

        private async Task SetTextBoxVisibilityCollapsedAsync(TextBox textBox)
        {
            await Task.Delay(10000);
            textBox.Visibility = Visibility.Collapsed;
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

        private void TextBoxState_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxState.Text = string.Empty;
            TextBoxState.BorderBrush = Brushes.White;
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
                    ListBoxGenres.ItemsSource = GenreController.SelectDataGenresFromGenresOfBook(bookId);
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

        private void TextBoxName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            AdditionalFunctions.TextPreviewKeyDown(sender, e);
        }

        private void TextBoxLastName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            AdditionalFunctions.TextPreviewKeyDown(sender, e);
        }

        private void TextBoxLastName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AdditionalFunctions.AuthorPreviewTextInput(sender, e);
        }

        private void TextBoxFirstName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            AdditionalFunctions.TextPreviewKeyDown(sender, e);
        }

        private void TextBoxFirstName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AdditionalFunctions.AuthorPreviewTextInput(sender, e);
        }

        private void TextBoxMiddleName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            AdditionalFunctions.TextPreviewKeyDown(sender, e);
        }

        private void TextBoxMiddleName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AdditionalFunctions.AuthorPreviewTextInput(sender, e);
        }

        private void TextBoxISBN_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            AdditionalFunctions.ISBNPreviewKeyDown(sender, e);
        }

        private void TextBoxISBN_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AdditionalFunctions.ISBNPreviewTextInput(sender, e);
        }

        private void TextBoxShortcut_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            AdditionalFunctions.TextPreviewKeyDown(sender, e);
        }
        #endregion
    }
}
