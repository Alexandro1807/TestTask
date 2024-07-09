using System;
using System.Collections.Generic;
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
using WpfTestTask.Additional;

namespace WpfTestTask.Views
{
    /// <summary>
    /// Логика взаимодействия для AdditionalFunctionsWindow.xaml
    /// </summary>
    public partial class AdditionalFunctionsWindow : Window
    {
        public AdditionalFunctionsWindow()
        {
            InitializeComponent();
        }

        private void ButtonAddRandomBooks_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TextBoxAddRandomBooks.Text, out int count))
                LabelAddRandomBooks.Content = AdditionalFunctions.AddRandomBooks(count);
        }
    }
}
