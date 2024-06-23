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

namespace WpfTestTask
{
    /// <summary>
    /// Логика взаимодействия для BooksWindow.xaml
    /// </summary>
    public partial class BooksWindow : Window
    {
        private BindingList<Book> _bookList;
        public BooksWindow()
        {
            InitializeComponent();
        }

        private void ButtonGetBooks_Click(object sender, RoutedEventArgs e)
        {
            _bookList = BookController.SelectDataBooks();
            DataGridBooks.ItemsSource = _bookList;
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
                TextBoxShortcut.Text = book.Shortcut == string.Empty ? "Краткое содержание не определено." : book.Shortcut;

                if (book.CoverText != "underfined")
                    ImageCover.Source = new BitmapImage(new Uri(book.CoverText));
                else ImageCover.Source = null;
                if (book.Cover != null && book.Cover.Length != 0)
                {
                    using (MemoryStream memoryStream = new MemoryStream(book.Cover))
                    {
                        memoryStream.Write(book.Cover, 0, book.Cover.Length);
                        memoryStream.Position = 0;
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        BitmapImage bitImage = new BitmapImage();
                        bitImage.BeginInit();
                        bitImage.StreamSource = memoryStream;
                        bitImage.EndInit();
                    }






                    BitmapImage image = new BitmapImage();
                    using (MemoryStream mem = new MemoryStream(book.Cover))
                    {
                        image.BeginInit();
                        image.StreamSource = mem;
                        image.EndInit();
                    }
                    image.Freeze();
                    ImageCover.Source = image;
                }
            }
            catch (Exception ex)
            {
                ImageCover.Source = null;
            }
            finally
            {

            }
            
        }
    }
}
