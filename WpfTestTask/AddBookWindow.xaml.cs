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

namespace WpfTestTask
{
    /// <summary>
    /// Логика взаимодействия для AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public AddBookWindow()
        {
            InitializeComponent();
        }

        private void SliderYearOfProduction_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextBoxYearOfProduction.Text = SliderYearOfProduction.Value.ToString();
        }

        private void ButtonPlusOneToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value += 1;
        }
        private void ButtonPlusTenToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value += 10;
        }

        private void ButtonPlusOneHundredToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value += 100;
        }

        private void ButtonMinusOneToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value -= 1;
        }

        private void ButtonMinusTenToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value -= 10;
        }

        private void ButtonMinusOneHundredToYearOfProduction_Click(object sender, RoutedEventArgs e)
        {
            SliderYearOfProduction.Value -= 100;
        }
    }
}
