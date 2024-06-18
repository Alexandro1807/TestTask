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
            _bookList = BookController.GetDataBooks();
            DataGridBooks.ItemsSource = _bookList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow win = new AddBookWindow();
            win.Show();
        }
    }
}
