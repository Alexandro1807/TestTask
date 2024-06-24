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
        public BooksWindow()
        {
            InitializeComponent();
        }

        private void ButtonGetBooks_Click(object sender, RoutedEventArgs e)
        {
            int booksCount = BookController.SelectBooksCount();
            LabelPageMax.Content = booksCount;
            for (int i = booksCount + 1; i <= 100; i++) ComboBoxPageCount.Items.Remove(i.ToString());
            InitializeFormToEndState();
            RefreshForm();
        }

        private void InitializeFormToEndState()
        {
            LabelPageCurrentMin.Visibility = Visibility.Visible;
            LabelPageTo.Visibility = Visibility.Visible;
            LabelPageCurrentMax.Visibility = Visibility.Visible;
            LabelPageFrom.Visibility = Visibility.Visible;
            LabelPageMax.Visibility = Visibility.Visible;
            TextBoxPage.Visibility = Visibility.Visible;
            TextBoxShortcut.Visibility = Visibility.Visible;
            BorderImage.Visibility = Visibility.Visible;
            ButtonPageNext.Visibility = Visibility.Visible;
            ButtonPagePrev.Visibility = Visibility.Visible;
            ButtonGetBooks.Visibility = Visibility.Collapsed;
            _isInitialized = true;
        }

        private void RefreshForm()
        {
            int limit = int.Parse(ComboBoxPageCount.SelectedItem.ToString());
            int offset = limit * (int.Parse(TextBoxPage.Text) - 1);
            _bookList = BookController.SelectDataBooksWithFunction(limit, offset);
            DataGridBooks.ItemsSource = _bookList;
            LabelPageCurrentMin.Content = 1;
            LabelPageCurrentMax.Content = limit + offset;
        }

        private void ButtonOpenAddBookWindow_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow win = new AddBookWindow();
            win.Show();
        }

        #region События изменений на форме
        private void DataGridBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Book book = (sender as DataGrid).SelectedItem as Book;
                if (book == null) return;
                TextBoxShortcut.Text = book.Shortcut == string.Empty ? "Краткое содержание не определено." : book.Shortcut;

                if (book.CoverText != "underfined")
                    ImageCover.Source = new BitmapImage(new Uri(book.CoverText));
                else ImageCover.Source = null;
                //Починить обработку изображения через байтовый массив, после чего разблокировать
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

        private void LabelPageCurrent_Loaded(object sender, RoutedEventArgs e)
        {
            LabelPageCurrentMin.Visibility = Visibility.Hidden;
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
            for (int i = 1; i <= 100; i++) ComboBoxPageCount.Items.Add(i.ToString());
        }

        private void ComboBoxPageCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int maxPage = int.Parse(LabelPageMax.Content.ToString());
            if (maxPage == 0) return;
            int limit = int.Parse(ComboBoxPageCount.SelectedItem.ToString());
            _pageCount = (int)Math.Round((double)maxPage / limit, MidpointRounding.ToPositiveInfinity);
            TextBoxPage.Text = "1";
        }
        #endregion
    }
}
