﻿using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        #region Инициализация формы
        public AddBookWindow()
        {
            InitializeComponent();
            InitializeFormToEndState(true);
        }

        private void InitializeFormToEndState(bool isFirstTime)
        {
            if (!isFirstTime)
            {
                TextBoxName.Text = TextBoxLastName.Text = TextBoxFirstName.Text = TextBoxMiddleName.Text = TextBoxISBN.Text = TextBoxShortcut.Text = string.Empty;
                SliderYearOfProduction.Value = DateTime.Now.Year;
                ListBoxGenres.Items.Clear();
                LabelCoverName.Content = string.Empty;
                CheckBoxGenreUndefined.IsChecked = false;
            }
            else
            {
                List<Genre> genres = GenreController.SelectDataGenres(false);
                ComboBoxGenresAdd.ItemsSource = genres;
                ComboBoxGenresRemove.ItemsSource = genres;
            }
        }

        private void SuccessForm()
        {
            SetTextBoxStateContent("Успешно. Повторить процедуру?", false);
            ElementsOfFormTurnOnOrOffAsync(false);
        }

        private void ElementsOfFormTurnOnOrOffAsync(bool isTurnOn)
        {
            ButtonAddBook.IsEnabled = ButtonMinusOneHundredToYearOfProduction.IsEnabled = ButtonMinusTenToYearOfProduction.IsEnabled = ButtonMinusOneToYearOfProduction.IsEnabled = ButtonPlusOneToYearOfProduction.IsEnabled = ButtonPlusTenToYearOfProduction.IsEnabled = ButtonPlusOneHundredToYearOfProduction.IsEnabled = ButtonAddCover.IsEnabled = isTurnOn;
            TextBoxName.IsEnabled = TextBoxLastName.IsEnabled = TextBoxFirstName.IsEnabled = TextBoxMiddleName.IsEnabled = TextBoxISBN.IsEnabled = TextBoxShortcut.IsEnabled = TextBoxYearOfProduction.IsEnabled = isTurnOn;
            SliderYearOfProduction.IsEnabled = isTurnOn;
            LabelCoverName.IsEnabled = isTurnOn;
            ListBoxGenres.IsEnabled = isTurnOn;
            CheckBoxGenreUndefined.IsEnabled = isTurnOn;
            ComboBoxGenresAdd.IsEnabled = ComboBoxGenresRemove.IsEnabled = isTurnOn;
        }
        #endregion

        #region Функции нажатия кнопок
        private void ButtonAddCover_Click(object sender, RoutedEventArgs e)
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

        private void ButtonAddBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBoxState.Visibility = Visibility.Hidden;
                Guid id = Guid.NewGuid();
                DateTime lastModified = DateTime.Now;
                int yearOfProduction = 0;
                if (!int.TryParse(TextBoxYearOfProduction.Text, out yearOfProduction)) yearOfProduction = DateTime.Now.Year;
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
                    genresOfBook.Add(new GenreOfBook(Guid.NewGuid(), lastModified, id, genre.Id));
                genresOnRow = GenreOfBookController.ConvertGenresOfBookToGenresOnRow(genresOfBook);
                coverText = ((string)LabelCoverName.Content).Replace("Путь: ", "");
                Book book = new Book(id, lastModified, name, lastName, firstName, middleName, yearOfProduction, isbn, shortcut, genresOfBook, genresOnRow, coverText, _cover);
                BookController.InsertDataBook(book);
                GenreOfBookController.InsertDataGenresOfBook(genresOfBook);
                //CoverController.InsertCovers(book); //Научиться сохранять байты в PostgreSQL, затем раскомментировать
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
            ElementsOfFormTurnOnOrOffAsync(true);
            InitializeFormToEndState(false);
        }

        private void ButtonCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Дополнительные функции
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

        private void LabelCoverName_Loaded(object sender, RoutedEventArgs e)
        {
            LabelCoverName.Visibility = Visibility.Hidden;
        }

        private void TextBoxState_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxState.Text = string.Empty;
            TextBoxState.BorderBrush = Brushes.White;
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
