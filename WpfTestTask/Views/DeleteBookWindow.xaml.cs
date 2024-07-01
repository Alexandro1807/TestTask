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
            LabelState.Content = "Вы отказались от удаления книги!";
            CloseWindowAsync(2000);
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
                LabelState.Content = "Удаление книги прошло успешно!";
            }
            catch (Exception ex)
            {
                LabelState.Content = "Ошибка: " + ex.Message;
            }
            finally
            {
                CloseWindowAsync(5000);
            }
        }

        /// <summary>
        /// Асинхронное закрытие формы с обратным отсчётом в дополнительной строке состояния
        /// </summary>
        /// <param name="label"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private async Task CloseWindowAsync(int delay)
        {
            ButtonYes.IsEnabled = false;
            ButtonNo.IsEnabled = false;
            LabelBackTimer.Visibility = Visibility.Visible;
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            while (stopwatch.IsRunning)
            {
                LabelBackTimer.Content = $"Выход через: " + await Task.Run(() => { return ((delay - stopwatch.Elapsed.TotalMilliseconds) / 1000).ToString("F3"); });
                if (stopwatch.Elapsed.TotalMilliseconds >= delay) stopwatch.Stop();
            }
            this.Close();
        }

        private void LabelState_Loaded(object sender, RoutedEventArgs e)
        {
            LabelState.Content = string.Empty;
        }

        private void LabelBackTimer_Loaded(object sender, RoutedEventArgs e)
        {
            LabelBackTimer.Visibility = Visibility.Hidden;
        }
    }
}
