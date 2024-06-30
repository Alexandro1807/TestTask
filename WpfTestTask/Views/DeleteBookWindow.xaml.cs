using Microsoft.Win32;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
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
    /// Логика взаимодействия для DeleteBookWindow.xaml
    /// </summary>
    public partial class DeleteBookWindow : Window
    {
        public DeleteBookWindow(Book book)
        {
            InitializeComponent();
            InitializeFormToEndState(book);
        }
        private void InitializeFormToEndState(Book book)
        {
            TextBoxId.Text = book.Id.ToString();
            TextBoxName.Text = book.Name;
            TextBoxAuthor.Text = string.Join(" ", book.LastName, book.FirstName, book.MiddleName);
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            if (!Guid.TryParse(TextBoxId.Text, out Guid id)) return;
            try
            {
                using (TransactionScope t = new TransactionScope())
                {
                    CoverController.DeleteDataCoverWithoutImage(id);
                    GenreOfBookController.DeleteDataGenresOfBook(id);
                    BookController.DeleteDataBook(id);
                    t.Complete();
                }
                
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
    }
}
