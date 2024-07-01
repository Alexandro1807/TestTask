using System.Transactions;
using System.Windows;
using WpfTestTask.Controllers;
using WpfTestTask.Models;

namespace WpfTestTask.Views
{
    /// <summary>
    /// Логика взаимодействия для DeleteBookWindow.xaml
    /// </summary>
    public partial class DeleteBookWindow : Window
    {
        #region Инициализация формы
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
        #endregion

        #region Функции нажатия кнопок
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
        #endregion

        #region Дополнительные функции
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
        #endregion

        #region События изменений на форме
        private void LabelState_Loaded(object sender, RoutedEventArgs e)
        {
            LabelState.Content = string.Empty;
        }

        private void LabelBackTimer_Loaded(object sender, RoutedEventArgs e)
        {
            LabelBackTimer.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
