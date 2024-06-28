using System;
using System.Collections.Generic;
using System.Data;
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
using System.ComponentModel;
using WpfTestTask.Controllers;
using System.IO;
using WpfTestTask.Additional;
using System.Windows.Media.Media3D;
using System.Threading;

namespace WpfTestTask
{
    /// <summary>
    /// Логика взаимодействия для BooksWindow.xaml
    /// </summary>
    public partial class BooksWindow : Window
    {
        private BindingList<Book> _bookList;
        private bool _isInitialized = false;
        private int _pageCount = 1;
        private bool _isRefreshingFilterFlag = false;
        private Timer _refreshingFilterTimer = null;
        public BooksWindow()
        {
            InitializeComponent();

            List<Genre> genres = GenreController.SelectGenresWithUndefinedToList();
            ComboBoxGenresFilter.ItemsSource = genres;
        }
        #region Инициализация формы
        #endregion
        #region Функции нажатия кнопок
        #endregion
        #region Дополнительные функции
        #endregion
        #region События изменений на форме
        #endregion
        private void ButtonGetBooks_Click(object sender, RoutedEventArgs e)
        {
            int booksCount = BookController.SelectBooksCount();
            LabelPageMax.Content = booksCount;
            ComboBoxPageCount.Items.Remove(15);
            for (int i = 1; i <= Math.Min(booksCount, 100); i++) ComboBoxPageCount.Items.Add(i.ToString());
            ComboBoxPageCount.SelectedItem = Math.Min(booksCount, 15).ToString();
            InitializeFormToEndState();
            RefreshForm();
        }

        private void InitializeFormToEndState()
        {
            ComboBoxPageCount.IsEnabled = true;
            LabelISBN.Visibility = Visibility.Visible;
            LabelPageCurrentMin.Visibility = Visibility.Visible;
            LabelPageTo.Visibility = Visibility.Visible;
            LabelPageCurrentMax.Visibility = Visibility.Visible;
            LabelPageFrom.Visibility = Visibility.Visible;
            LabelPageMax.Visibility = Visibility.Visible;
            TextBoxPage.Visibility = Visibility.Visible;
            TextBoxShortcut.Visibility = Visibility.Visible;
            TextBoxISBN.Visibility = Visibility.Visible;
            BorderImage.Visibility = Visibility.Visible;
            ButtonPageNext.Visibility = Visibility.Visible;
            ButtonPagePrev.Visibility = Visibility.Visible;
            ButtonOpenAddBookWindow.Visibility = Visibility.Visible;
            ButtonOpenEditBookWindow.Visibility = Visibility.Visible;
            ButtonOpenDeleteBookWindow.Visibility = Visibility.Visible;
            GroupBoxFilters.Visibility = Visibility.Visible;
            ButtonGetBooks.Visibility = Visibility.Collapsed;
            _isInitialized = true;
        }

        private void RefreshForm()
        {
            //Анимация обновления
            RefreshAnimationStartAsync();
            
            lock (string.Empty) _isRefreshingFilterFlag = false;

            //Установка фильтров
            string nameFilter = TextBoxNameFilter.Text != "" ? TextBoxNameFilter.Text : "undefined";
            string authorFilter = TextBoxAuthorFilter.Text != "" ? TextBoxAuthorFilter.Text : "undefined";
            string genreFilter = ComboBoxGenresFilter.Text != "" ? ComboBoxGenresFilter.Text : "undefined";
            int yearOfProductionFilter = -1;
            if (!int.TryParse(TextBoxYearOfProductionFilter.Text, out yearOfProductionFilter)) ; //Продумать действия при ошибке присваивания

            int limit = int.Parse(ComboBoxPageCount.SelectedItem.ToString());
            int offset = limit * (int.Parse(TextBoxPage.Text) - 1);
            int rowCount = 0;
            _bookList = BookController.SelectDataBooksWithFunction(limit, offset, out rowCount);
            DataGridBooks.ItemsSource = _bookList;
            ImageCover.Source = null;
            TextBoxShortcut.Text = string.Empty;
            TextBoxISBN.Text = string.Empty;
            LabelPageCurrentMin.Content = offset + 1;
            LabelPageCurrentMax.Content = offset + rowCount;
            RefreshAnimationStopAsync();
        }

        private async void RefreshAnimationStartAsync()
        {
            _isInitialized = false;
            ElementsOfFormTurnOnOrOffAsync();
            while (!_isInitialized)
            {
                string refresh = "Обновление";
                string[] strings = { ".", "..", "...", "" };
                foreach (string s in strings)
                {
                    await Task.Delay(100);
                    TextBlockRefresh.Text = refresh + s;
                }
            }
        }
        
        private async void RefreshAnimationStopAsync()
        {
            await Task.Delay(2000); //Имитация ожидания обработки сервера под высокой нагрузкой
            _isInitialized = true;
            ElementsOfFormTurnOnOrOffAsync();
        }

        private async void ElementsOfFormTurnOnOrOffAsync()
        {
            await Task.Delay(1);
            Visibility visibility = !_isInitialized ? Visibility.Visible : Visibility.Hidden;
            RectangleRefresh.Visibility = visibility;
            TextBlockRefresh.Visibility = visibility;
            ButtonPagePrev.IsEnabled = TextBoxPage.IsEnabled = ButtonPageNext.IsEnabled = TextBoxShortcut.IsEnabled = TextBoxISBN.IsEnabled = BorderImage.IsEnabled = GroupBoxFilters.IsEnabled = _isInitialized;
        }

        private void ButtonOpenAddBookWindow_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow win = new AddBookWindow();
            win.Show();
        }

        private void DataGridBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Book book = (sender as DataGrid).SelectedItem as Book;
                if (book == null) return;
                TextBoxShortcut.Text = book.Shortcut == string.Empty ? "Краткое содержание не определено." : book.Shortcut;
                TextBoxISBN.Text = book.ISBN == string.Empty ? "Не определено." : book.ISBN;
                if (book.CoverText != "underfined")
                    { ImageCover.Source = new BitmapImage(new Uri(book.CoverText)); TextBlockImageNotFound.Visibility = Visibility.Hidden; }
                else { ImageCover.Source = null; TextBlockImageNotFound.Visibility = Visibility.Visible; }
                //Починить обработку изображения через байтовый массив, после чего раскомментировать
                //if (book.Cover != null && book.Cover.Length != 0)
                //{
                //    using (MemoryStream memoryStream = new MemoryStream(book.Cover))
                //    {
                //        memoryStream.Write(book.Cover, 0, book.Cover.Length);
                //        memoryStream.Position = 0;
                //        memoryStream.Seek(0, SeekOrigin.Begin);
                //        BitmapImage bitImage = new BitmapImage();
                //        bitImage.BeginInit();
                //        bitImage.StreamSource = memoryStream;
                //        bitImage.EndInit();
                //    }
                //    BitmapImage image = new BitmapImage();
                //    using (MemoryStream mem = new MemoryStream(book.Cover))
                //    {
                //        image.BeginInit();
                //        image.StreamSource = mem;
                //        image.EndInit();
                //    }
                //    image.Freeze();
                //    ImageCover.Source = image;
                //}
            }
            catch (Exception ex)
            {
                ImageCover.Source = null;
            }            
        }

        private void ButtonPageNext_Click(object sender, RoutedEventArgs e)
        {
            int textBoxPage = int.Parse(TextBoxPage.Text);
            if (textBoxPage < _pageCount) TextBoxPage.Text = string.Format($"{textBoxPage + 1}");
        }

        private void ButtonPagePrev_Click(object sender, RoutedEventArgs e)
        {
            int textBoxPage = int.Parse(TextBoxPage.Text);
            if (textBoxPage > 1) TextBoxPage.Text = string.Format($"{textBoxPage - 1}");
        }

        private void ButtonPageNext_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonPageNext.Content = ">>";
            ButtonPageNext.Visibility = Visibility.Hidden;
        }

        private void ButtonPagePrev_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonPagePrev.Content = "<<";
            ButtonPagePrev.Visibility = Visibility.Hidden;
        }

        private void LabelPageCurrent_Loaded(object sender, RoutedEventArgs e)
        {
            LabelPageCurrentMin.Visibility = Visibility.Hidden;
        }

        private void TextBoxPage_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxPage.Visibility = Visibility.Hidden;
        }

        private void TextBoxPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = AdditionalFunctions.regexNumbers.IsMatch(e.Text);
            if (!e.Handled)
                if (int.Parse(TextBoxPage.Text + e.Text) > _pageCount) e.Handled = true;
        }

        private void TextBoxPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
                if (TextBoxPage.Text.Remove(TextBoxPage.Text.Length - 1, 1).Length <= 0) e.Handled = true;
        }


        private void TextBoxPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxPage.Text == "") return;
            if (_isInitialized) RefreshForm(); //Обновление формы (повторный вызов функции)
        }

        private void LabelPageCurrentMax_Loaded(object sender, RoutedEventArgs e)
        {
            LabelPageCurrentMax.Visibility = Visibility.Hidden;
        }

        private void LabelPageMax_Loaded(object sender, RoutedEventArgs e)
        {
            LabelPageMax.Visibility = Visibility.Hidden;
        }

        private void LabelTo_Loaded(object sender, RoutedEventArgs e)
        {
            LabelPageTo.Visibility = Visibility.Hidden;
        }

        private void LabelFrom_Loaded(object sender, RoutedEventArgs e)
        {
            LabelPageFrom.Visibility = Visibility.Hidden;
        }

        private void TextBoxShortcut_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxShortcut.Text = string.Empty;
            TextBoxShortcut.Visibility = Visibility.Hidden;
        }

        private void BorderImage_Loaded(object sender, RoutedEventArgs e)
        {
            BorderImage.Visibility = Visibility.Hidden;
        }

        private void ButtonOpenAddBookWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonOpenAddBookWindow.Visibility = Visibility.Hidden;
        }

        private void ComboBoxPageCount_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxPageCount.Items.Add(15);
            ComboBoxPageCount.SelectedItem = 15;
            ComboBoxPageCount.IsEnabled = false;
        }

        private void ComboBoxPageCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            int maxPage = int.Parse(LabelPageMax.Content.ToString());
            if (maxPage == 0) return;
            int limit = int.Parse(ComboBoxPageCount.SelectedItem.ToString());
            _pageCount = (int)Math.Round((double)maxPage / limit, MidpointRounding.ToPositiveInfinity);
            if (TextBoxPage.Text == "1") TextBoxPage.Text = "";
            TextBoxPage.Text = "1";
        }

        private void GroupBoxFilters_Loaded(object sender, RoutedEventArgs e)
        {
            GroupBoxFilters.Visibility = Visibility.Hidden;
        }

        private void ButtonOpenEditBookWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonOpenEditBookWindow.Visibility = Visibility.Hidden;
        }

        private void ButtonOpenDeleteBookWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonOpenDeleteBookWindow.Visibility = Visibility.Hidden;
        }

        private void ButtonOpenEditBookWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonOpenDeleteBookWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonClearFilters_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LabelISBN_Loaded(object sender, RoutedEventArgs e)
        {
            LabelISBN.Visibility = Visibility.Hidden;
        }

        private void TextBoxISBN_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxISBN.Visibility = Visibility.Hidden;
        }

        private void TextBlockImageNotFound_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockImageNotFound.Visibility = Visibility.Hidden;
        }

        private void TextBoxNameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshingFilterTimerAsync();
        }

        private void TextBoxAuthorFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshingFilterTimerAsync();
        }

        private void ComboBoxGenresFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshingFilterTimerAsync();
        }

        private void TextBoxYearOfProductionFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshingFilterTimerAsync();
        }

        private async void RefreshingFilterTimerAsync()
        {
            lock (string.Empty) _isRefreshingFilterFlag = false;
            if (_refreshingFilterTimer != null) _refreshingFilterTimer.Dispose();
            _refreshingFilterTimer = new Timer(new TimerCallback(RefreshingFilterTimerSuccess), TextBoxNameFilter, 500, Timeout.Infinite);
            await Task.Delay(500);
            if (_isRefreshingFilterFlag) RefreshForm();
        }

        private void RefreshingFilterTimerSuccess(object timerState)
        {
            lock (string.Empty) _isRefreshingFilterFlag = true;
        }

        private void RectangleRefresh_Loaded(object sender, RoutedEventArgs e)
        {
            RectangleRefresh.Visibility = Visibility.Hidden;
        }

        private void TextBlockRefresh_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockRefresh.Visibility = Visibility.Hidden;
        }
    }
}
