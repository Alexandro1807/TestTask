using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfTestTask.Additional
{
    /// <summary>
    /// Дополнительные функции.
    /// </summary>
    static class AdditionalFunctions
    {
        public static readonly Regex regexNumbers = new Regex("[^0-9]+");
    }
}
