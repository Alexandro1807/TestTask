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

namespace WpfTestTask
{
    /// <summary>
    /// Логика взаимодействия для Books.xaml
    /// </summary>
    public partial class Books : Window
    {
        public Books()
        {
            InitializeComponent();
        }

        private void ButtonGetBooks_Click(object sender, RoutedEventArgs e)
        {
            PSqlConnection pSqlConnection = new PSqlConnection();
            DataGridBooks.ItemsSource = pSqlConnection.GetData("SELECT * FROM \"Books\"").ItemsSource;
        }
    }
}
