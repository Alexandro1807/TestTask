using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfTestTask.Additional
{
    /// <summary>
    /// Дополнительные функции.
    /// </summary>
    static class AdditionalFunctions
    {
        /// <summary>
        /// Добавление в ComboBox 
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="list"></param>
        public static void ComboBoxFilter(ComboBox comboBox, List<ListOfGuidAndString> list)
        {
            if (list.Count > 0)
            {
                comboBox.ItemsSource = list;
                comboBox.SelectedItem = list.First();
            }
        }
    }
}
